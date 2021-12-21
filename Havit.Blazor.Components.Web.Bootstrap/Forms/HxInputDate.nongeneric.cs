using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Non-generic API for <see cref="HxInputDate{TValue}"/>.
	/// Marker for resources for <see cref="HxInputDate{TValue}"/>.
	/// </summary>
	public class HxInputDate
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxInputDate{TValue}"/>.
		/// </summary>
		public static InputDateSettings Defaults { get; } = new InputDateSettings();
	}
}
