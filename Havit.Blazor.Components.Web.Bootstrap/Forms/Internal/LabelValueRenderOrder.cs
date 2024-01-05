namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Specifies the render order of label and value/input.
/// </summary>
public enum LabelValueRenderOrder
{
	/// <summary>
	/// Renders the label first, followed by the value/input (used by the majority of components).
	/// </summary>
	LabelValue,

	/// <summary>
	/// Renders the value/input first, followed by the label (used by the former HxInputCheckbox).
	/// Obsolete, should not be needed anymore.
	/// </summary>
	ValueLabel,

	/// <summary>
	/// Renders only the value/input. The label is not rendered (used by HxAutosuggest{TItem, TValue} with floating label which renders the label internally).
	/// </summary>
	ValueOnly
}
