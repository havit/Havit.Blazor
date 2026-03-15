namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxCheckboxList{TValue, TItem}"/>.
/// </summary>
public record CheckboxListSettings : InputSettings
{
	/// <summary>
	/// Color for <see cref="CheckboxListRenderMode.ToggleButtons"/>.
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// Indicates whether to use <see href="https://getbootstrap.com/docs/5.3/components/buttons/#outline-buttons">Bootstrap "outline" buttons</see>.
	/// for <see cref="CheckboxListRenderMode.ToggleButtons"/> and <see cref="CheckboxListRenderMode.ButtonGroup"/>.
	/// </summary>
	public bool? Outline { get; set; }
}
