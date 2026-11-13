namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxRadioButtonListBase{TValue, TItem}"/>.
/// </summary>
public record RadioButtonListSettings : InputSettings
{
	/// <summary>
	/// Color for <see cref="RadioButtonListRenderMode.ToggleButtons"/> and <see cref="RadioButtonListRenderMode.ButtonGroup"/>.
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// Indicates whether to use <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap "outline" buttons</see>.
	/// for <see cref="RadioButtonListRenderMode.ToggleButtons"/> and <see cref="RadioButtonListRenderMode.ButtonGroup"/>.
	/// </summary>
	public ButtonVariant? Variant { get; set; }

	/// <summary>
	/// Size of buttons for <see cref="RadioButtonListRenderMode.ToggleButtons"/> and <see cref="RadioButtonListRenderMode.ButtonGroup"/>.
	/// </summary>
	public ButtonSize? ButtonSize { get; set; }

	/// <summary>
	/// Size of the radio buttons (not used for the toggle button render modes, use <see cref="ButtonSize"/>) (<c>radio-sm</c>/<c>radio-lg</c>).
	/// </summary>
	public InputSize? InputSize { get; set; }
}
