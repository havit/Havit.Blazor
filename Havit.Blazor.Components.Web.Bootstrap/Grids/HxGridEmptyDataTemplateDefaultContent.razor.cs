using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Default placeholder shown by <see cref="HxGrid{TItem}"/> when there is no data to display.
/// </summary>
public partial class HxGridEmptyDataTemplateDefaultContent
{
	[Parameter] public RenderFragment ChildContent { get; set; }

	[Inject] protected IStringLocalizer<HxGrid> HxGridLocalizer { get; set; }
}
