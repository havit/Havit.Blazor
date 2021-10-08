using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Defaults for the <see cref="HxInputFile"/>.
	/// </summary>
	public class InputFileDefaults : IInputDefaultsWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		public InputSize InputSize { get; set; } = InputSize.Regular;
	}
}
