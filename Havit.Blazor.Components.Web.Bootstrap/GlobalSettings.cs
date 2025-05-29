using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings you can set globally for all components. Exposed as <see cref="HxSetup.Defaults"/>.
/// </summary>
public class GlobalSettings
{
	/// <summary>
	/// Specifies how the input labels should be displayed.
	/// </summary>
	public LabelType LabelType { get; set; } = LabelType.Regular;

	/// <summary>
	/// Input size.
	/// </summary>
	public InputSize InputSize { get; set; } = InputSize.Regular;
}
