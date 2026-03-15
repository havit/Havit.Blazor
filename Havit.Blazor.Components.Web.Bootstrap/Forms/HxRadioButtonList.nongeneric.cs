namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Non-generic API for the <see cref="HxRadioButtonList{TValue, TItem}"/> component.
/// </summary>
public sealed class HxRadioButtonList
{
	/// <summary>
	/// Application-wide defaults for <see cref="HxRadioButtonList{TValue, TItem}"/> and derived components.
	/// </summary>
	public static RadioButtonListSettings Defaults { get; set; }

	static HxRadioButtonList()
	{
		Defaults = new RadioButtonListSettings()
		{
			// ValidationMessageMode = null, HxInputBase sets the default
			Color = ThemeColor.None, // we do not have HxRadioButton to provide a default here
			Outline = false, // we do not have HxRadioButton to provide a default here
		};
	}
}
