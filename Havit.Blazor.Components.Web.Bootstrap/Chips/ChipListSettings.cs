namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxChipList"/> component.
/// </summary>
public class ChipListSettings
{
	/// <summary>
	/// The theme color of the chips (applied as a Bootstrap <c>theme-*</c> class).
	/// </summary>
	public ThemeColor? Color { get; set; }

	/// <summary>
	/// Settings for the <see cref="HxBadge"/> previously used to render chips.
	/// </summary>
	[Obsolete("Chips are no longer rendered as badges since Bootstrap 6 (the native Chip component is used). Use the Color property (mapped to a theme-* class) instead. Only the Color and CssClass values of these settings are honored.")]
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
