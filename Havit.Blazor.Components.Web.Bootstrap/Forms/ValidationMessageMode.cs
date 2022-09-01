namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Validation message display mode for <see cref="HxValidationMessage{TValue}"/>
/// </summary>
public enum ValidationMessageMode
{
	/// <summary>
	/// Validation messages are displayed in the regular way.
	/// </summary>
	Regular = 0,

	/// <summary>
	/// Validation messages are displayed as a tooltips.
	/// </summary>
	Tooltip = 1,

	/// <summary>
	/// Validation messages with a space reservation.
	/// </summary>
	KeepSpace = 2,

	/// <summary>
	/// Renders no validation message.
	/// </summary>
	None = 0xFF
}