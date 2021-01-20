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
		[Parameter] public string CssClass { get; set; }
		[Parameter] public string HeaderCssClass { get; set; }
		[Parameter] public string BodyCssClass { get; set; }
		[Parameter] public string FooterCssClass { get; set; }
		[Parameter] public CardSkin Skin { get; set; }
	}
}
