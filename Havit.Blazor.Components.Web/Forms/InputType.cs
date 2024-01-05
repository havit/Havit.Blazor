namespace Havit.Blazor.Components.Web;

/// <summary>
/// Enum for <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Element/input#input_types">HTML input types</see>.
/// </summary>
/// <remarks>
/// As the enum is currently used only for the <c>HxInputText</c> component, only relevant types are included.
/// As all the values will be needed, they can be added later (add restrictions/validation to <c>HxInputText</c> then).
/// </remarks>
public enum InputType
{
	/// <summary>
	/// The default value. A single-line text field. Line-breaks are automatically removed from the input value.
	/// </summary>
	Text = 0,

	/// <summary>
	/// A field for editing an email address. It looks like a <see cref="Text"/> input, but has validation parameters
	/// and a relevant keyboard in supporting browsers and devices with dynamic keyboards.
	/// </summary>
	Email,

	/// <summary>
	/// A single-line text field whose value is obscured. It will alert the user if the site is not secure.
	/// </summary>
	Password,

	/// <summary>
	/// A single-line text field for entering search strings. Line-breaks are automatically removed from the input value.
	/// It may include a delete icon in supporting browsers that can be used to clear the field. It displays a search icon instead
	/// of the enter key on some devices with dynamic keypads.
	/// </summary>
	Search,

	/// <summary>
	/// A control for entering a telephone number. It displays a telephone keypad on some devices with dynamic keypads.
	/// </summary>
	Tel,


	/// <summary>
	/// A field for entering a URL. It looks like a <see cref="Text"/> input, but has validation parameters and a relevant
	/// keyboard in supporting browsers and devices with dynamic keyboards.
	/// </summary>
	Url,
}
