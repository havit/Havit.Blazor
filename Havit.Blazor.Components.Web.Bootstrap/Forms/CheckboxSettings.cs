namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxCheckbox"/>.
/// </summary>
public record CheckboxSettings : InputSettings
{
	/// <summary>
	/// Color for <see cref="CheckboxRenderMode.ToggleButton"/>.
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// Indicates whether to use <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap "outline" buttons</see>.
	/// for <see cref="CheckboxRenderMode.ToggleButton"/>.
	/// </summary>
	public bool? Outline { get; set; }

	/// <summary>
	/// Checkbox render mode.
	/// </summary>
	public CheckboxRenderMode? RenderMode { get; set; }
}
