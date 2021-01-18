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
		/// A skin to be used for any <see cref="HxCard"/> without explicit <see cref="HxCard.Skin"/> specified.
		/// </summary>
		public static CardSkin Default { get; set; }

		/// <summary>
		/// Empty skin to be able to disable <see cref="Default"/> locally.
		/// </summary>
		public static CardSkin None => new CardSkin();

		/// <summary>
		/// CSS class to be rendered with button.
		/// </summary>
		public string CssClass { get; set; }
	}
}
