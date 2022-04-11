using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

public partial class HxGridEmptyDataTemplateDefaultContent
{
	[Parameter] public RenderFragment ChildContent { get; set; }

	[Inject] protected IStringLocalizer<HxGrid> HxGridLocalizer { get; set; }
}
