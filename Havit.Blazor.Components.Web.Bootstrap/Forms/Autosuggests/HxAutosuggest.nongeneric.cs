using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Non-generic API for <see cref="HxAutosuggest{TItem, TValue}" />.
	/// </summary>
	public class HxAutosuggest
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxInputDate{TValue}"/>.
		/// </summary>
		public static AutosuggestDefaults Defaults { get; } = new AutosuggestDefaults();
	}
}
