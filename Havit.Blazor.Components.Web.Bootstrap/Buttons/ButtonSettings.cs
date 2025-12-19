namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxButton"/> and derived components.
/// </summary>
public record ButtonSettings
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.3/components/buttons/#sizes">Bootstrap button size</see>.
	/// </summary>
	public ButtonSize? Size { get; set; }

	/// <summary>
	/// CSS class to be rendered with the button.
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// CSS class to be rendered with the button icon.
	/// </summary>
	public string IconCssClass { get; set; }

	/// <summary>
	/// Icon to be rendered with the button.
	/// </summary>
	public IconBase Icon { get; set; }

	/// <summary>
	/// Position of the icon within the button.
	/// </summary>
	public ButtonIconPlacement? IconPlacement { get; set; }

	/// <summary>
	/// Bootstrap button color (style).
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap outline button style</see>.
	/// </summary>
	public bool? Outline { get; set; } = false;

	/// <summary>
	/// Tooltip settings.
	/// </summary>
	public TooltipSettings TooltipSettings { get; set; }
}
