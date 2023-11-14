using System.Text.RegularExpressions;
using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Model;

public abstract class MemberModel
{
	private Type enclosingType;

	private bool generic;
	private string genericTypeSuffix;

	protected string TryFormatComment(string comment, Type enclosingType = null)
	{
		this.enclosingType = enclosingType;

		try
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

			// <see cref=""/> + other <see> variantions
			{
				Regex regex = new("<see");
				comment = regex.Replace(comment, "<a");

				regex = new("<a cref=");
				comment = regex.Replace(comment, "<code><a cref=");

				regex = new("</see>");
				comment = regex.Replace(comment, "</a>");

				regex = new("cref=\"([A-Za-z\\.:`\\d])+");
				var matches = regex.Matches(comment);

				foreach (var match in matches)
				{
					string link = match.ToString().Split('\"').LastOrDefault(); // get the part in the quotes (value of the cref attribute)
					link = PrepareLinkForFullLinkGeneration(link);
					string[] splitLink = link.Split('.');

					regex = new("cref=\"([A-Za-z\\.:`\\d])+\" ?/>"); // find this part of the element (beggining already replaced): cref="P:System.Text.Regex.Property" />
					comment = regex.Replace(comment, GenerateFullLink(splitLink, link), 1); // replace the above with a generated link to the documentation
				}
			}
		}
		catch
		{
			// NOOP
		}

		return comment;
	}

	private string PrepareLinkForFullLinkGeneration(string link)
	{
		generic = IsGeneric(link); // this information is used later to generate a Microsoft documentation link

		Regex regex = new("`\\d");
		var match = regex.Match(link);
		genericTypeSuffix = match.Value;

		return regex.Replace(link, "");
	}

	private string GenerateFullLink(string[] splitLink, string fullLink)
	{
		if (splitLink is null || splitLink.Length == 0)
		{
			return string.Empty;
		}

		string supportClassesResultLink = HandleSupportClasses(splitLink, fullLink);
		if (supportClassesResultLink is not null)
		{
			return supportClassesResultLink;
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

	private string HandleSupportClasses(string[] splitLink, string fullLink)
	{
		if (IsEnum(splitLink))
		{
			string internalTypeLink = ApiRenderer.GenerateLinkForInternalType(splitLink[^2], false, $"{splitLink[^2]}.{splitLink[^1]}");
			if (internalTypeLink is not null)
			{
				return internalTypeLink;
			}
		}

		if (splitLink[^1] == "Defaults")
		{
			string internalTypeLink = ApiRenderer.GenerateLinkForInternalType($"{splitLink[^2].Remove(0, 2)}{splitLink[^1]}", false, $"{splitLink[^2]}.{splitLink[^1]}"); // We have to generate a type name suitable for the support type page.
			if (internalTypeLink is not null)
			{
				return internalTypeLink;
			}
		}

		return null;
	}

	private string GenerateHavitDocumentationLink(string[] splitLink)
	{
		bool containsHx = splitLink.Any(t => t.Contains("hx", StringComparison.OrdinalIgnoreCase));
		string seeName = "";

		string fullLink = "";

		bool isType = IsType(splitLink);
		bool isProperty = IsProperty(splitLink);
		bool isComponent = true;

		if (isType)
		{
			for (int i = splitLink.Length - 1; i >= 0; i--) // loop through the array from the back, prepend the current part, and stop once the component name is reached
			{
				fullLink = $"{splitLink[i]}";

				if (splitLink[i].Contains("Hx"))
				{
					containsHx = true;
					seeName = splitLink[i];
					break;
				}
			}

			string className = GetFullGenericTypeName(splitLink[^1]);
			isComponent = ApiTypeHelper.GetType(className)?.IsSubclassOf(typeof(ComponentBase)) ?? false;
		}
		else if (isProperty)
		{
			if (splitLink.Length >= 2)
			{
				fullLink = $"{splitLink[^2]}#{splitLink[^1]}";
				if (enclosingType is not null && PrepareLinkForFullLinkGeneration(enclosingType.Name) == splitLink[^2])
				{
					seeName = $"{splitLink[^1]}";
				}
				else
				{
					seeName = $"{splitLink[^2]}.{splitLink[^1]}";
				}
			}

			isComponent = ApiTypeHelper.GetType(splitLink[^2])?.IsSubclassOf(typeof(ComponentBase)) ?? false;
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
		if ((fullLink.Length == 0) || ((containsHx == false) && isType))
		{
			fullLink = splitLink[^1];
			seeName = splitLink[^1];
		}

		if ((enclosingType is null || enclosingType.FullName.Contains("Hx") || !isProperty) && isComponent)
		{
			fullLink = $"href=\"/components/{fullLink}";
		}
		else
		{
			fullLink = $"href=\"/types/{fullLink}";
		}
		fullLink += $"\">{seeName}</a></code>";
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

		string genericTypeLinkSuffix = generic ? "-1" : "";

		fullLink = $"href=\"https://docs.microsoft.com/en-us/dotnet/api/{fullLink}{genericTypeLinkSuffix}";
		fullLink += $"\">{seeName}</a></code>";
		return fullLink;
	}

	private bool IsProperty(string[] splitLink)
	{
		return splitLink[0][0] == 'P';
	}
	private bool IsType(string[] splitLink)
	{
		return splitLink[0][0] == 'T';
	}
	private bool IsEnum(string[] splitLink)
	{
		return splitLink[0][0] == 'F';
	}

	private bool IsGeneric(string link)
	{
		return link.Contains('`');
	}

	/// <returns>The type name including the <c>`1</c> or <c>`2</c> suffix if the type is generic.</returns>
	private string GetFullGenericTypeName(string typeName)
	{
		if (!generic)
		{
			return typeName;
		}

		return $"{typeName}{genericTypeSuffix}";
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
