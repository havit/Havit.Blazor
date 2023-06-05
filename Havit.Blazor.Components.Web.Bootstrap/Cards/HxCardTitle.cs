namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap <see href="https://getbootstrap.com/docs/5.3/components/card/#titles-text-and-links">card-title</see> component.
/// </summary>
public class HxCardTitle : ComponentBase
{
	/// <summary>
	/// Text content.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Underlying HTML element for card-title. Default is <c>h5</c>.
	/// </summary>
	[Parameter] public string HtmlElement { get; set; } = "h5";

	/// <summary>
	/// Additional CSS class(es) for the card-title element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(1, HtmlElement);

		builder.AddAttribute(2, "class", CssClassHelper.Combine("card-title", this.CssClass));
		builder.AddMultipleAttributes(3, AdditionalAttributes);
		builder.AddContent(4, ChildContent);

		builder.CloseElement();
	}
}
