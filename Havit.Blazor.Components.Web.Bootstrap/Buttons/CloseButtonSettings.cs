using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxCloseButton"/> and derived components.
/// </summary>
public class CloseButtonSettings
{
	/// <summary>
	/// Toggles between the light and dark version of the button.
	/// Default is <c>true</c>.
	/// </summary>
	public bool? Dark { get; set; }
}
