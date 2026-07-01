namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Bootstrap <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/close-button/">close-button</see> component.<br />
/// A simple close button for dismissing content such as dialogs and alerts.
/// The icon is rendered with a CSS <c>mask-image</c> tinted with <c>currentcolor</c>, so it adapts to the surrounding
/// text color and color mode automatically (the former <c>White</c> parameter/<c>btn-close-white</c> class no longer exist;
/// use <c>data-bs-theme="dark"</c> on the button or a parent element to invert it explicitly).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxCloseButton">https://havit.blazor.eu/components/HxCloseButton</see>
/// </summary>
public class HxCloseButton : HxButton
{
	protected override string CoreCssClass => $"{base.CoreCssClass} btn-close";

	public HxCloseButton()
	{
		AdditionalAttributes ??= new();
		AdditionalAttributes.Add("aria-label", "Close"); // Adding the aria-label for accessibility.
	}
}
