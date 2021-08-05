using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Non-generic API for <see cref="HxInputNumber{TValue}"/>.
	/// Marker for resources for <see cref="HxInputNumber{TValue}"/>.
	/// </summary>
	public class HxInputNumber
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxAutosuggest{TItem, TValue}"/>.
		/// </summary>
		public static InputNumberDefaults Defaults { get; } = new InputNumberDefaults();
	}
}
