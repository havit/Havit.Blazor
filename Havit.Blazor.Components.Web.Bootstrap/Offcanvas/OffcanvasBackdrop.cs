namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Options for controlling the behavior of the <see cref="HxOffcanvas.Backdrop"/>.
/// </summary>
public enum OffcanvasBackdrop
{
	/// <summary>
	/// A backdrop will be rendered. The offcanvas will be closed upon clicking on the backdrop.
	/// </summary>
	True = 0,

	/// <summary>
	/// No backdrop will be rendered. Users can interact with other parts of the app while the offcanvas is open.
	/// </summary>
	False = 1,

	/// <summary>
	/// A static backdrop will be rendered. The offcanvas will not be closed upon clicking on the backdrop.
	/// </summary>
	Static = 2
}
