namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Placement of the <see cref="HxCarousel"/> controls (previous/next, play/pause) and indicators
/// relative to the slides. Does not apply to <see cref="HxCarousel.Overlay"/> carousels.
/// </summary>
public enum CarouselControlsPlacement
{
	/// <summary>
	/// Controls are rendered below the slides (default).
	/// </summary>
	Bottom = 0,

	/// <summary>
	/// Controls are rendered above the slides.
	/// </summary>
	Top = 1
}
