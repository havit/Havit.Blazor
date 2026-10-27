namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Options for controlling the behavior of the <see cref="HxDrawer.Backdrop"/>.
/// </summary>
public enum DrawerBackdrop
{
	/// <summary>
	/// A backdrop will be rendered. The drawer will be closed upon clicking on the backdrop.
	/// </summary>
	True = 0,

	/// <summary>
	/// No (visible) backdrop will be rendered and clicking outside the drawer does not close it.
	/// To let users interact with other parts of the app while the drawer is open, set <see cref="HxDrawer.ScrollingEnabled"/> to <c>true</c>
	/// (the drawer opens as non-modal); otherwise the drawer is modal and the rest of the page stays inert.
	/// </summary>
	False = 1,

	/// <summary>
	/// A static backdrop will be rendered. The drawer will not be closed upon clicking on the backdrop.
	/// </summary>
	Static = 2
}
