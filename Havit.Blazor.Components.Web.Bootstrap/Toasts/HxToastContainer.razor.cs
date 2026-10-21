namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Positions and stacks the <see cref="HxToast"/> messages on the screen; rendered automatically by <see cref="HxMessenger"/>.
/// </summary>
public partial class HxToastContainer : ComponentBase
{
	/// <summary>
	/// Positioning of the toasts on the screen (fixed to the viewport).
	/// With <see cref="ToastContainerPosition.None"/>, the container is rendered in the normal document flow.
	/// </summary>
	/// <remarks>
	/// A positioned container is moved to the browser top layer when a modal <see cref="HxDialog"/> or <see cref="HxDrawer"/> opens,
	/// so the toasts stay visible above it.
	/// </remarks>
	[Parameter] public ToastContainerPosition Position { get; set; }

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Toasts to display.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	private string GetPositionCss()
	{
		// Own positioning classes (see HxToastContainer.razor.css) instead of the position utilities - they have to fully control the inset
		// (incl. resetting the UA [popover] styles) when the container is moved to the top layer (see HxToast.js).
		return Position switch
		{
			ToastContainerPosition.TopStart => "hx-toast-container-fixed hx-toast-container-top hx-toast-container-start",
			ToastContainerPosition.TopCenter => "hx-toast-container-fixed hx-toast-container-top hx-toast-container-center",
			ToastContainerPosition.TopEnd => "hx-toast-container-fixed hx-toast-container-top hx-toast-container-end",
			ToastContainerPosition.MiddleStart => "hx-toast-container-fixed hx-toast-container-middle hx-toast-container-start",
			ToastContainerPosition.MiddleCenter => "hx-toast-container-fixed hx-toast-container-middle hx-toast-container-center",
			ToastContainerPosition.MiddleEnd => "hx-toast-container-fixed hx-toast-container-middle hx-toast-container-end",
			ToastContainerPosition.BottomStart => "hx-toast-container-fixed hx-toast-container-bottom hx-toast-container-start",
			ToastContainerPosition.BottomCenter => "hx-toast-container-fixed hx-toast-container-bottom hx-toast-container-center",
			ToastContainerPosition.BottomEnd => "hx-toast-container-fixed hx-toast-container-bottom hx-toast-container-end",
			ToastContainerPosition.None => "position-static",
			_ => throw new InvalidOperationException($"Unknown {nameof(Position)} value.")
		};
	}
}
