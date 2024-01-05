namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Displays a bootstrap icon. See <see href="https://icons.getbootstrap.com/">https://icons.getbootstrap.com/</see>.
/// You usually don't use this component directly, but instead use <see cref="HxIcon" />.
/// </summary>
internal class HxBootstrapIcon : ComponentBase
{
	/// <summary>
	/// The icon to display.
	/// </summary>
	[Parameter] public BootstrapIcon Icon { get; set; }

	/// <summary>
	/// The CSS class to combine with the basic icon CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		// no base call

		builder.OpenElement(0, "i");
		builder.AddAttribute(1, "class", CssClassHelper.Combine("hx-icon", "bi-" + Icon.Name, CssClass));
		builder.AddMultipleAttributes(2, AdditionalAttributes);

		builder.CloseElement(); // i
	}
}
