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
	[Parameter] public bool ExpandOnSelection { get; set; }

	[CascadingParameter] protected HxTreeView<TItem> TreeViewContainer { get; set; }

	private string _collapseId = "hx" + Guid.NewGuid().ToString("N");
	private bool _initiallyExpanded;
	private bool _previouslySelected;
	private HxCollapse _collapseReference;

	protected override void OnInitialized()
	{
		_initiallyExpanded = InitialExpandedSelector?.Invoke(Item) ?? false;
		if (_initiallyExpanded)
		{
			IsExpanded = true;
		}
	}

	protected override async Task OnParametersSetAsync()
	{
		bool hasChildren = ChildrenSelector(Item)?.Any() ?? false;
		if (!hasChildren)
		{
			_collapseReference = null;
		}

		bool isSelected = Item.Equals(TreeViewContainer.SelectedItem);
		if (ExpandOnSelection && isSelected && !_previouslySelected)
		{
			await ExpandAsync();
		}
		_previouslySelected = isSelected;
	}

	private async Task HandleItemClicked()
	{
		if (ExpandOnSelection)
		{
			await ExpandAsync();
			// prevents duplicate expansion from OnParametersSetAsync when the selection change rerenders this item
			_previouslySelected = true;
		}
		await OnItemSelected.InvokeAsync(Item);
	}

	private async Task ExpandAsync()
	{
		if (!(ChildrenSelector(Item)?.Any() ?? false))
		{
			// no children to expand (and _collapseReference may be a stale reference to an already removed HxCollapse)
			return;
		}

		if (_collapseReference is not null)
		{
			await _collapseReference.ShowAsync();
		}
		else
		{
			// HxCollapse not rendered yet (initial selection) - let it render expanded
			IsExpanded = true;
		}
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