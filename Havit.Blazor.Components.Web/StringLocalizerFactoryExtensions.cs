using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web;

/// <summary>
/// Extension methods for IStringLocalizerFactory.
/// </summary>
public static class StringLocalizerFactoryExtensions
{
	/// <summary>
	/// Returns the localized value when resourceType is not <c>null</c> (value used as the resource name).
	/// Otherwise, returns the value (without attempting to get the localized value).
	/// </summary>
	public static string GetLocalizedValue(this IStringLocalizerFactory stringLocalizerFactory, string value, Type resourceType)
	{
		if ((resourceType != null) && !String.IsNullOrEmpty(value))
		{
			return stringLocalizerFactory.Create(resourceType).GetString(value).Value;
		}

		return value;
	}
}
