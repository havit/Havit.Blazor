namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Fullscreen behavior for <see cref="HxModal"/>.
/// </summary>
public enum ModalFullscreen
{
	/// <summary>
	/// Fullscreen mode disabled.
	/// </summary>
	Disabled = 0,

	/// <summary>
	/// Always fullscreen modal.
	/// </summary>
	Always = 1,

	/// <summary>
	/// Fullscreen for viewports below the "small" breakpoint (<c>576px</c>).
	/// </summary>
	SmallDown = 2,

	/// <summary>
	/// Fullscreen for viewports below the "medium" breakpoint (<c>768px</c>).
	/// </summary>
	MediumDown = 3,

	/// <summary>
	/// Fullscreen for viewports below the "large" breakpoint (<c>1024px</c>).
	/// </summary>
	LargeDown = 4,

	/// <summary>
	/// Fullscreen for viewports below the "extra-large" breakpoint (<c>1280px</c>).
	/// </summary>
	ExtraLargeDown = 5,

	/// <summary>
	/// Fullscreen for viewports below the "2xl" breakpoint (<c>1536px</c>).
	/// </summary>
	XxlDown = 6,
}
