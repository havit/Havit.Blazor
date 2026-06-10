namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Responsive horizontal setting (breakpoint) for <see cref="HxStepper"/>.
/// The responsive breakpoints are container query based (they react to the width of the containing element, not the viewport).
/// Default is <see cref="StepperHorizontal.Never"/>.
/// </summary>
public enum StepperHorizontal
{
	/// <summary>
	/// Never horizontal, always vertical.
	/// </summary>
	Never = 0,

	/// <summary>
	/// Horizontal for containers above the "small" breakpoint (<c>576px</c>).
	/// </summary>
	SmallUp,

	/// <summary>
	/// Horizontal for containers above the "medium" breakpoint (<c>768px</c>).
	/// </summary>
	MediumUp,

	/// <summary>
	/// Horizontal for containers above the "large" breakpoint (<c>1024px</c>).
	/// </summary>
	LargeUp,

	/// <summary>
	/// Horizontal for containers above the "extra-large" breakpoint (<c>1280px</c>).
	/// </summary>
	ExtraLargeUp,

	/// <summary>
	/// Horizontal for containers above the "2xl" breakpoint (<c>1536px</c>).
	/// </summary>
	XxlUp,

	/// <summary>
	/// Always horizontal, never vertical.
	/// </summary>
	Always
}
