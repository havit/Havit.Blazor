namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Extends <see cref="IFormValueComponent"/> with properties for rendering the <c>form-adorn</c> wrapper
/// (icon/text decorations rendered inside the field chrome, as opposed to input groups which render outside it).
/// </summary>
public interface IFormValueComponentWithAdorns : IFormValueComponent
{
	/// <summary>
	/// Custom CSS class to be rendered with the <c>form-adorn</c> wrapper.
	/// </summary>
	string AdornCssClass => null;

	/// <summary>
	/// Text adornment at the beginning of the input, rendered as <c>.form-adorn-text</c>.
	/// </summary>
	string AdornStartText => null;

	/// <summary>
	/// Adornment (typically an icon) at the beginning of the input, rendered as <c>.form-adorn-icon</c>.
	/// </summary>
	RenderFragment AdornStartTemplate => null;

	/// <summary>
	/// Text adornment at the end of the input, rendered as <c>.form-adorn-text</c>.
	/// </summary>
	string AdornEndText => null;

	/// <summary>
	/// Adornment (typically an icon) at the end of the input, rendered as <c>.form-adorn-icon</c>.
	/// </summary>
	RenderFragment AdornEndTemplate => null;
}
