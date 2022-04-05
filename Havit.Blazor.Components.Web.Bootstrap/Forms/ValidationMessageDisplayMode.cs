namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Validation message display mode.
/// </summary>
public enum ValidationMessageDisplayMode
{
	/// <summary>
	/// Validation message is displayed in the regular way.
	/// </summary>
	Regular,

	/// <summary>
	/// Validation message is displayed as a tooltip.
	/// </summary>
	Tooltip,

	/// <summary>
	/// Validation message with space reservation.
	/// </summary>
	KeepSpace
}