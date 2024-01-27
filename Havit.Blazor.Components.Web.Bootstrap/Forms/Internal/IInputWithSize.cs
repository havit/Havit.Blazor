
namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Input with sizing support.
/// </summary>
public interface IInputWithSize
{
	/// <summary>
	/// The size of the input.
	/// </summary>
	InputSize InputSizeEffective { get; }

	/// <summary>
	/// Gets the CSS class for the input size.
	/// </summary>
	string GetInputSizeCssClass() => InputSizeEffective.AsFormControlCssClass();

	/// <summary>
	/// Gets the CSS class for the input group size.
	/// </summary>
	string GetInputGroupSizeCssClass() => InputSizeEffective.AsInputGroupCssClass();
}
