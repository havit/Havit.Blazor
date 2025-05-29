
namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Input with sizing support.
/// </summary>
public interface IInputWithSize
{
	// We do not want to force the InputSize property here as only InputSizeEffective is needed
	// to provide necessary information for the rendering.
	// The Internal implementation components do not have the InputSize, they use just InputSizeEffective.

	/// <summary>
	/// The effective size of the input after all the defaults and cascading settings are applied.
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
