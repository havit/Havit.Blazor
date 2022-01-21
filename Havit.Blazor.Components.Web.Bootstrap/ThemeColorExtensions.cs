namespace Havit.Blazor.Components.Web.Bootstrap
{
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

		public static string ToOutlineButtonColorCss(this ThemeColor themeColor)
		{
			return themeColor switch
			{
				ThemeColor.Primary => "btn-outline-primary",
				ThemeColor.Secondary => "btn-outline-secondary",
				ThemeColor.Success => "btn-outline-success",
				ThemeColor.Danger => "btn-outline-danger",
				ThemeColor.Warning => "btn-outline-warning",
				ThemeColor.Info => "btn-outline-info",
				ThemeColor.Light => "btn-outline-light",
				ThemeColor.Dark => "btn-outline-dark",
				ThemeColor.Link => "btn-link",
				ThemeColor.None => null,
				_ => throw new InvalidOperationException($"Unknown {nameof(HxButton)} color {themeColor:g}.")
			};
		}
	}
}
