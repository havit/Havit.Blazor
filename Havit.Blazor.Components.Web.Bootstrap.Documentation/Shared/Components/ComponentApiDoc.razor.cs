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

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
	public partial class ComponentApiDoc
	{
		[Parameter] public Type Type { get; set; }

		[Inject] private NavigationManager NavigationManager { get; set; }

		private static readonly HttpClient client = new HttpClient();

		private List<Property> properties = new();
		private List<Property> events = new();
		private List<Method> methods = new();

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
			properties = GetProperties(reader);
			events = SeparateEvents();
			methods = GetMethods(reader);

			StateHasChanged();
		}

		private List<Property> SeparateEvents()
		{
			List<Property> events = new();

			foreach (var property in properties)
			{
				if (property.PropertyInfo.PropertyType == typeof(EventCallback<>) || property.PropertyInfo.PropertyType == typeof(EventCallback))
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

		private List<Property> GetProperties(DocXmlReader reader)
		{
			List<Property> typeProperties = new();

			foreach (var property in Type.GetProperties())
			{
				Property newProperty = new();
				newProperty.PropertyInfo = property;
				newProperty.Comments = reader.GetMemberComments(property);

				typeProperties.Add(newProperty);
			}

			return typeProperties;
		}

		private List<Method> GetMethods(DocXmlReader reader)
		{
			List<Method> typeMethods = new();

			foreach (var method in Type.GetMethods())
			{
				Method newMethod = new();
				newMethod.MethodInfo = method;
				newMethod.Comments = reader.GetMethodComments(method);

				if (newMethod.MethodInfo.Name.Contains("set") == false && newMethod.MethodInfo.Name.Contains("get") == false)
				{
					typeMethods.Add(newMethod);
				}
			}

			return typeMethods;
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

						debug += $"{match}";

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

				bool isType = false;
				if (splitLink[0][0] == 'T')
				{
					isType = true;
				}

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
				throw new NotImplementedException();
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
					inputComments.Summary = FormatComment(inputComments.Summary);
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
		}
	}
}
