using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{

	/// <summary>
	/// Settings for <see cref="HxFormValue"/>.
	/// </summary>
	public record FormValueSettings : IInputSettingsWithSize
	{
		/// <summary>
		/// Input size. Default is <see cref="InputSize.Regular"/>.
		/// </summary>
		public InputSize? InputSize { get; set; } = Bootstrap.InputSize.Regular;
	}
}
