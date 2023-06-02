namespace Havit.Blazor.Components.Web.Bootstrap;

public static class ThemeColorExtensions
{
	public static string ToBackgroundColorCss(this ThemeColor themeColor)
	{
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as background color."),
			_ => "bg-" + themeColor.ToString("f").ToLower()
		};
	}

	public static string ToTextColorCss(this ThemeColor themeColor)
	{
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as text color."),
			_ => "text-" + themeColor.ToString("f").ToLower()
		};
	}
}
