namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxInputText"/> and derived components.
/// </summary>
public record InputTextSettings : InputSettings
{
	/// <summary>
	/// Size of the input.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// The label type.
	/// </summary>
	public LabelType? LabelType { get; set; }

	/// <summary>
	/// Hint to browsers regarding the type of virtual keyboard configuration to use when editing.
	/// </summary>
	public InputMode? InputMode { get; set; }

	/// <summary>
	/// Determines whether all the text within the input field is automatically selected when it receives focus.
	/// </summary>
	public bool? SelectOnFocus { get; set; }

	/// <summary>
	/// Hint to browsers regarding the spell checking of the input.
	/// </summary>
	public bool? Spellcheck { get; set; }
}
