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

	public static Type GetType(string typeName, bool includeTypesContainingTypeName = false)
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

		try
		{
			result = Type.GetType($"Havit.Blazor.Components.Web.Bootstrap.{typeName}, Havit.Blazor.Components.Web.Bootstrap");
			if (result is not null)
			{
				return result;
			}
		}
		catch { }

		try
		{
			result = Type.GetType($"Havit.Blazor.Components.Web.{typeName}, Havit.Blazor.Components.Web");
			if (result is not null)
			{
				return result;
			}
		}
		catch { }

		try
		{
			result = Type.GetType($"Havit.Blazor.Components.Web.ECharts.{typeName}, Havit.Blazor.Components.Web.ECharts");
			if (result is not null)
			{
				return result;
			}
		}
		catch { }

		if (includeTypesContainingTypeName)
		{
			try
			{
				result = typeof(HxButton).Assembly.GetTypes().FirstOrDefault((t) => t.FullName.Contains(typeName));
				if (result is not null)
				{
					return result;
				}
			}
			catch { }
		}

		return null;
	}
}
