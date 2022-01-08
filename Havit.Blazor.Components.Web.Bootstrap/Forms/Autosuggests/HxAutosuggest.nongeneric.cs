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
		/// Application-wide defaults for the <see cref="HxAutosuggest{TItem, TValue}"/> and derived components.
		/// </summary>
		public static AutosuggestSettings Defaults { get; set; }

		static HxAutosuggest()
		{
			Defaults = new AutosuggestSettings()
			{
				InputSize = Bootstrap.InputSize.Regular,
				SearchIcon = BootstrapIcon.Search,
				ClearIcon = BootstrapIcon.XCircleFill,
				MinimumLength = 2,
				Delay = 300,
			};
		}
	}
}
