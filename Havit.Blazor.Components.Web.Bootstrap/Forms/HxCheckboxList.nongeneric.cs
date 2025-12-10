namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for the <see cref="HxCheckboxList{TValue, TItem}"/> component.
/// </summary>
public sealed class HxCheckboxList
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxCheckboxList{TValue, TItem}"/> and derived components.
	/// </summary>
	public static CheckboxListSettings Defaults { get; set; }

	static HxCheckboxList()
	{
		Defaults = new CheckboxListSettings()
		{
			// ValidationMessageMode = null, HxInputBase sets the default
			// Color = null, HxCheckbox.Color is not set, HxCheckbox uses its own default then
			// Outline = null, HxCheckbox.Outline is not set, HxCheckbox uses its own default then
		};
	}
}
