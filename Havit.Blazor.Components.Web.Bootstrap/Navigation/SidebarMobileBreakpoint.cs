namespace Havit.Blazor.Components.Web.Bootstrap;

public enum SidebarResponsiveBreakpoint
{
	/// <summary>
	/// Mobile mode disabled, always render as a sidebar.
	/// </summary>
	None = 0,

	/// <summary>
	/// Mobile for viewports below the "small" breakpoint (<c>576px</c>, exclusive).
	/// </summary>
	Small = 1,

	/// <summary>
	/// Mobile for viewports below the "medium" breakpoint (<c>768px</c>, exclusive).
	/// </summary>
	Medium = 3,

	/// <summary>
	/// Mobile for viewports below the "large" breakpoint (<c>992px</c>, exclusive).
	/// </summary>
	Large = 4,

	/// <summary>
	/// Mobile for viewports below the "extra-large" breakpoint (<c>1200px</c>, exclusive).
	/// </summary>
	ExtraLarge = 5,

	/// <summary>
	/// Mobile for viewports below the "XXL" breakpoint (<c>1400px</c>, exclusive).
	/// </summary>
	Xxl = 6,
}
