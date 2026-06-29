namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Defines what happens at the first and last slide of a <see cref="HxCarousel"/>.
/// </summary>
public enum CarouselEnds
{
	/// <summary>
	/// Scrolls seamlessly past the ends for an endless conveyor effect (default).
	/// Applies to single-slide carousels; multi-item, peek, centered, and variable-width
	/// layouts (and users who prefer reduced motion) fall back to <see cref="Wrap"/>.
	/// </summary>
	Loop = 0,

	/// <summary>
	/// Jumps from the last slide back to the first (and vice versa), so the controls never reach a dead end.
	/// </summary>
	Wrap = 1,

	/// <summary>
	/// Hard-stops at the first and last slide. The previous control is disabled on the first slide
	/// and the next control on the last (via the native <c>disabled</c> attribute on <c>&lt;button&gt;</c> controls).
	/// </summary>
	Stop = 2
}
