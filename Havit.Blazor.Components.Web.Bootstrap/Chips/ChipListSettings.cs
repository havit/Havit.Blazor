namespace Havit.Blazor.Components.Web.Bootstrap;

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
	/// Enables/disables the reset button.
	/// </summary>
	public bool? ShowResetButton { get; set; }
}
