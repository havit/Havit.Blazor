namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Validation message display mode for <see cref="HxValidationMessage{TValue}"/>
/// </summary>
public enum ValidationMessageMode
{
	/// <summary>
	/// Validation messages are displayed in the regular way (takes space, when displayed).
	/// </summary>
	Regular = 0,

	/// <summary>
	/// Validation messages are displayed as a tooltips.
	/// </summary>
	Tooltip = 1,

	/// <summary>
	/// Validation messages, which do not take extra space when displayed.
	/// </summary>
	/// <remarks>
	/// We want to avoid changing position of the form submit button to prevent the "no onclick/onsubmit issue when button position changes".
	/// </remarks>
	Floating = 2,

	/// <summary>
	/// Renders no validation message.
	/// </summary>
	None = 0xFF
}