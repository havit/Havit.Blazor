using System.Reflection;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Model;
using LoxSmoke.DocXml;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

public class ComponentApiDocModelBuilder : IComponentApiDocModelBuilder
{
	private Type type;
	private ComponentApiDocModel model;

	private static readonly Dictionary<string, string> inputBaseSummaries = new()
		{
			{ "AdditionalAttributes", "A collection of additional attributes that will be applied to the created element." },
			{ "Value", "Value of the input. This should be used with two-way binding." },
			{ "ValueExpression", "An expression that identifies the bound value." },
			{ "ValueChanged", "A callback that updates the bound value." },
			{ "ChildContent", "Content of the component." }
		};

	private static readonly List<string> byDefaultExcludedProperties = new() { "JSRuntime", "SetParametersAsync" };
	private static readonly List<string> objectDerivedMethods = new() { "ToString", "GetType", "Equals", "GetHashCode", "ReferenceEquals" };
	private static readonly List<string> derivedMethods = new() { "Dispose", "DisposeAsync", "SetParametersAsync", "ChildContent" };
	private BindingFlags bindingFlags = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

	private readonly IDocXmlProvider docXmlProvider;

	public ComponentApiDocModelBuilder(IDocXmlProvider docXmlProvider)
	{
		this.docXmlProvider = docXmlProvider;
	}

	public ComponentApiDocModel BuildModel(Type type, bool isDelegate)
	{
		this.type = type;

		model = new ComponentApiDocModel();
		model.IsDelegate = isDelegate;

		DocXmlReader reader = LoadDocXmlReaderBasedOnNamespace(this.type.Namespace);

		model.Class = GetClassModel(reader);
		var propertiesExtracted = GetProperties(reader);
		model.Properties = propertiesExtracted.properties;
		var parameters = propertiesExtracted.parameters;
		model.Parameters = parameters.OrderByDescending(p => p.EditorRequired).ToList();

		model.StaticProperties = propertiesExtracted.staticProperties;
		model.Events = propertiesExtracted.events;

		var methodsExtracted = GetMethods(reader);
		model.Methods = methodsExtracted.methods;
		model.StaticMethods = methodsExtracted.staticMethods;

		HandleEnum(reader);
		HandleDelegate();

		return model;
	}

	private DocXmlReader LoadDocXmlReaderBasedOnNamespace(string typeNamespace)
	{
		if (string.IsNullOrEmpty(typeNamespace))
		{
			return docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.Bootstrap.xml");
		}
		else if (typeNamespace.Contains("Havit.Blazor.GoogleTagManager"))
		{
			return docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.GoogleTagManager.xml");
		}
		else if (typeNamespace.Contains("Bootstrap"))
		{
			return docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.Bootstrap.xml");
		}
		else if (typeNamespace == "Havit.Blazor.Components.Web")
		{
			return docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.xml");
		}
		else
		{
			return docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.Bootstrap.xml");
		}
	}

	private void HandleDelegate()
	{
		if (!model.IsDelegate)
		{
			return;
		}

		MethodInfo method = type.GetMethod("Invoke");
		model.DelegateSignature = $"{ApiRenderer.FormatType(method.ReturnType.ToString())} {ApiRenderer.FormatType(type, asLink: false)}(";
		foreach (ParameterInfo param in method.GetParameters())
		{
			model.DelegateSignature += $"{ApiRenderer.FormatType(param.ParameterType)} {param.Name}";
		}
		model.DelegateSignature += ")";
	}

	private void HandleEnum(DocXmlReader reader)
	{
		model.IsEnum = type.IsEnum;
		if (!model.IsEnum)
		{
			return;
		}

		string[] names = type.GetEnumNames();
		EnumComments enumComments = reader.GetEnumComments(type);
		for (int i = 0; i < names.Length; i++)
		{
			EnumModel enumMember = new();
			enumMember.Name = names[i];
			try { enumMember.Index = (int)Enum.Parse(type, enumMember.Name); } catch { }
			try
			{
				var enumValueComment = enumComments.ValueComments
					.Where(o => o.Value == i)
					.ToList()
					.FirstOrDefault(c => !string.IsNullOrEmpty(c.Summary));

				if (enumValueComment is not null)
				{
					enumMember.Summary = enumValueComment.Summary;
				}
			}
			catch { }
			model.EnumMembers.Add(enumMember);
		}
	}

	private ClassModel GetClassModel(DocXmlReader reader)
	{
		return new() { Comments = reader.GetTypeComments(type) };
	}

