namespace Havit.Blazor.Components.Web;

/// <summary>
/// Enum for <see href="https://developer.mozilla.org/en-US/docs/Web/HTML/Global_attributes/inputmode">HTML input modes</see>.
/// </summary>
public enum InputMode
{
	/// <summary>
	/// Standard input keyboard for the user's current locale.
	/// </summary>
	Text = 0,

	/// <summary>
	/// No virtual keyboard. For when the page implements its own keyboard input control.
	/// </summary>
	None,

	/// <summary>
	/// Fractional numeric input keyboard containing the digits and decimal separator for the user's locale
	/// (typically <kbd>.</kbd> or <kbd>,</kbd>). Devices may or may not show a minus key (<kbd>-</kbd>).
	/// </summary>
	Decimal,

	/// <summary>
	/// Numeric input keyboard, but only requires the digits <kbd>0</kbd>–<kbd>9</kbd>. Devices may or may not show a minus key.
	/// </summary>
	Numeric,

	/// <summary>
	/// A telephone keypad input, including the digits <kbd>0</kbd>–<kbd>9</kbd>, the asterisk (<kbd>*</kbd>), and the pound (<kbd>#</kbd>) key.
	/// Inputs that *require* a telephone number should typically use <c>InputType.Tel</c> instead.
	/// </summary>
	Tel,

	/// <summary>
	/// A virtual keyboard optimized for search input. For instance, the return/submit key may be labeled
	/// "Search", along with possible other optimizations. Inputs that require a search query should typically
	/// use <c>InputType.Search</c> instead.
	/// </summary>
	Search,

	/// <summary>
	/// A virtual keyboard optimized for entering email addresses. Typically includes the <kbd>@</kbd> character
	/// as well as other optimizations. Inputs that require email addresses should typically use <c>InputType.Email</c> instead.
	/// </summary>
	Email,

	/// <summary>
	/// A keypad optimized for entering URLs. This may have the <kbd>/</kbd> key more prominent, for example.
	/// Enhanced features could include history access and so on. Inputs that require a URL should typically
	/// use <c>InputType.Url</c> instead.
	/// </summary>
	Url
}
