using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxInputRange"/> component.
/// </summary>
public class InputRangeSettings
{
	/// <summary>
	/// Minimum value.
	/// Default is <c>0</c>.
	/// </summary>
	public float? Min { get; set; }

	/// <summary>
	/// Maximum value.
	/// Default is <c>100</c>.
	/// </summary>
	public float? Max { get; set; }

	/// <summary>
	/// By default, slider inputs snap to integer values. To change this, you can specify a step value.
	/// </summary>
	public float? Step { get; set; }

	/// <summary>
	/// Instructs whether the <code>Value</code> is going to be updated instantly, or <code>onchange</code> (usually <code>onmouseup</code>).
	/// </summary>
	public BindEvent? BindEvent { get; set; }
}
