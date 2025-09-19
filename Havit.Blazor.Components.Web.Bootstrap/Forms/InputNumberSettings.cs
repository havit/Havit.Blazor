namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxInputNumber{TValue}"/> and derived components.
/// </summary>
public record InputNumberSettings : InputSettings
{
	/// <summary>
	/// The size of the input.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// The label type.
	/// </summary>
	public LabelType? LabelType { get; set; }

	/// <summary>
	/// A hint to browsers regarding the type of virtual keyboard configuration to use when editing.
	/// </summary>
	public InputMode? InputMode { get; set; }

	/// <summary>
	/// Allows switching between textual and numeric input types.
	/// Only <see cref="InputType.Text"/> (default) and <see cref="InputType.Number"/> are supported.
	/// </summary>
	public InputType? Type { get; set; }

	/// <summary>
	/// Determines whether all the content within the input field is automatically selected when it receives focus.
	/// </summary>
	public bool? SelectOnFocus { get; set; }

	/// <summary>
	/// When enabled, pasted values are normalized to contain only valid numeric characters.
	/// </summary>
	public bool? SmartPaste { get; set; }

	/// <summary>
	/// When enabled, the input may provide an optimized keyboard experience for numeric entry.
	/// Currently, this means whenever a minus key is pressed, the sign of the number is toggled.
	/// </summary>
	public bool? SmartKeyboard { get; set; }
}
