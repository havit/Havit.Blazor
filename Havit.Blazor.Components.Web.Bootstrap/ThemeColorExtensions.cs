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

	public static string ToButtonColorCss(this ThemeColor themeColor, bool outline = false)
	{
		// TODO v6 (#1442): Bootstrap 6 replaces per-color button classes (btn-primary, btn-outline-primary)
		// with a composition of a variant class (btn-solid, btn-outline, btn-subtle, btn-text) and a theme-* class.
		return (themeColor, outline) switch
		{
			(ThemeColor.Primary, false) => "btn-primary",
			(ThemeColor.Primary, true) => "btn-outline-primary",
			(ThemeColor.Secondary, false) => "btn-secondary",
			(ThemeColor.Secondary, true) => "btn-outline-secondary",
			(ThemeColor.Success, false) => "btn-success",
			(ThemeColor.Success, true) => "btn-outline-success",
			(ThemeColor.Danger, false) => "btn-danger",
			(ThemeColor.Danger, true) => "btn-outline-danger",
			(ThemeColor.Warning, false) => "btn-warning",
			(ThemeColor.Warning, true) => "btn-outline-warning",
			(ThemeColor.Info, false) => "btn-info",
			(ThemeColor.Info, true) => "btn-outline-info",
			(ThemeColor.Accent, false) => "btn-accent",
			(ThemeColor.Accent, true) => "btn-outline-accent",
			(ThemeColor.Inverse, false) => "btn-inverse",
			(ThemeColor.Inverse, true) => "btn-outline-inverse",
			(ThemeColor.Link, _) => "btn-link",
			(ThemeColor.None, _) => null,
			_ => throw new InvalidOperationException($"Unknown color {themeColor:g}.")
		};
	}
}
