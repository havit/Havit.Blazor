namespace Havit.Blazor.Components.Web;

/// <summary>
/// Enum for <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input#input_types">HTML input types</see>.
/// </summary>
/// <remarks>
/// As the enum is currently used only for <c>HxInputText</c> component, only relevant types are included.
/// As all the values will by needed, they can be added any later (add restrictions/validation to <c>HxInputText</c> then).
/// </remarks>
public enum InputType
{
	/// <summary>
	/// The default value. A single-line text field. Line-breaks are automatically removed from the input value.
	/// </summary>
	Text = 0,

	/// <summary>
	/// A field for editing an email address. Looks like a <see cref="Text"/> input, but has validation parameters
	/// and relevant keyboard in supporting browsers and devices with dynamic keyboards.
	/// </summary>
	Email,

	/// <summary>
	/// A single-line text field whose value is obscured. Will alert user if site is not secure.
	/// </summary>
	Password,

	/// <summary>
	/// A single-line text field for entering search strings. Line-breaks are automatically removed from the input value.
	/// May include a delete icon in supporting browsers that can be used to clear the field. Displays a search icon instead
	/// of enter key on some devices with dynamic keypads.
	/// </summary>
	Search,

	/// <summary>
	/// 	A control for entering a telephone number. Displays a telephone keypad in some devices with dynamic keypads.
	/// </summary>
	Tel,


	/// <summary>
	/// A field for entering a URL. Looks like a <see cref="Text"/> input, but has validation parameters and relevant
	/// keyboard in supporting browsers and devices with dynamic keyboards.
	/// </summary>
	Url,
}