	private (List<PropertyModel> properties, List<PropertyModel> parameters, List<PropertyModel> staticProperties, List<PropertyModel> events) GetProperties(DocXmlReader reader)
	{
		List<PropertyModel> typeProperties = new();
		List<PropertyModel> parameters = new();
		List<PropertyModel> staticProperties = new();
		List<PropertyModel> events = new();

		List<PropertyInfo> propertyInfos = type.GetProperties(bindingFlags).ToList();

		// Generic components have their defaults stored in a separate non-generic class to simplify access, therefore, we have to load this classes properties as well.
		if (type.IsGenericType)
		{
			Type nongenericType = Type.GetType($"Havit.Blazor.Components.Web.Bootstrap.{ApiRenderer.RemoveSpecialCharacters(type.Name)}, Havit.Blazor.Components.Web.Bootstrap");
			if (nongenericType is not null)
			{
				propertyInfos = propertyInfos.Concat(nongenericType.GetProperties(bindingFlags)).ToList();
			}
		}

		foreach (var property in propertyInfos)
		{
			PropertyModel newProperty = new();
			newProperty.PropertyInfo = property;

			if (DetermineWhetherPropertyShouldBeAdded(newProperty) == false)
			{
				continue;
			}

			newProperty.Comments = reader.GetMemberComments(property);
			if (string.IsNullOrWhiteSpace(newProperty.Comments.Summary))
			{
				inputBaseSummaries.TryGetValue(newProperty.PropertyInfo.Name, out string summary);
				if (summary is not null)
				{
					newProperty.Comments.Summary = summary;
				}

				if (string.IsNullOrWhiteSpace(newProperty.Comments.Summary))
				{
					newProperty.Comments = docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.xml").GetMemberComments(property);
					if (string.IsNullOrWhiteSpace(newProperty.Comments.Summary))
					{
						newProperty.Comments = docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.Bootstrap.xml").GetMemberComments(property);
					}
				}
			}

			if (string.IsNullOrEmpty(newProperty.Comments.Summary))
			{
				// newProperty.Comments = FindInheritDoc(newProperty, reader); TO-DO
			}

			if (IsEvent(newProperty))
			{
				events.Add(newProperty);
			}
			else if (HasParameterAttribute(newProperty, out bool editorRequired))
			{
				newProperty.EditorRequired = editorRequired;
				parameters.Add(newProperty);
			}
			else if (newProperty.IsStatic)
			{
				staticProperties.Add(newProperty);
			}
			else
			{
				typeProperties.Add(newProperty);
			}
		}

		return (typeProperties, parameters, staticProperties, events);
	}

	private (List<MethodModel> methods, List<MethodModel> staticMethods) GetMethods(DocXmlReader reader)
	{
		List<MethodModel> typeMethods = new();
		List<MethodModel> staticMethods = new();

		foreach (var method in type.GetMethods(bindingFlags))
		{
			MethodModel newMethod = new();
			newMethod.MethodInfo = method;
			newMethod.Comments = reader.GetMethodComments(method);

			if (DetermineWhetherMethodShouldBeAdded(newMethod))
			{
				if (newMethod.MethodInfo.IsStatic)
				{
					staticMethods.Add(newMethod);
				}
				else
				{
					typeMethods.Add(newMethod);
				}
			}
		}

		return (typeMethods, staticMethods);
	}

	private bool DetermineWhetherPropertyShouldBeAdded(PropertyModel property)
	{
		string name = property.PropertyInfo.Name;
		if (byDefaultExcludedProperties.Contains(name))
		{
			return false;
		}

		return true;
	}

	private bool DetermineWhetherMethodShouldBeAdded(MethodModel method)
	{
		// don't add a method if it is JSInvokable
		var customAttributes = method.MethodInfo.CustomAttributes.ToList();
		foreach (var attribute in customAttributes)
		{
			if (attribute.AttributeType == typeof(Microsoft.JSInterop.JSInvokableAttribute))
			{
				return false;
			}
		}

		string name = method.MethodInfo.Name;
		if (name.StartsWith("set") || name.StartsWith("get") || objectDerivedMethods.Contains(name) || derivedMethods.Contains(name))
		{
			return false;
		}

		return true;
	}

	private bool HasParameterAttribute(PropertyModel property, out bool editorRequired)
	{
		var customAttributes = property.PropertyInfo.CustomAttributes.ToList();

		bool hasParameterAttribute = false;
		editorRequired = false;

		foreach (var attribute in customAttributes)
		{
			if (attribute.AttributeType == typeof(ParameterAttribute))
			{
				hasParameterAttribute = true;
			}
			else if (attribute.AttributeType == typeof(EditorRequiredAttribute))
			{
				editorRequired = true;
			}
		}

		return hasParameterAttribute;
	}

	private bool IsEvent(PropertyModel property)
	{
		return property.PropertyInfo.PropertyType == typeof(EventCallback<>) || property.PropertyInfo.PropertyType == typeof(EventCallback);
	}

	private CommonComments FindInheritDoc(PropertyModel property, DocXmlReader reader)
	{
		Type[] interfaces = type.GetInterfaces();

		foreach (var currentInterface in interfaces)
		{
			var matchingMember = currentInterface.GetMembers().Where(o => o.Name == property.PropertyInfo.Name).FirstOrDefault();
			return reader.GetMemberComments(matchingMember);
		}

		return null;
	}
}
