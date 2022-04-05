using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxInputBase"/> and derived components.
/// </summary>
public class InputsSettings
{
	/// <summary>
	/// Specifies how the validation message should be displayed.
	/// </summary>
	public ValidationMessageDisplayMode? ValidationMessageDisplayMode { get; set; }
}
