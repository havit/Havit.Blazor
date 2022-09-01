using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxInputRange{TValue}"/> component.
/// </summary>
public record InputRangeSettings : InputsSettings
{
	/// <summary>
	/// Instructs whether the <code>Value</code> is going to be updated instantly, or <code>onchange</code> (usually <code>onmouseup</code>).
	/// </summary>
	public BindEvent? BindEvent { get; set; }
}
