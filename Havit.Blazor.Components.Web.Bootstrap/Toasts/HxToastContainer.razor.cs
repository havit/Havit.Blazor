namespace Havit.Blazor.Components.Web.Bootstrap;

public partial class HxToastContainer : ComponentBase
{
	/// <summary>
	/// Positioning of the toasts on screen.
	/// </summary>
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
		// https://getbootstrap.com/docs/5.3/utilities/position/#center-elements
		return this.Position switch
		{
			ToastContainerPosition.TopStart => "position-fixed top-0 start-0",
			ToastContainerPosition.TopCenter => "position-fixed start-50 translate-middle-x",
			ToastContainerPosition.TopEnd => "position-fixed top-0 end-0",
			ToastContainerPosition.MiddleStart => "position-fixed top-50 start-0 translate-middle-y",
			ToastContainerPosition.MiddleCenter => "position-fixed top-50 start-50 translate-middle",
			ToastContainerPosition.MiddleEnd => "position-fixed top-50 end-0 translate-middle-y",
			ToastContainerPosition.BottomStart => "position-fixed bottom-0 start-0",
			ToastContainerPosition.BottomCenter => "position-fixed bottom-0 start-50 translate-middle-x",
			ToastContainerPosition.BottomEnd => "position-fixed bottom-0 end-0",
			ToastContainerPosition.None => null,
			_ => throw new InvalidOperationException($"Unknown {nameof(Position)} value.")
		};
	}
}
