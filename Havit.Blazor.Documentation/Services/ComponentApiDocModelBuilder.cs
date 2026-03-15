using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Havit.Blazor.Documentation.Model;
using LoxSmoke.DocXml;
using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Services;

public class ComponentApiDocModelBuilder : IComponentApiDocModelBuilder
{
	private static readonly Dictionary<string, string> s_inputBaseSummaries = new()
	{
		["AdditionalAttributes"] = "A collection of additional attributes that will be applied to the created element.",
		["Value"] = "Value of the input. This should be used with two-way binding.",
		["ValueExpression"] = "An expression that identifies the bound value.",
		["ValueChanged"] = "A callback that updates the bound value.",
		["ChildContent"] = "Content of the component.",
		["Enabled"] = "When <code>null</code> (default), the Enabled value is received from cascading <code>FormState</code>.\n"
			+ "When value is <code>false</code>, input is rendered as disabled.\n"
			+ "To set multiple controls as disabled use <code>HxFormState</code>.",
		["DisplayName"] = "Gets or sets the display name for this field.<br/>This value is used when generating error messages when the input value fails to parse correctly."
	};

	private static readonly List<string> s_ignoredMethods = new()
	{
		"ToString",
		"GetType",
		"Equals",
		"GetHashCode",
		"ReferenceEquals",
		"Dispose",
		"DisposeAsync",
		"SetParametersAsync",
		"ChildContent"
	};

	private static readonly List<Type> s_attributesForMethodFiltering = new()
	{
		typeof(JSInvokableAttribute),
		typeof(CompilerGeneratedAttribute)
	};

	private const BindingFlags CommonBindingFlags = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

	private readonly IDocXmlProvider _docXmlProvider;

	public ComponentApiDocModelBuilder(IDocXmlProvider docXmlProvider)
	{
		_docXmlProvider = docXmlProvider;
	}

	public ComponentApiDocModel BuildModel(Type type)
	{
		var model = new ComponentApiDocModel();
		model.Type = type;
		model.IsDelegate = ApiTypeHelper.IsDelegate(type);

		DocXmlReader reader = LoadDocXmlReaderBasedOnNamespace(type.Namespace);

		MapClassModel(reader, model);

		if (model.IsDelegate)
		{
			AdjustDelegate(model);
		}
		else
		{
			MapProperties(reader, model);
			MapMethods(reader, model);
			MapEnum(reader, model);
		}

		return model;
	}

	private DocXmlReader LoadDocXmlReaderBasedOnNamespace(string typeNamespace)
	{
		if (string.IsNullOrEmpty(typeNamespace))
		{
			return _docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.Bootstrap.xml");
		}
		else if (typeNamespace.Contains("Havit.Blazor.GoogleTagManager"))
		{
			return _docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.GoogleTagManager.xml");
		}
		else if (typeNamespace.Contains("Smart"))
		{
			return _docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.Bootstrap.Smart.xml");
		}
		else if (typeNamespace.Contains("ECharts"))
		{
			return _docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.ECharts.xml");
		}
		else if (typeNamespace.Contains("Bootstrap"))
		{
			return _docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.Bootstrap.xml");
		}
		else if (typeNamespace == "Havit.Blazor.Components.Web")
		{
			return _docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.xml");
		}
		else
		{
			return _docXmlProvider.GetDocXmlReaderFor("Havit.Blazor.Components.Web.Bootstrap.xml");
		}
	}

	private void AdjustDelegate(ComponentApiDocModel model)
	{
		Contract.Requires<InvalidOperationException>(model.IsDelegate);

		MethodInfo invokeMethodInfo = model.Type.GetMethod("Invoke");
		string returnType = string.Empty;

		var genericTypeArgument = invokeMethodInfo.ReturnType.GetGenericArguments().FirstOrDefault();

		if (genericTypeArgument is not null)
		{
			string genericTypeArgumentName = genericTypeArgument.ToString();
			Console.WriteLine("genericTypeArgument.ToString(): " + genericTypeArgumentName);

			returnType = $"Task&lt;{ApiRenderer.FormatType(genericTypeArgumentName, true)}&gt; ";
		}
		else
		{
			returnType = ApiRenderer.FormatType(invokeMethodInfo.ReturnType, true);
		}

		model.DelegateSignature = $"{returnType} {ApiRenderer.FormatType(model.Type, false)} (";
		foreach (ParameterInfo param in invokeMethodInfo.GetParameters())
		{
			model.DelegateSignature += $"{ApiRenderer.FormatType(param.ParameterType)} {param.Name}";
		}
		model.DelegateSignature += ")";
	}

