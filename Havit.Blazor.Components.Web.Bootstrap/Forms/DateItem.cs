using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Date item for <see cref="HxInputDate" />.
	/// </summary>
	public class DateItem
	{
		/// <summary>
		/// Custom label.
		/// </summary>
		public string Label { get; set; }

		/// <summary>
		/// Date.
		/// </summary>
		public DateTime Date { get; set; }
	}
}
