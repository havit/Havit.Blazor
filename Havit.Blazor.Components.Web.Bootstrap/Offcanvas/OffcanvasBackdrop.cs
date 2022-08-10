using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

public enum OffcanvasBackdrop
{
	/// <summary>
	/// No backdrop will be rendered. User can interact with other parts of the app while the offcanvas is open.
	/// </summary>
	False,

	/// <summary>
	/// A backdrop will be rendered. Offcanvas will be closed upon clicking on the backdrop.
	/// </summary>
	True,

	/// <summary>
	/// A static backdrop will be rendered. Offcanvas will not be closed upon clicking on the backdrop.
	/// </summary>
	Static
}
