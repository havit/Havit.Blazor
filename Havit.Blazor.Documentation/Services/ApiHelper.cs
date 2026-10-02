namespace Havit.Blazor.Documentation.Services;

public static class ApiTypeHelper
{
	/// <summary>
	/// Minimum length of the type name to search for types containing the name (to prevent returning an arbitrary type for empty or very short inputs).
	/// </summary>
	private const int MinimumContainingTypeNameLength = 3;

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

		if (string.IsNullOrWhiteSpace(typeName))
		{
			return null;
		}

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

		if (includeTypesContainingTypeName && (typeName.Trim().Length >= MinimumContainingTypeNameLength))
		{
			try
			{
				string searchedName = typeName.Trim();

				// Match on the (non-generic) type name only, never on the namespace (FullName),
				// and prefer the closest match: equality, then prefix, then substring.
				var matchingTypes = typeof(HxButton).Assembly.GetTypes()
					.Where(t => t.IsVisible)
					.Select(t => new { Type = t, Name = GetNameWithoutGenericArity(t) })
					.Select(t => new { t.Type, t.Name, Rank = GetMatchRank(t.Name, searchedName) })
					.Where(t => t.Rank is not null)
					.OrderBy(t => t.Rank)
					.ThenBy(t => t.Name.Length)
					.ThenBy(t => t.Type.FullName, StringComparer.Ordinal)
					.ToList();

				if (matchingTypes.Count > 0)
				{
					int bestRank = matchingTypes[0].Rank.Value;
					var bestMatchingTypes = matchingTypes.Where(t => t.Rank == bestRank).Select(t => t.Type).ToList();

					if (preferGenericTypes)
					{
						result = bestMatchingTypes.FirstOrDefault(t => t.IsGenericType) ?? bestMatchingTypes[0];
					}
					else
					{
						result = bestMatchingTypes[0];
					}
					return result;
				}
			}
			catch { }
		}

		return null;
	}

	private static int? GetMatchRank(string candidateName, string searchedName)
	{
		if (candidateName.Equals(searchedName, StringComparison.OrdinalIgnoreCase))
		{
			return 0;
		}
		if (candidateName.StartsWith(searchedName, StringComparison.OrdinalIgnoreCase))
		{
			return 1;
		}
		if (candidateName.Contains(searchedName, StringComparison.OrdinalIgnoreCase))
		{
			return 2;
		}
		return null;
	}

	private static string GetNameWithoutGenericArity(Type type)
	{
		int backtickPosition = type.Name.IndexOf('`');
		return (backtickPosition > 0) ? type.Name.Substring(0, backtickPosition) : type.Name;
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
