using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class CardSkin
	{
		/// <summary>
		/// Empty skin to be able to disable <see cref="HxCard.DefaultSkin"/> locally.
		/// </summary>
		public static CardSkin None => new CardSkin();

		public string CssClass { get; set; }
		public string HeaderCssClass { get; set; }
		public string BodyCssClass { get; set; }
		public string FooterCssClass { get; set; }
	}
}
