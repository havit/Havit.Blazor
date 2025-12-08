namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxCheckbox"/>.
/// </summary>
public record CheckboxSettings : InputSettings
{
	/// <summary>
	/// Bootstrap button color (style).
	/// For <see cref="CheckboxRenderMode.ToggleButton"/>.
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap outline button style</see>.
	/// For <see cref="CheckboxRenderMode.ToggleButton"/>.
	/// </summary>
	public bool? Outline { get; set; }

	/// <summary>
	/// Checkbox render mode.
	/// </summary>
	public CheckboxRenderMode? RenderMode { get; set; }
}
