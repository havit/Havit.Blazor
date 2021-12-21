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
		/// Application-wide defaults for the <see cref="HxAutosuggest{TItem, TValue}"/>.
		/// </summary>
		public static AutosuggestSettings Defaults { get; } = new AutosuggestSettings();
	}
}
