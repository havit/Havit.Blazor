namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

public static class ApiHelper
{
	private static readonly Dictionary<string, Type> delegateTypes = new()
		{
			{ "AutosuggestDataProviderDelegate", typeof(AutosuggestDataProviderDelegate<>) },
			{ "GridDataProviderDelegate", typeof(GridDataProviderDelegate<>) },
			{ "InputTagsDataProviderDelegate", typeof(InputTagsDataProviderDelegate) },
			{ "CalendarDateCustomizationProviderDelegate", typeof(CalendarDateCustomizationProviderDelegate) }
		};

	public static bool DetermineIfTypeIsInternal(string typeText)
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

	public static Type GetType(string typeName)
	{
		Type result;

		// Formatting typeName.
		int? openingBracePosition = GetOpeningBracePosition(typeName);
		if (openingBracePosition.HasValue)
		{
			typeName = typeName.Remove(openingBracePosition.Value, typeName.Length - openingBracePosition.Value);
		}

		// Handling delegate types, all other types are found by the Type.GetType() method.
		delegateTypes.TryGetValue(typeName, out result);
		if (result is not null)
		{
			return result; //  (result, true);
		}

		try
		{
			result = Type.GetType($"Havit.Blazor.Components.Web.Bootstrap.{typeName}, Havit.Blazor.Components.Web.Bootstrap");
			if (result is not null)
			{
				return result; // (result, false);
			}
		}
		catch { }

		try
		{
			result = Type.GetType($"Havit.Blazor.Components.Web.{typeName}, Havit.Blazor.Components.Web");
			if (result is not null)
			{
				return result; // ;
			}
		}
		catch { }

		return null; // (null, false);
	}

	private static int? GetOpeningBracePosition(string text)
	{
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] == '<')
			{
				return i;
			}
		}

		return null;
	}

}
