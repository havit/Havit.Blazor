namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Validation message display mode.
/// </summary>
public enum ValidationMessageDisplayMode
{
	/// <summary>
	/// Validation messages are displayed in the regular way.
	/// </summary>
	Regular,

	/// <summary>
	/// Validation messagse are displayed as a tooltips.
	/// </summary>
	Tooltip,

	/// <summary>
	/// Validation messages with a space reservation.
	/// </summary>
	KeepSpace,

	/// <summary>
	/// Renders no validation message.
	/// </summary>
	None
}