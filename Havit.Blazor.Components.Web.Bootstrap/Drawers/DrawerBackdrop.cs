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
	/// No backdrop will be rendered. Users can interact with other parts of the app while the drawer is open.
	/// </summary>
	False = 1,

	/// <summary>
	/// A static backdrop will be rendered. The drawer will not be closed upon clicking on the backdrop.
	/// </summary>
	Static = 2
}
