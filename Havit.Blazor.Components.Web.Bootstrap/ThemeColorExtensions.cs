namespace Havit.Blazor.Components.Web.Bootstrap;

public static class ThemeColorExtensions
{
	public static string ToBackgroundColorCss(this ThemeColor themeColor, bool subtle = false)
	{
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as background color."),
			_ => "bg-" + themeColor.ToString("f").ToLower() + (subtle ? "-subtle" : null)
		};
	}

	public static string ToTextColorCss(this ThemeColor themeColor, bool emphasis = false)
	{
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as text color."),
			_ => "text-" + themeColor.ToString("f").ToLower() + (emphasis ? "-emphasis" : null)
		};
	}
	public static string ToBorderColorCss(this ThemeColor themeColor, bool subtle = false)
	{
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as border color."),
			_ => "border-" + themeColor.ToString("f").ToLower() + (subtle ? "-subtle" : null)
		};
	}
}
