using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Xml.Linq;
using System.Xml.XPath;
using LoxSmoke.DocXml;
using System.Text.RegularExpressions;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
	public partial class ComponentApiDoc
	{
		[Parameter] public RenderFragment ChildContent { get; set; }

		/// <summary>
		/// A type to generate the documentation for
		/// </summary>
		[Parameter] public Type Type { get; set; }

		[Parameter] public bool DisplayBootstrapLink { get; set; } = true;
		[Parameter] public string CustomBootstrapLink { get; set; }

		/// <summary>
		/// If true, removes API header, and makes the type name header smaller
		/// </summary>
		[Parameter] public bool SubComponent { get; set; } = false;

		/// <summary>
		/// Names of members that will be excluded from the displayed documentation
		/// </summary>
		[Parameter] public List<string> ExcludedMembers { get; set; } = new();

		[Inject] private NavigationManager NavigationManager { get; set; }

		private static readonly HttpClient client = new HttpClient();

		private string classSummary = string.Empty;
		private List<Property> properties = new();
		private List<Property> events = new();
		private List<Method> methods = new();
		private List<Method> staticMethods = new();

		private static string debug;

		protected override void OnParametersSet()
		{
			DownloadFileAndGetSummaries();
		}

		private async void DownloadFileAndGetSummaries()
		{
			TextReader textReader = new StringReader(await GetFile(GetDownloadLink()));
			XPathDocument xPathDocument = new(textReader);

			DocXmlReader reader = new(xPathDocument);

			classSummary = GetClassSummary(reader);
			properties = GetProperties(reader);
			events = SeparateEvents();

			var methods = GetMethods(reader);
			this.methods = methods.methods;
			staticMethods = methods.staticMethods;

			StateHasChanged();
		}

		private List<Property> SeparateEvents()
		{
			List<Property> events = new();

			foreach (var property in properties)
			{
				if (property.PropertyInfo.PropertyType == typeof(EventCallback<>) || property.PropertyInfo.PropertyType == typeof(EventCallback) || property.PropertyInfo.PropertyType.ToString().ToLower().Contains("event"))
				{
					events.Add(property);
				}
			}

			foreach (var currentEvent in events)
			{
				properties.Remove(currentEvent);
			}

			return events;
		}

		private string GetClassSummary(DocXmlReader reader)
		{
			return reader.GetTypeComments(Type).Summary;
		}

		private List<Property> GetProperties(DocXmlReader reader)
		{
			List<Property> typeProperties = new();

			foreach (var property in Type.GetProperties())
			{
				Property newProperty = new();
				newProperty.PropertyInfo = property;

				if (DetermineWhetherPropertyShouldBeAdded(newProperty) == false)
				{
					continue;
				}

				newProperty.Comments = reader.GetMemberComments(property);

				if (string.IsNullOrEmpty(newProperty.Comments.Summary))
				{
					// newProperty.Comments = FindInheritDoc(newProperty, reader); TO-DO
				}

				typeProperties.Add(newProperty);
			}

			return typeProperties;
		}

		private (List<Method> methods, List<Method> staticMethods) GetMethods(DocXmlReader reader)
		{
			List<Method> typeMethods = new();
			List<Method> staticMethods = new();

			foreach (var method in Type.GetMethods())
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
			List<string> byDefaultExcludedProperties = new() { "Defaults", "JSRuntime" };
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
			List<string> objectDerivedMethods = new() { "ToString", "GetType", "Equals", "GetHashCode" };
			List<string> derivedMethods = new() { "Dispose", "DisposeAsync", "SetParametersAsync", "ChildContent" };
			if (name.Contains("set") || name.Contains("get") || objectDerivedMethods.Contains(name) || derivedMethods.Contains(name) || ExcludedMembers.Contains(name))
			{
				return false;
			}

			return true;
		}

		private CommonComments FindInheritDoc(Property property, DocXmlReader reader)
		{
			Type[] interfaces = Type.GetInterfaces();

			foreach (var currentInterface in interfaces)
			{
				var matchingMember = currentInterface.GetMembers().Where(o => o.Name == property.PropertyInfo.Name).FirstOrDefault();
				debug += matchingMember.Name + " " + reader.GetMemberComments(matchingMember).Summary;
				return reader.GetMemberComments(matchingMember);
			}

			return null;
		}

		private string GetDownloadLink()
		{
			if (string.IsNullOrEmpty(Type.Namespace) == true)
			{
				return $"{NavigationManager.BaseUri}Havit.Blazor.Components.Web.Bootstrap.xml";
			}

			if (Type.Namespace.Contains("Bootstrap"))
			{
				return $"{NavigationManager.BaseUri}Havit.Blazor.Components.Web.Bootstrap.xml";
			}

			return $"{NavigationManager.BaseUri}Havit.Blazor.Components.Web.xml";
		}

		private async Task<string> GetFile(string uri)
		{
			string response = await client.GetStringAsync(uri);
			return response;
		}

		private string GetBootstrapComponentName()
		{
			return Type.Name.Replace("Hx", "");
		}

		public class Member
		{
			protected string FormatComment(string comment)
			{
				if (string.IsNullOrEmpty(comment))
				{
					return string.Empty;
				}

				// <c>
				{
					Regex regex = new("<c>");
					comment = regex.Replace(comment, "<code>");
					regex = new("</c>");
					comment = regex.Replace(comment, "</code>");
				}

				// <see cref=""/>
				{
					Regex regex = new("<see");
					comment = regex.Replace(comment, "<a");

					regex = new("cref=\"([A-Za-z\\.:])+");
					var matches = regex.Matches(comment);

					foreach (var match in matches)
					{
						string link = match.ToString().Split('\"').LastOrDefault(); // get the part in the quotes (value of the cref attribute)
						string[] splitLink = link.Split('.');

						regex = new("cref=\"([A-Za-z\\.:`\\d])+\" ?/>"); // find this part of the element (beggining already replaced): cref="P:System.Text.Regex.Property" />
						comment = regex.Replace(comment, GenerateFullLink(splitLink), 1); // replace the above with a generated link to the documentation
					}
				}

				return comment;
			}

			private string GenerateFullLink(string[] splitLink)
			{
				if (splitLink is null || splitLink.Length == 0)
				{
					return string.Empty;
				}

				if (splitLink[0].Contains("Havit"))
				{
					return GenerateHavitDocumentationLink(splitLink);
				}

				if (splitLink[0].Contains("Microsoft") || splitLink[0].Contains("System"))
				{
					return GenerateMicrosoftDocumentationLink(splitLink);
				}

				throw new NotSupportedException();
			}

			private string GenerateHavitDocumentationLink(string[] splitLink)
			{
				bool containsHx = false;
				string seeName = "";

				string fullLink = "";

				bool isType = IsType(splitLink);

				if (isType)
				{
					for (int i = splitLink.Length - 1; i >= 0; i--) // loop through the array from the back, prepend the current part, and stop once the component name is reached
					{
						fullLink = $"{splitLink[i]}/{fullLink}";

						if (splitLink[i].Contains("Hx"))
						{
							containsHx = true;
							seeName = splitLink[i];
							break;
						}
					}
				}
				else
				{
					if (splitLink.Length >= 2)
					{
						fullLink = splitLink[^2];
						seeName = $"{splitLink[^2]}.{splitLink[^1]}";
					}
				}

				// Default case: if "Hx" wasn't found anywhere, it couldn't be detected what part to use in the link, so the default part will be chosen
				if (fullLink.Length == 0 || (containsHx == false && isType))
				{
					fullLink = splitLink[^1];
					seeName = splitLink[^1];
				}

				fullLink = $"href=\"/components/{fullLink}";
				fullLink += $"\">{seeName}</a>";
				return fullLink;
			}

			private string GenerateMicrosoftDocumentationLink(string[] splitLink)
			{
				bool isType = IsType(splitLink);

				string fullLink = "";
				string seeName = splitLink[^1];
				if (isType)
				{
					string link = ConcatenateStringArray(splitLink);
					fullLink = link.Split(':')[^1];
				}

				fullLink = $"href=\"https://docs.microsoft.com/en-us/dotnet/api/{fullLink}";
				fullLink += $"\">{seeName}</a>";
				return fullLink;
			}

			private bool IsType(string[] splitLink)
			{
				bool isType = false;
				if (splitLink[0][0] == 'T')
				{
					isType = true;
				}

				return isType;
			}

			private string ConcatenateStringArray(string[] stringArray)
			{
				StringBuilder sb = new StringBuilder();
				foreach (var text in stringArray)
				{
					sb.Append(text);
					sb.Append(".");
				}

				sb.Remove(sb.Length - 1, 1);

				return sb.ToString();
			}
		}

		public class Property : Member
		{
			public PropertyInfo PropertyInfo { get; set; }

			public CommonComments Comments
			{
				set
				{
					CommonComments inputComments = value;
					try { inputComments.Summary = FormatComment(inputComments.Summary); } catch { }
					comments = inputComments;
				}
				get
				{
					return comments;
				}
			}
			private CommonComments comments;
		}

		public class Method : Member
		{
			public MethodInfo MethodInfo { get; set; }

			public MethodComments Comments
			{
				set
				{
					MethodComments inputComments = value;
					inputComments.Summary = FormatComment(inputComments.Summary);
					comments = inputComments;
				}
				get
				{
					return comments;
				}
			}
			private MethodComments comments;

			public string GetParameters()
			{
				StringBuilder concatenatedParameters = new StringBuilder();
				var parameters = MethodInfo.GetParameters();

				if (parameters is null || parameters.Length == 0)
				{
					return "()";
				}

				concatenatedParameters.Append("(");
				foreach (var parameter in parameters)
				{
					concatenatedParameters.Append($"{FormatType(parameter.ParameterType)} {parameter.Name}, ");
				}
				concatenatedParameters.Remove(concatenatedParameters.Length - 2, 2);
				concatenatedParameters.Append(")");

				return concatenatedParameters.ToString();
			}
		}

		public static string FormatType(Type type)
		{
			string shortType = type.ToString().Split('.')[^1];
			shortType = RemoveSpecialCharacters(shortType);

			switch (shortType)
			{
				case "Int16":
					return "short";
				case "UInt16":
					return "ushort";
				case "Int32":
					return "int";
				case "UInt32":
					return "uint";
				case "Int64":
					return "long";
				case "UInt64":
					return "ulong";
				case "Boolean":
					return "bool";
				case "String":
				case "Char":
				case "Decimal":
				case "Double":
				case "Byte":
				case "Sbyte":
				case "Void":
					return shortType.ToLower();
			}

			return shortType;
		}

		public static string RemoveSpecialCharacters(string text)
		{
			Regex regex = new("[^a-zA-Z]");
			return regex.Replace(text, "");
		}
	}
}
