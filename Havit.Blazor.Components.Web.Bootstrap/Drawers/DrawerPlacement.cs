namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Represents the placement of an drawer.
/// </summary>
public enum DrawerPlacement
{
	/// <summary>
	/// Drawer is placed at the end.
	/// </summary>
	End = 0,

	/// <summary>
	/// Drawer is placed at the start.
	/// </summary>
	Start,

	/// <summary>
	/// Drawer is placed at the top.
	/// </summary>
	Top,

	/// <summary>
	/// Drawer is placed at the bottom.
	/// </summary>
	Bottom,

	/// <summary>
	/// Drawer covers the whole viewport (<c>drawer-fullscreen</c>, new in Bootstrap 6).
	/// </summary>
	Fullscreen
}
