namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Responsive horizontal setting (breakpoint) for <see cref="HxListGroup"/>.
/// Default is <see cref="ListGroupHorizontal.Never"/>.
/// </summary>
public enum ListGroupHorizontal
{
	/// <summary>
	/// Never horizontal, always vertical.
	/// </summary>
	Never = 0,

	/// <summary>
	/// Horizontal for viewports above the "small" breakpoint (<c>576px</c>).
	/// </summary>
	SmallUp,

	/// <summary>
	/// Horizontal for viewports above the "medium" breakpoint (<c>768px</c>).
	/// </summary>
	MediumUp,

	/// <summary>
	/// Horizontal for viewports above the "large" breakpoint (<c>992px</c>).
	/// </summary>
	LargeUp,

	/// <summary>
	/// Horizontal for viewports above the "extra-large" breakpoint (<c>1200px</c>).
	/// </summary>
	ExtraLargeUp,

	/// <summary>
	/// Horizontal for viewports above the "XXL" breakpoint (<c>1400px</c>).
	/// </summary>
	XxlUp,

	/// <summary>
	/// Always horizontal, never vertical.
	/// </summary>
	Always
}
