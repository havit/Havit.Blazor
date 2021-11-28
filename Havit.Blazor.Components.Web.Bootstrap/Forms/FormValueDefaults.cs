using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap.Forms
{

	/// <summary>
	/// Default values for <see cref="HxFormValue"/> and derived components.
	/// </summary>
	public class FormValueDefaults : IInputDefaultsWithSize
	{
		/// <summary>
		/// Input size. Default is <see cref="InputSize.Regular"/>.
		/// </summary>
		public InputSize InputSize { get; set; } = InputSize.Regular;
	}
}
