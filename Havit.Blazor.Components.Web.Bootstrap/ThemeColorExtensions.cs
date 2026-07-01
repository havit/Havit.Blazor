namespace Havit.Blazor.Components.Web.Bootstrap;

public static class ThemeColorExtensions
{
	/// <summary>
	/// Returns the Bootstrap 6 composable theme class (<c>theme-*</c>) which applies the semantic
	/// theme tokens (<c>--theme-bg</c>, <c>--theme-fg</c>, <c>--theme-border</c>, ...) to a component.
	/// </summary>
	public static string ToThemeCss(this ThemeColor themeColor)
	{
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as a theme class."),
			_ => "theme-" + themeColor.ToString("f").ToLower()
		};
	}

	public static string ToBackgroundColorCss(this ThemeColor themeColor, bool subtle = false)
	{
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as background color."),
			// Bootstrap 6: the subtle variant moved from suffix (bg-primary-subtle) to infix (bg-subtle-primary).
			_ => (subtle ? "bg-subtle-" : "bg-") + themeColor.ToString("f").ToLower()
		};
	}

	public static string ToTextBackgroundColorCss(this ThemeColor themeColor)
	{
		// Bootstrap 6 removed the text-bg-* utilities; the theme-* class sets both background and contrast foreground.
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as text-bg color."),
			_ => themeColor.ToThemeCss()
		};
	}

	public static string ToTextColorCss(this ThemeColor themeColor, bool emphasis = false)
	{
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as text color."),
			// Bootstrap 6: text color utilities were renamed from text-* to fg-* (emphasis: fg-emphasis-*).
			_ => (emphasis ? "fg-emphasis-" : "fg-") + themeColor.ToString("f").ToLower()
		};
	}

	public static string ToBorderColorCss(this ThemeColor themeColor, bool subtle = false)
	{
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as border color."),
			// Bootstrap 6: the subtle variant moved from suffix (border-primary-subtle) to infix (border-subtle-primary).
			_ => (subtle ? "border-subtle-" : "border-") + themeColor.ToString("f").ToLower()
		};
	}

}
