namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxTreeViewItemInternal<TItem> : ComponentBase
{
	[Parameter] public TItem Item { get; set; }
	[Parameter] public EventCallback<TItem> OnItemSelected { get; set; }
	[Parameter] public EventCallback<TItem> OnItemExpanded { get; set; }
	[Parameter] public EventCallback<TItem> OnItemCollapsed { get; set; }

	[Parameter] public bool? IsExpanded { get; set; }

	[Parameter] public Func<TItem, string> TitleSelector { get; set; }
	[Parameter] public Func<TItem, IconBase> IconSelector { get; set; }
	[Parameter] public Func<TItem, bool> InitialExpandedSelector { get; set; }
	[Parameter] public string CssClass { get; set; }
	[Parameter] public Func<TItem, string> CssClassSelector { get; set; }
	[Parameter] public Func<TItem, IEnumerable<TItem>> ChildrenSelector { get; set; }
	[Parameter] public int Level { get; set; }
	[Parameter] public RenderFragment<TItem> ContentTemplate { get; set; }

	[CascadingParameter] protected HxTreeView<TItem> TreeViewContainer { get; set; }

	private string _collapseId = "hx" + Guid.NewGuid().ToString("N");
	private bool _initiallyExpanded;

	protected override void OnInitialized()
	{
		_initiallyExpanded = InitialExpandedSelector?.Invoke(Item) ?? false;
		if (_initiallyExpanded)
		{
			IsExpanded = true;
		}
	}

	private async Task HandleItemClicked()
	{
		await OnItemSelected.InvokeAsync(Item);
	}

	private async Task HandleCollapseHiddenAsync()
	{
		IsExpanded = false;
		await OnItemCollapsed.InvokeAsync(Item);
	}

	private async Task HandleCollapseShownAsync()
	{
		IsExpanded = true;
		await OnItemExpanded.InvokeAsync(Item);
	}
}