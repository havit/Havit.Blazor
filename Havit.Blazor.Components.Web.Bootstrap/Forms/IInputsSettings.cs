using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the components derived from <see cref="HxInputBase"/>.
/// </summary>
public interface IInputsSettings
{
	/// <summary>
	/// Specifies how the validation message should be displayed.
	/// </summary>
	public ValidationMessageMode? ValidationMessageMode { get; set; }
}
