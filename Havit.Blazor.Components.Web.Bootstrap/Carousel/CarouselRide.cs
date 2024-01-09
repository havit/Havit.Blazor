namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Behavior of the <see cref="HxCarousel"/>.
/// </summary>
public enum CarouselRide
{
	/// <summary>
	/// Automatically starts the carousel on load.
	/// </summary>
	Carousel = 0,

	/// <summary>
	/// Allows manual control of the carousel.
	/// </summary>
	False = 1,

	/// <summary>
	/// Automatically starts the carousel after the user manually cycles the first item.
	/// </summary>
	True = 2
}
