using Havit.Blazor.Components.Web.Infrastructure;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Generic item for the <see cref="HxDropdownMenu"/>.
/// </summary>
public partial class HxDropdownItem : ICascadeEnabledComponent
{
	[CascadingParameter] protected FormState FormState { get; set; }
	FormState ICascadeEnabledComponent.FormState { get => FormState; set => FormState = value; }

	/// <summary>
	/// Additional CSS class for the underlying <c>li&gt;span</c> element.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional CSS class for the underlying <c>li</c> container (wrapping the main <c>span</c> inside).
	/// </summary>
	[Parameter] public string ContainerCssClass { get; set; }

	/// <summary>
	/// Raised when the item is clicked.
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClickAsync(MouseEventArgs args) => OnClick.InvokeAsync(args);

	/// <inheritdoc cref="ICascadeEnabledComponent.Enabled" />
	[Parameter] public bool? Enabled { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto the underlying <c>li&gt;span</c> element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }
}
