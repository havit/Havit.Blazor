namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Fullscreen behavior for <see cref="HxModal"/>.
/// </summary>
public enum ModalFullscreen
{
	/// <summary>
	/// Fullscreen mode disabled,
	/// </summary>
	Disabled = 0,

	/// <summary>
	/// Always fullscreen modal.
	/// </summary>
	Always = 1,

	/// <summary>
	/// Fullscreen for viewports bellow "small" breakpoint (<c>576px</c>).
	/// </summary>
	SmallDown = 2,

	/// <summary>
	/// Fullscreen for viewports bellow "medium" breakpoint (<c>768px</c>).
	/// </summary>
	MediumDown = 3,

	/// <summary>
	/// Fullscreen for viewports bellow "large" breakpoint (<c>992px</c>).
	/// </summary>
	LargeDown = 4,

	/// <summary>
	/// Fullscreen for viewports bellow "extra-large" breakpoint (<c>1200px</c>).
	/// </summary>
	ExtraLargeDown = 5,

	/// <summary>
	/// Fullscreen for viewports bellow "XXL" breakpoint (<c>1400px</c>).
	/// </summary>
	XxlDown = 6,
}