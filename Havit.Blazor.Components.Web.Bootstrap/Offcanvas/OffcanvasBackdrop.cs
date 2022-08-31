using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Options for controlling the behavior of the <see cref="HxOffcanvas.Backdrop"/>.
/// </summary>
public enum OffcanvasBackdrop
{
	/// <summary>
	/// A backdrop will be rendered. Offcanvas will be closed upon clicking on the backdrop.
	/// </summary>
	True = 0,

	/// <summary>
	/// No backdrop will be rendered. User can interact with other parts of the app while the offcanvas is open.
	/// </summary>
	False = 1,

	/// <summary>
	/// A static backdrop will be rendered. Offcanvas will not be closed upon clicking on the backdrop.
	/// </summary>
	Static = 2
}
