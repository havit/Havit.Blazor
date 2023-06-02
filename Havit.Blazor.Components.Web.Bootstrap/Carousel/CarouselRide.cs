namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Behavior of the <see cref="HxCarousel"/>.
/// </summary>
public enum CarouselRide
{
	/// <summary>
	/// Autoplays the carousel on load.
	/// </summary>
	Carousel = 0,

	/// <summary>
	/// Manual control of the carousel.
	/// </summary>
	False = 1,

	/// <summary>
	/// Autoplays the carousel after the user manually cycles the first item.
	/// </summary>
	True = 2
}
