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

	public static string ToTextBackgroundColorCss(this ThemeColor themeColor)
	{
		return themeColor switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as text-bg color."),
			_ => "text-bg-" + themeColor.ToString("f").ToLower()
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

	public static string ToButtonColorCss(this ThemeColor themeColor, bool outline = false)
	{
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
			(ThemeColor.Light, false) => "btn-light",
			(ThemeColor.Light, true) => "btn-outline-light",
			(ThemeColor.Dark, false) => "btn-dark",
			(ThemeColor.Dark, true) => "btn-outline-dark",
			(ThemeColor.Link, _) => "btn-link",
			(ThemeColor.None, _) => null,
			_ => throw new InvalidOperationException($"Unknown color {themeColor:g}.")
		};
	}
}
