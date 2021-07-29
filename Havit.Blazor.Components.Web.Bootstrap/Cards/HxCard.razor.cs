using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxCard
	{
		[Parameter] public RenderFragment HeaderTemplate { get; set; }
		[Parameter] public RenderFragment BodyTemplate { get; set; }
		[Parameter] public RenderFragment FooterTemplate { get; set; }
		/// <summary>
		/// Additional CSS classes for the card-container.
		/// </summary>
		[Parameter] public string CssClass { get; set; }
		[Parameter] public string HeaderCssClass { get; set; }
		[Parameter] public string BodyCssClass { get; set; }
		[Parameter] public string FooterCssClass { get; set; }
		/// <summary>
		/// Skin to be applied. You can set a default skin to <see cref="HxCard.DefaultSkin"/>. It will be applied to any <see cref="HxCard"/> without explicit skin specified.
		/// </summary>
		[Parameter] public CardSkin Skin { get; set; }

		/// <summary>
		/// A skin to be used for any <see cref="HxCard"/> without explicit <see cref="HxCard.Skin"/> specified.
		/// </summary>
		public static CardSkin DefaultSkin { get; set; }
	}
}
