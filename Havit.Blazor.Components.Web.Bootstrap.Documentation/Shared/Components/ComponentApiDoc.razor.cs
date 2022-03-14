using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using System.Xml.XPath;
using LoxSmoke.DocXml;
using System.Text.RegularExpressions;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
	public partial class ComponentApiDoc
	{
		private static readonly (string type, string name)[] typeNames =
		{
			new() { type = "Int16",   name = "short"   },
			new() { type = "UInt16",  name = "ushort"  },
			new() { type = "Int32",   name = "int"     },
			new() { type = "UInt32",  name = "uint"    },
			new() { type = "Int64",   name = "long"    },
			new() { type = "UInt64",  name = "ulong"   },
			new() { type = "Boolean", name = "bool"    },
			new() { type = "String",  name = "string"  },
			new() { type = "Char",    name = "char"    },
			new() { type = "Decimal", name = "decimal" },
			new() { type = "Double",  name = "double"  },
			new() { type = "Single",  name = "float"   },
			new() { type = "Byte",    name = "byte"    },
			new() { type = "Sbyte",   name = "sbyte"   },
			new() { type = "Void",    name = "void"    },
			new() { type = "Object",  name = "object"  }
		};

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

		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter] public RenderFragment MainContent { get; set; }

		[Parameter] public RenderFragment CssVariables { get; set; }

		/// <summary>
		/// A type to generate the documentation for
		/// </summary>
		[Parameter] public Type Type { get; set; }

		/// <summary>
		/// Names of members that will be excluded from the displayed documentation
		/// </summary>
		[Parameter] public List<string> ExcludedMembers { get; set; } = new();

		[Parameter] public bool Delegate { get; set; }

		private DocXmlReader webReader;
		private DocXmlReader bootstrapReader;

		private ClassMember classMember;

		private List<Property> properties = new();
		private List<Property> parameters = new();
		private List<Property> staticProperties = new();
		private List<Property> events = new();

		private List<Method> methods = new();
		private List<Method> staticMethods = new();

		private List<EnumMember> enumMembers = new();

		private string delegateSignature;

		private bool isEnum;

		private BindingFlags bindingFlags = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

		protected override void OnParametersSet()
		{
			DownloadFileAndGetSummaries();
		}

		private void DownloadFileAndGetSummaries()
		{
			bootstrapReader = LoadDocXmlReader("Havit.Blazor.Components.Web.Bootstrap.xml");
			webReader = LoadDocXmlReader("Havit.Blazor.Components.Web.xml");
			DocXmlReader reader = LoadDocXmlReaderBasedOnNamespace(Type.Namespace);

			classMember = GetClassMember(reader);
			var properties = GetProperties(reader);
			this.properties = properties.properties;
			parameters = properties.parameters;
			parameters.OrderByDescending(p => p.EditorRequired);

			staticProperties = properties.staticProperties;
			events = properties.events;

			var methods = GetMethods(reader);
			this.methods = methods.methods;
			staticMethods = methods.staticMethods;

			HandleEnum(reader);
			HandleDelegate();

			StateHasChanged();
		}

		private DocXmlReader LoadDocXmlReader(string resourceName)
		{
			var assembly = Assembly.GetExecutingAssembly();

			resourceName = assembly.GetManifestResourceNames()
				.Single(str => str.EndsWith(resourceName));

			using (Stream stream = assembly.GetManifestResourceStream(resourceName))
			using (StreamReader reader = new StreamReader(stream))
			{
				TextReader textReader = new StringReader(reader.ReadToEnd());
				XPathDocument xPathDocument = new(textReader);

				return new(xPathDocument);
			}
		}

		private DocXmlReader LoadDocXmlReaderBasedOnNamespace(string typeNamespace)
		{
			if (string.IsNullOrEmpty(typeNamespace))
			{
				return bootstrapReader;
			}
			else if (typeNamespace.Contains("Havit.Blazor.GoogleTagManager"))
			{
				return LoadDocXmlReader("Havit.Blazor.GoogleTagManager.xml");
			}
			else if (typeNamespace.Contains("Bootstrap"))
			{
				return bootstrapReader;
			}
			else if (typeNamespace == "Havit.Blazor.Components.Web")
			{
				return webReader;
			}
			else
			{
				return bootstrapReader;
			}
		}

		private void HandleDelegate()
		{
			if (!Delegate)
			{
				return;
			}

			MethodInfo method = Type.GetMethod("Invoke");
			delegateSignature = $"{FormatType(method.ReturnType.ToString())} {FormatType(Type, asLink: false)}(";
			foreach (ParameterInfo param in method.GetParameters())
			{
				delegateSignature += $"{FormatType(param.ParameterType)} {param.Name}";
			}
			delegateSignature += ")";
		}

		private void HandleEnum(DocXmlReader reader)
		{
			isEnum = Type.IsEnum;
			if (!isEnum)
			{
				return;
			}

			string[] names = Type.GetEnumNames();
			EnumComments enumComments = reader.GetEnumComments(Type);
			for (int i = 0; i < names.Length; i++)
			{
				EnumMember enumMember = new();
				enumMember.Name = names[i];
				try { enumMember.Index = (int)Enum.Parse(Type, enumMember.Name); } catch { }
				try
				{
					var enumValueComment = enumComments.ValueComments.Where(o => o.Name == enumMember.Name).ToList().FirstOrDefault();
					if (enumValueComment is not null)
					{
						enumMember.Summary = enumValueComment.Summary;
					}
				}
				catch { }
				enumMembers.Add(enumMember);
			}
		}

		private ClassMember GetClassMember(DocXmlReader reader)
		{
			return new() { Comments = reader.GetTypeComments(Type) };
		}

		private (List<Property> properties, List<Property> parameters, List<Property> staticProperties, List<Property> events) GetProperties(DocXmlReader reader)
		{
			List<Property> typeProperties = new();
			List<Property> parameters = new();
			List<Property> staticProperties = new();
			List<Property> events = new();

			foreach (var property in Type.GetProperties(bindingFlags))
			{
				Property newProperty = new();
				newProperty.PropertyInfo = property;

				if (DetermineWhetherPropertyShouldBeAdded(newProperty) == false)
				{
					continue;
				}

				newProperty.Comments = reader.GetMemberComments(property);
				if (string.IsNullOrWhiteSpace(newProperty.Comments.Summary))
				{
					string summary = string.Empty;
					inputBaseSummaries.TryGetValue(newProperty.PropertyInfo.Name, out summary);
					if (summary is not null)
					{
						newProperty.Comments.Summary = summary;
					}

					if (string.IsNullOrWhiteSpace(newProperty.Comments.Summary))
					{
						newProperty.Comments = webReader.GetMemberComments(property);
						if (string.IsNullOrWhiteSpace(newProperty.Comments.Summary))
						{
							newProperty.Comments = bootstrapReader.GetMemberComments(property);
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
				else if (IsPropertyStatic(newProperty))
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

		private (List<Method> methods, List<Method> staticMethods) GetMethods(DocXmlReader reader)
		{
			List<Method> typeMethods = new();
			List<Method> staticMethods = new();

			foreach (var method in Type.GetMethods(bindingFlags))
			{
				Method newMethod = new();
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

		private bool DetermineWhetherPropertyShouldBeAdded(Property property)
		{
			string name = property.PropertyInfo.Name;
			if (byDefaultExcludedProperties.Contains(name) || ExcludedMembers.Contains(name))
			{
				return false;
			}

			return true;
		}

		private bool DetermineWhetherMethodShouldBeAdded(Method method)
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
			if (name.StartsWith("set") || name.StartsWith("get") || objectDerivedMethods.Contains(name) || derivedMethods.Contains(name) || ExcludedMembers.Contains(name))
			{
				return false;
			}

			return true;
		}

		private bool HasParameterAttribute(Property property, out bool editorRequired)
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

		private bool IsPropertyStatic(Property property)
		{
			return property.PropertyInfo.GetAccessors(false).Any(o => o.IsStatic);
		}

		private bool IsEvent(Property property)
		{
			return property.PropertyInfo.PropertyType == typeof(EventCallback<>) || property.PropertyInfo.PropertyType == typeof(EventCallback);
		}

		private CommonComments FindInheritDoc(Property property, DocXmlReader reader)
		{
			Type[] interfaces = Type.GetInterfaces();

			foreach (var currentInterface in interfaces)
			{
				var matchingMember = currentInterface.GetMembers().Where(o => o.Name == property.PropertyInfo.Name).FirstOrDefault();
				return reader.GetMemberComments(matchingMember);
			}

			return null;
		}

		private static string ReplaceTypeNames(string type)
		{
			foreach (var typeName in typeNames)
			{
				type = type.Replace(typeName.type, typeName.name);
			}

			return type;
		}

		private string FormatMethod(Type type)
		{
			string formattedName = FormatType(type);

			if (!formattedName.Contains("IAsyncResult"))
			{
				return formattedName;
			}

			string typeName = null;
			if (!string.IsNullOrEmpty(delegateSignature))
			{
				string[] charSplit = delegateSignature.Split(new[] { '<', '>', '/', '\"' });
				string[] slashSplit = delegateSignature.Split("&lt;");

				var charSplitResult = charSplit.ToList().FirstOrDefault(s => s.Contains("Result"));
				if (charSplitResult.Contains("&lt;"))
				{
					typeName = slashSplit.ToList().FirstOrDefault(s => s.Contains("Result"));
				}
				else
				{
					typeName = charSplitResult;
				}
			}
			typeName ??= $"{Type.Name.Replace("Provider", "").Replace("Delegate", "")}Result";

			return $"IAsyncResult<<a href=\"/types/{typeName}\">{typeName}</a>>";
		}

		public static string FormatType(Type type, bool asLink = true)
		{
			string typeName = type.FullName;
			if (string.IsNullOrWhiteSpace(typeName))
			{
				typeName = type.AssemblyQualifiedName;
				if (string.IsNullOrWhiteSpace(typeName))
				{
					typeName = type.Name;
					if (string.IsNullOrWhiteSpace(typeName))
					{
						return string.Empty;
					}
				}
			}

			typeName = Regex.Replace(typeName, @"[a-zA-Z]*\.", ""); // Remove namespaces

#pragma warning disable CA1416 // Validate platform compatibility
			var provider = CodeDomProvider.CreateProvider("CSharp");
			var reference = new CodeTypeReference(typeName);

			typeName = ReplaceTypeNames(provider.GetTypeOutput(reference));
#pragma warning restore CA1416 // Validate platform compatibility
			typeName = Regex.Replace(typeName, "Nullable<[a-zA-Z]+>", capture => $"{capture.Value[9..^1]}?");

			typeName = ConstructGenericTypeName(type, typeName) ?? typeName;

			string internalTypeName = null;
			if (asLink)
			{
				internalTypeName = GenerateLinkForInternalType(typeName);
			}
			if (internalTypeName is not null)
			{
				typeName = internalTypeName;
			}
			else
			{
				typeName = HttpUtility.HtmlEncode(typeName);
			}

			return typeName;
		}

		public static string FormatType(string typeName)
		{
			if (string.IsNullOrWhiteSpace(typeName))
			{
				return string.Empty;
			}

			typeName = Regex.Replace(typeName, @"[a-zA-Z]*\.", ""); // Remove namespaces

#pragma warning disable CA1416 // Validate platform compatibility
			var provider = CodeDomProvider.CreateProvider("CSharp");
			var reference = new CodeTypeReference(typeName);

			typeName = ReplaceTypeNames(provider.GetTypeOutput(reference));
#pragma warning restore CA1416 // Validate platform compatibility
			typeName = Regex.Replace(typeName, "Nullable<[a-zA-Z]+>", capture => $"{capture.Value[9..^1]}?");

			string internalTypeName = GenerateLinkForInternalType(typeName);
			if (internalTypeName is not null)
			{
				typeName = internalTypeName;
			}
			else
			{
				typeName = HttpUtility.HtmlEncode(typeName);
			}

			return typeName;
		}

		private static string ConstructGenericTypeName(Type type, string typeName)
		{
			if (!type.IsGenericType || !typeName.ToLower().Contains("object"))
			{
				return null;
			}

			var genericArguments = type.GetGenericTypeDefinition()
				.GetGenericArguments();
			var genericArgumentNamesFromFullTypeName = GetGenericParameterNamesFromFullTypeName(typeName);
			List<string> genericParameterNames = new();

			if (genericArgumentNamesFromFullTypeName.Count != genericArguments.Length)
			{
				return null;
			}
			for (int i = 0; i < genericArguments.Length; i++)
			{
				if (genericArgumentNamesFromFullTypeName[i].Contains("object"))
				{
					genericParameterNames.Add(FormatGenericParameterName(genericArguments[i].Name));
				}
				else
				{
					genericParameterNames.Add(genericArgumentNamesFromFullTypeName[i].Trim());
				}
			}

			if (genericParameterNames is null || genericParameterNames.Count == 0)
			{
				return null;
			}

			// Constructing the generic type name from a list of generic type name parameters.
			typeName = typeName.Split('<').FirstOrDefault() + "<";
			genericParameterNames.ForEach(n => typeName += $"{n}, ");

			typeName = typeName.Remove(typeName.Length - 2, 2);
			typeName += ">";

			return typeName;
		}

		private static List<string> GetGenericParameterNamesFromFullTypeName(string typeName)
		{
			string allGenericParameterNames = typeName.Split('<').LastOrDefault().Replace(">", "");
			return allGenericParameterNames.Split(',').ToList();
		}

		private static string FormatGenericParameterName(string genericParameterName)
		{
			if (genericParameterName == "T") // When a generic type parameter is named "TItem", then when retrieved with reflection, it's name is only "T".
			{
				return "TItem";
			}
			else if (genericParameterName == "TResult")
			{
				return "TValue";
			}
			else
			{
				return genericParameterName;
			}
		}

		public static string GenerateLinkForInternalType(string typeName, bool checkForInternal = true, string linkText = null)
		{
			string typeNameForOwnDocumentation = typeName.Replace("?", "");
			bool generic = false;
			if (Regex.Matches(typeNameForOwnDocumentation, "<").Count == 1)
			{
				generic = true;

				typeNameForOwnDocumentation = Regex.Replace(typeNameForOwnDocumentation, "^[^<]+", "");
				typeNameForOwnDocumentation = Regex.Replace(typeNameForOwnDocumentation, "<[a-zA-Z]+>", capture => $"{capture.Value[1..^1]}");
			}

			if (!checkForInternal)
			{
				return GenerateLinkTagForInternalType(typeName, typeNameForOwnDocumentation, linkText, generic);
			}

			if (InternalTypeDoc.DetermineIfTypeIsInternal(typeNameForOwnDocumentation))
			{
				return GenerateLinkTagForInternalType(typeName, typeNameForOwnDocumentation, linkText, generic);
			}

			return null;
		}

		private static string GenerateLinkTagForInternalType(string typeName, string typeNameForOwnDocumentation, string linkText, bool generic)
		{
			if (linkText is null)
			{
				linkText = typeName;

				if (generic)
				{
					linkText = typeNameForOwnDocumentation;
				}
			}

			string[] aroundLinkTexts = typeName.Split(typeNameForOwnDocumentation);
			if (generic && aroundLinkTexts.Length == 2)
			{
				return $"{aroundLinkTexts[0]}<a href=\"/types/{HttpUtility.UrlEncode(typeNameForOwnDocumentation)}\">{HttpUtility.HtmlEncode(linkText)}</a>{aroundLinkTexts[^1]}</code>";
			}

			return $"<a href=\"/types/{HttpUtility.UrlEncode(typeNameForOwnDocumentation)}\">{HttpUtility.HtmlEncode(linkText)}</a></code>";
		}

		public static string RemoveSpecialCharacters(string text)
		{
			Regex regex = new("[^a-zA-Z]");
			return regex.Replace(text, "");
		}
	}
}
