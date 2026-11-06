namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Open/close animation of the <see cref="HxDialog"/>.
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/dialog/#animations"/>.
/// </summary>
public enum DialogAnimation
{
	/// <summary>
	/// Fade on open and close (Bootstrap default).
	/// </summary>
	Fade = 0,

	/// <summary>
	/// No animation (<c>dialog-instant</c>).
	/// </summary>
	None,

	/// <summary>
	/// Slides down from the top of the viewport (<c>dialog-slide-down</c>).
	/// </summary>
	SlideDown,

	/// <summary>
	/// Slides up from the bottom of the viewport (<c>dialog-slide-up</c>).
	/// </summary>
	SlideUp
}
