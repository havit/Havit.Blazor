using Microsoft.Extensions.Localization;

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
	[Inject] protected IStringLocalizer<HxCloseButton> Localizer { get; set; }

	/// <summary>
	/// The close button is a standalone <c>.btn-close</c> element (no <c>.btn</c>): the <c>.btn</c> hover/disabled backgrounds would paint over the masked icon.
	/// </summary>
	protected override string CoreCssClass => "btn-close";

	/// <summary>
	/// Renders only <see cref="CoreCssClass"/> and <see cref="HxButton.CssClass"/>; the button variant, color and size classes do not apply to the close button.
	/// </summary>
	protected override string GetButtonCssClass()
	{
		return CssClassHelper.Combine(CoreCssClass, CssClassEffective);
	}

	/// <summary>
	/// Returns <see cref="HxButton.AriaLabel"/> or a localized "Close" text when not set.
	/// </summary>
	protected override string GetAriaLabel() => AriaLabel ?? Localizer["Close"];
}
