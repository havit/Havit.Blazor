namespace Havit.Blazor.Components.Web.Bootstrap;

public static class ButtonVariantExtensions
{
	/// <summary>
	/// Bootstrap 6 buttons compose a variant class (<c>btn-solid</c>, <c>btn-outline</c>, ...) with a theme color class (<c>theme-*</c>).
	/// Without a color, the variant classes have no theme tokens to consume, so no variant class is emitted (a plain neutral <c>btn</c> look),
	/// with the exception of the standalone <c>btn-link</c> variant.
	/// </summary>
	public static string ToButtonVariantAndColorCssClass(this ButtonVariant variant, ThemeColor color)
	{
		if (color == ThemeColor.Link)
		{
			return ButtonVariant.Link.ToButtonVariantCssClass();
		}
		if (color == ThemeColor.None)
		{
			return (variant == ButtonVariant.Link) ? ButtonVariant.Link.ToButtonVariantCssClass() : null;
		}
		return CssClassHelper.Combine(
			variant.ToButtonVariantCssClass(),
			(variant == ButtonVariant.Link) ? null : color.ToThemeCss());
	}


	public static string ToButtonVariantCssClass(this ButtonVariant variant)
	{
		return variant switch
		{
			ButtonVariant.Solid => "btn-solid",
			ButtonVariant.Outline => "btn-outline",
			ButtonVariant.Subtle => "btn-subtle",
			ButtonVariant.Text => "btn-text",
			ButtonVariant.Link => "btn-link",
			_ => throw new InvalidOperationException($"Unknown {nameof(ButtonVariant)} value {variant}.")
		};
	}
}
