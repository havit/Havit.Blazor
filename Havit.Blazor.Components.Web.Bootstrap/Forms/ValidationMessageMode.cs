namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Validation message display mode for <see cref="HxValidationMessage{TValue}"/>
/// </summary>
public enum ValidationMessageMode
{
	/// <summary>
	/// Validation messages are displayed in the regular way (occupy space when displayed).
	/// </summary>
	Regular = 0,

	/// <summary>
	/// Validation messages are displayed as tooltips.
	/// </summary>
	Tooltip = 1,

	/// <summary>
	/// Validation messages that do not occupy extra space when displayed.
	/// </summary>
	/// <remarks>
	/// We want to avoid changing the position of the form submit button to prevent the "no onclick/onsubmit issue when button position changes".
	/// </remarks>
	Floating = 2,

	/// <summary>
	/// Validation messages are rendered but visually hidden (e.g., for screen readers).
	/// </summary>
	VisuallyHidden = 3,

	/// <summary>
	/// Renders no validation message.
	/// </summary>
	None = 0xFF
}
