using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public interface IHxGridColumn<TItemType>
	{   // TODO: CssClass? (možnost zarovnání, atp.)
		RenderFragment GetHeaderTemplate();
		RenderFragment GetItemTemplate(TItemType item);
		RenderFragment GetFooterTemplate();
	}
}
