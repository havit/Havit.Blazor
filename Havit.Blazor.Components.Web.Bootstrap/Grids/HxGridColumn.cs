using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public class HxGridColumn<TItemType> : HxGridColumnBase<TItemType>
	{
		// TODO: Suppress SA1134 je v CSPROJ, uvolnit celofiremně?
		[Parameter] public string HeaderText { get; set; }
		[Parameter] public RenderFragment HeaderTemplate { get; set; }
		[Parameter] public Func<TItemType, string> ItemText { get; set; } // TODO: Na rozdíl od běžných vlastností Text tato není typu string, Func<TItemType, string>!
		[Parameter] public RenderFragment<TItemType> ItemTemplate { get; set; }
		[Parameter] public string FooterText { get; set; }
		[Parameter] public RenderFragment FooterTemplate { get; set; }

		protected override RenderFragment GetHeaderTemplate() => RenderFragmentBuilder.CreateFrom(HeaderText, HeaderTemplate);

		protected override RenderFragment GetItemTemplate(TItemType item) => RenderFragmentBuilder.CreateFrom(ItemText?.Invoke(item), ItemTemplate?.Invoke(item));

		protected override RenderFragment GetFooterTemplate() => RenderFragmentBuilder.CreateFrom(FooterText, FooterTemplate);

	}
}
