namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxChipList"/> component.
/// </summary>
public class ChipListSettings
{
	/// <summary>
	/// Settings for the <see cref="HxBadge"/> used to render chips.
	/// </summary>
	public BadgeSettings ChipBadgeSettings { get; set; }

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	public string CssClass { get; set; }

	/// <summary>
	/// Enables or disables the reset button.
	/// </summary>
	public bool? ShowResetButton { get; set; }
}
