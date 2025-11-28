namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxCheckboxList{TValue, TItem}"/>.
/// </summary>
public record CheckboxListSettings : InputSettings
{
	/// <summary>
	/// Bootstrap button style - theme color.<br />
	/// The default is taken from <see cref="HxButton.Defaults"/> (<see cref="ThemeColor.None"/> if not customized).
	/// For <see cref="CheckboxRenderMode.ToggleButton"/>.
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap "outline" button</see> style.
	/// For <see cref="CheckboxListRenderMode.ToggleButton"/> and <see cref="CheckboxListRenderMode.ButtonGroup"/>.
	/// </summary>
	public bool? Outline { get; set; }
}
