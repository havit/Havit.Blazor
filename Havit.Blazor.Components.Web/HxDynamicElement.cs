namespace Havit.Blazor.Components.Web;

/// <summary>
/// Renders an element with the specified name, attributes, and child content.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxDynamicElement">https://havit.blazor.eu/components/HxDynamicElement</see>
/// </summary>
public class HxDynamicElement : ComponentBase
{
	/// <summary>
	/// Gets or sets the name of the element to render.
	/// </summary>
	[Parameter] public string ElementName { get; set; } = "span";

	/// <summary>
	/// Raised after the element is clicked.
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	/// <remarks>
	/// Please note, that this method is not called, when the <see cref="OnClick"/> parameter is not set.
	/// </remarks>
	protected virtual Task InvokeOnClickAsync(MouseEventArgs args) => OnClick.InvokeAsync(args);

	/// <summary>
	/// Stops onClick event propagation. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool OnClickStopPropagation { get; set; }

	/// <summary>
	/// Prevents the default action for the onclick event. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool OnClickPreventDefault { get; set; }

	/// <summary>
	/// Element reference.
	/// </summary>
	[Parameter] public ElementReference ElementRef { get; set; }

	/// <summary>
	/// Action (synchronous, not an EventCallback) called when the element's reference is captured.
	/// </summary>
	[Parameter] public Action<ElementReference> ElementRefChanged { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	[Parameter(CaptureUnmatchedValues = true)]
	public IDictionary<string, object> AdditionalAttributes { get; set; }

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenElement(0, ElementName);

		if (OnClick.HasDelegate)
		{
			builder.AddAttribute(1, "onclick", InvokeOnClickAsync);
			builder.AddEventPreventDefaultAttribute(2, "onclick", OnClickPreventDefault);
			builder.AddEventStopPropagationAttribute(3, "onclick", OnClickStopPropagation);
		}
		builder.AddMultipleAttributes(4, AdditionalAttributes);
		builder.AddElementReferenceCapture(5, capturedRef =>
		{
			ElementRef = capturedRef;
			ElementRefChanged?.Invoke(ElementRef);
		});
		builder.AddContent(6, ChildContent);

		builder.CloseElement();
	}
}
