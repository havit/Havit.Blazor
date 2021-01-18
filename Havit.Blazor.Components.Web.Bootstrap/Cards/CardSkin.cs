using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class CardSkin
	{
		public static CardSkin DefaultSkin { get; set; }

		/// <summary>
		/// CSS class to be rendered with button.
		/// </summary>
		public string CssClass { get; }
	}
}
