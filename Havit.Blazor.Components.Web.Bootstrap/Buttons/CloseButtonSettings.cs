using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxCloseButton"/> and derived components.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/types/CloseButtonSettings">https://havit.blazor.eu/types/CloseButtonSettings</see>
/// </summary>
public class CloseButtonSettings
{
	/// <summary>
	/// Toggles between the light and dark version of the button.
	/// Default is <c>false</c>.
	/// </summary>
	public bool? White { get; set; }
}