	private void MapEnum(DocXmlReader reader, ComponentApiDocModel model)
	{
		model.IsEnum = model.Type.IsEnum;
		if (!model.IsEnum)
		{
			return;
		}

		string[] names = model.Type.GetEnumNames();
		var values = model.Type.GetEnumValues();
		EnumComments enumComments = reader.GetEnumComments(model.Type);
		for (int i = 0; i < names.Length; i++)
		{
			var enumMember = new EnumModel();
			enumMember.Name = names[i];
			enumMember.Value = (int)values.GetValue(i);
			try
			{
				var enumValueComment = enumComments.ValueComments
					.Where(o => o.Name == enumMember.Name)
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

	private void MapClassModel(DocXmlReader reader, ComponentApiDocModel model)
	{
		model.Class = new ClassModel()
		{
			Comments = reader.GetTypeComments(model.Type)
		};
	}

	private void MapProperties(DocXmlReader reader, ComponentApiDocModel model)
	{
		List<PropertyInfo> propertyInfos = model.Type.GetProperties(CommonBindingFlags).ToList();

		// Generic components have their defaults stored in a separate non-generic class to simplify access, therefore, we have to load this classes properties as well.
		if (model.Type.IsGenericType)
		{
			Type nongenericType = Type.GetType($"Havit.Blazor.Components.Web.Bootstrap.{ApiRenderer.RemoveSpecialCharacters(model.Type.Name)}, Havit.Blazor.Components.Web.Bootstrap");
			if (nongenericType is not null)
			{
				propertyInfos = propertyInfos.Concat(nongenericType.GetProperties(CommonBindingFlags)).ToList();
			}
		}

		foreach (var propertyInfo in propertyInfos)
		{
			var newProperty = new PropertyModel();
			newProperty.PropertyInfo = propertyInfo;
			newProperty.Comments = reader.GetMemberComments(propertyInfo);

			if (string.IsNullOrWhiteSpace(newProperty.Comments.Summary))
			{
				if (s_inputBaseSummaries.TryGetValue(newProperty.PropertyInfo.Name, out string summary))
				{
					newProperty.Comments.Summary = summary;
				}
			}

			if (IsEventCallback(newProperty))
			{
				model.Events.Add(newProperty);
			}
			else if (propertyInfo.GetCustomAttribute<ParameterAttribute>() is not null)
			{
				newProperty.EditorRequired = (propertyInfo.GetCustomAttribute<EditorRequiredAttribute>() is not null);
				model.Parameters.Add(newProperty);
			}
			else if (newProperty.IsStatic)
			{
				model.StaticProperties.Add(newProperty);
			}
			else
			{
				model.Properties.Add(newProperty);
			}
		}
	}

	private void MapMethods(DocXmlReader reader, ComponentApiDocModel model)
	{
		foreach (var methodInfo in model.Type.GetMethods(CommonBindingFlags))
		{
			if (ShouldIncludeMethod(methodInfo))
			{
				var newMethod = new MethodModel();
				newMethod.MethodInfo = methodInfo;
				newMethod.Comments = reader.GetMethodComments(methodInfo);

				if (newMethod.MethodInfo.IsStatic)
				{
					model.StaticMethods.Add(newMethod);
				}
				else
				{
					model.Methods.Add(newMethod);
				}
			}
		}
	}

	private bool ShouldIncludeMethod(MethodInfo methodInfo)
	{
		foreach (var attribute in s_attributesForMethodFiltering)
		{
			if (methodInfo.GetCustomAttribute(attribute) is not null)
			{
				return false;
			}
		}

		// Check if the method is a property accessor, ...
		if (methodInfo.IsSpecialName)
		{
			return false;
		}

		string name = methodInfo.Name;
		if (s_ignoredMethods.Contains(name))
		{
			return false;
		}

		return true;
	}

	// ...

	private bool IsEventCallback(PropertyModel property)
	{
		string propertyName = property.PropertyInfo.Name;

		// SomethingChanged should be treated as regular property
		// OnSomethingChanged should be treated as event-callback
		if (propertyName.EndsWith("Changed") && !Regex.IsMatch(propertyName, @"^On[A-Z]"))
		{
			return false;
		}

		return (property.PropertyInfo.PropertyType.IsGenericType && (property.PropertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(EventCallback<>)))
			|| property.PropertyInfo.PropertyType == typeof(EventCallback);
	}
}
