namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Represents properties (and methods) of a component rendering a form value (ie. form inputs).
/// </summary>
public interface IFormValueComponent
{
	/// <summary>
	/// Custom CSS class to render with wrapping div.
	/// </summary>
	string CssClass => null;

	/// <summary>
	/// Label to render before input (or after input for Checkbox).		
	/// </summary>
	string Label => null;

	/// <summary>
	/// Label to render before input (or after input for Checkbox).
	/// </summary>
	RenderFragment LabelTemplate => null;

	/// <summary>
	/// Element id to render as label for attribute.
	/// </summary>
	string LabelFor => null;

	/// <summary>
	/// Custom CSS class to render with the label.
	/// </summary>
	string LabelCssClass => null;

	/// <summary>
	/// Renders content of the component (value, input).
	/// </summary>
	/// <param name="builder"></param>
	void RenderValue(RenderTreeBuilder builder); // no default implementation!

	/// <summary>
	/// Renders validation message.
	/// </summary>
	void RenderValidationMessage(RenderTreeBuilder builder) { /* NOOP (default implementation) */ }

	/// <summary>
	/// Hint to render after input as form-text.
	/// </summary>
	string Hint => null;

	/// <summary>
	/// Hint to render after input as form-text.
	/// </summary>
	RenderFragment HintTemplate => null;

	/// <summary>
	/// CSS class to be rendered with the wrapping div.
	/// </summary>
	string CoreCssClass => "hx-form-group position-relative";

	/// <summary>
	/// CSS class to be rendered with the label.
	/// </summary>
	string CoreLabelCssClass => "form-label";

	/// <summary>
	/// CSS class to be rendered with the hint.
	/// </summary>
	string CoreHintCssClass => "form-text";

	/// <summary>
	/// Render order LabelValue or ValueLabel.
	/// </summary>
	LabelValueRenderOrder RenderOrder => LabelValueRenderOrder.LabelValue;
}
