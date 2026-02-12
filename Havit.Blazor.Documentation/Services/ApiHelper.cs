namespace Havit.Blazor.Documentation.Services;

public static class ApiTypeHelper
{
	private static readonly Dictionary<string, Type> s_delegateTypes = new()
	{
		["AutosuggestDataProviderDelegate"] = typeof(AutosuggestDataProviderDelegate<>),
		["GridDataProviderDelegate"] = typeof(GridDataProviderDelegate<>),
		["InputTagsDataProviderDelegate"] = typeof(InputTagsDataProviderDelegate),
		["CalendarDateCustomizationProviderDelegate"] = typeof(CalendarDateCustomizationProviderDelegate),
		["SearchBoxDataProviderDelegate"] = typeof(SearchBoxDataProviderDelegate<>),
	};

	public static bool IsLibraryType(string typeText)
	{
		try
		{
			Type type = GetType(typeText);
			if (type is null)
			{
				return false;
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	public static bool IsDelegate(Type type)
	{
		return typeof(Delegate).IsAssignableFrom(type);
	}

	public static Type GetType(string typeName, bool includeTypesContainingTypeName = false, bool preferGenericTypes = true)
	{
		Type result;

		// Formatting typeName.
		int openingBracePosition = typeName.IndexOf("<");
		if (openingBracePosition > 0)
		{
			typeName = typeName.Remove(openingBracePosition, typeName.Length - openingBracePosition);
		}

		// Handling delegate types, all other types are found by the Type.GetType() method.
		s_delegateTypes.TryGetValue(typeName, out result);
		if (result is not null)
		{
			return result;
		}

		// Try to find the type in each namespace
		List<Type> candidates = new List<Type>();

		// Check Havit.Blazor.Components.Web.Bootstrap
		TryAddTypeCandidate(candidates, $"Havit.Blazor.Components.Web.Bootstrap.{typeName}", "Havit.Blazor.Components.Web.Bootstrap");

		// Check Havit.Blazor.Components.Web
		TryAddTypeCandidate(candidates, $"Havit.Blazor.Components.Web.{typeName}", "Havit.Blazor.Components.Web");

		// Check Havit.Blazor.Components.Web.ECharts
		TryAddTypeCandidate(candidates, $"Havit.Blazor.Components.Web.ECharts.{typeName}", "Havit.Blazor.Components.Web.ECharts");

		// If we have candidates, prefer generic types if requested
		if (candidates.Count > 0)
		{
			if (preferGenericTypes)
			{
				// Prefer generic types (IsGenericType = true)
				result = candidates.FirstOrDefault(t => t.IsGenericType) ?? candidates[0];
			}
			else
			{
				result = candidates[0];
			}
			return result;
		}

		if (includeTypesContainingTypeName)
		{
			try
			{
				var containingTypes = typeof(HxButton).Assembly.GetTypes()
					.Where(t => t.FullName.Contains(typeName))
					.ToList();

				if (containingTypes.Count > 0)
				{
					if (preferGenericTypes)
					{
						result = containingTypes.FirstOrDefault(t => t.IsGenericType) ?? containingTypes[0];
					}
					else
					{
						result = containingTypes[0];
					}
					return result;
				}
			}
			catch { }
		}

		return null;
	}

	private static void TryAddTypeCandidate(List<Type> candidates, string fullTypeName, string assemblyName)
	{
		try
		{
			// Try non-generic type first
			Type type = Type.GetType($"{fullTypeName}, {assemblyName}");
			if (type is not null)
			{
				candidates.Add(type);
			}

			// Try generic type with 1 parameter (most common case)
			type = Type.GetType($"{fullTypeName}`1, {assemblyName}");
			if (type is not null)
			{
				candidates.Add(type);
			}

			// Try generic type with 2 parameters
			type = Type.GetType($"{fullTypeName}`2, {assemblyName}");
			if (type is not null)
			{
				candidates.Add(type);
			}

			// Try generic type with 3 parameters (rare, but possible)
			type = Type.GetType($"{fullTypeName}`3, {assemblyName}");
			if (type is not null)
			{
				candidates.Add(type);
			}
		}
		catch { }
	}
}
