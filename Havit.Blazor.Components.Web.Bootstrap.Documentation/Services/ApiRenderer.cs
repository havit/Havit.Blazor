﻿using System.CodeDom;
using System.CodeDom.Compiler;
using System.Text.RegularExpressions;
using System.Web;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Model;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Pages;
using Microsoft.CSharp;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

public static class ApiRenderer
{
	private static readonly (string type, string name)[] typeSimplifications =
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

	public static string FormatType(Type type, bool asLink = true)
	{
		string typeName = type.ToString();  // e.g. "System.Collections.Generic.List`1[System.String]"

		typeName = Regex.Replace(typeName, @"[a-zA-Z]*\.", ""); // Remove namespaces (TODO also removes parents of nested types)

		// simplify known types
		foreach (var typeSimplification in typeSimplifications)
		{
			typeName = typeName.Replace(typeSimplification.type, typeSimplification.name);
		}

		typeName = typeName.Replace("`1", String.Empty);
		typeName = typeName.Replace("`2", String.Empty);
		typeName = typeName.Replace("`3", String.Empty);
		typeName = typeName.Replace('[', '<');
		typeName = typeName.Replace(']', '>');
		typeName = typeName.Replace(",", ", ");

		if (typeName.StartsWith("Nullable<"))
		{
			typeName = typeName.Replace("Nullable<", String.Empty);
			typeName = typeName.Substring(0, typeName.Length - 1) + "?";
		}

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

	public static string FormatMethodReturnType(Type returnType, ComponentApiDocModel model)
	{
		string formattedName = FormatType(returnType);

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
		typeName ??= $"{returnType.Name.Replace("Provider", "").Replace("Delegate", "")}Result";

		return $"IAsyncResult<<a href=\"/types/{typeName}\">{typeName}</a>>";
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
		if (Regex.Matches(typeNameForOwnDocumentation, "<").Count == 1 && !typeNameForOwnDocumentation.Contains("Delegate")) // When a delegate is presented, we don't want to show only the generic data type's name contained in the signature but the whole name.
		{
			generic = true;

			typeNameForOwnDocumentation = Regex.Replace(typeNameForOwnDocumentation, "^[^<]+", "");
			typeNameForOwnDocumentation = Regex.Replace(typeNameForOwnDocumentation, "<[a-zA-Z]+>", capture => $"{capture.Value[1..^1]}");
		}

		if (!checkForInternal)
		{
			return GenerateLinkTagForInternalType(typeName, typeNameForOwnDocumentation, linkText, generic);
		}

		if (ApiHelper.DetermineIfTypeIsInternal(typeNameForOwnDocumentation))
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
