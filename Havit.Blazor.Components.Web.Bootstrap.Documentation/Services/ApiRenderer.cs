using System.CodeDom;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using System.Web;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Model;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Pages;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

public static class ApiRenderer
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
		//typeName = ReplaceTypeNames(typeName);
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

		// TODO Duplicate code, see FormatType above

		typeName = Regex.Replace(typeName, @"[a-zA-Z]*\.", ""); // Remove namespaces

#pragma warning disable CA1416 // Validate platform compatibility
		var provider = CodeDomProvider.CreateProvider("CSharp");
		var reference = new CodeTypeReference(typeName);

		typeName = ReplaceTypeNames(provider.GetTypeOutput(reference));
#pragma warning restore CA1416 // Validate platform compatibility
		//typeName = ReplaceTypeNames(typeName);
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

	public static string FormatMethod(Type type, ComponentApiDocModel model)
	{
		string formattedName = FormatType(type);

		if (!formattedName.Contains("IAsyncResult"))
		{
			return formattedName;
		}

		string typeName = null;
		if (!string.IsNullOrEmpty(model.DelegateSignature))
		{
			string[] charSplit = model.DelegateSignature.Split(new[] { '<', '>', '/', '\"' });
			string[] slashSplit = model.DelegateSignature.Split("&lt;");

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
		typeName ??= $"{type.Name.Replace("Provider", "").Replace("Delegate", "")}Result";

		return $"IAsyncResult<<a href=\"/types/{typeName}\">{typeName}</a>>";
	}

	public static string FormatGenericParameterName(string genericParameterName)
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

	public static string ReplaceTypeNames(string type)
	{
		foreach (var typeName in typeNames)
		{
			type = type.Replace(typeName.type, typeName.name);
		}

		return type;
	}


	public static string ConstructGenericTypeName(Type type, string typeName)
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
			genericParameterNames.Add(genericArgumentNamesFromFullTypeName[i].Trim());
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

	public static List<string> GetGenericParameterNamesFromFullTypeName(string typeName)
	{
		string allGenericParameterNames = typeName.Split('<').LastOrDefault().Replace(">", "");
		return allGenericParameterNames.Split(',').ToList();
	}

	public static string RemoveSpecialCharacters(string text)
	{
		Regex regex = new("[^a-zA-Z]");
		return regex.Replace(text, "");
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

}
