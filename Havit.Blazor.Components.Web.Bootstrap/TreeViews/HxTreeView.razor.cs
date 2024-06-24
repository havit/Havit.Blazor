namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Component to display a hierarchy data structure.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxTreeView">https://havit.blazor.eu/components/HxTreeView</see>
/// </summary>
/// <typeparam name="TItem">Type of tree data item.</typeparam>
public partial class HxTreeView<TItem> : ComponentBase
{
	/// <summary>
	/// Collection of hierarchical data to display.
	/// </summary>
	[Parameter, EditorRequired] public IEnumerable<TItem> Items { get; set; }

	/// <summary>
	/// Selected data item.
	/// </summary>		
	[Parameter] public TItem SelectedItem { get; set; }

	/// <summary>
	/// Event fired when the selected data item changes.
	/// </summary>		
	[Parameter] public EventCallback<TItem> SelectedItemChanged { get; set; }
	/// <summary>
	/// Event fired when an item is expanded.
	/// </summary>		
	[Parameter] public EventCallback<TItem> OnItemExpanded { get; set; }

	/// <summary>
	/// Event fired when an item is collapsed.
	/// </summary>		
	[Parameter] public EventCallback<TItem> OnItemCollapsed { get; set; }

	/// <summary>
	/// Triggers the <see cref="SelectedItemChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeSelectedDataItemChangedAsync(TItem selectedDataItem) => SelectedItemChanged.InvokeAsync(selectedDataItem);

	/// <summary>
	/// Selector to display the item title from the data item.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemTitleSelector { get; set; }

	/// <summary>
	/// Selector to display the icon from the data item.
	/// </summary>
	[Parameter] public Func<TItem, IconBase> ItemIconSelector { get; set; }

	/// <summary>
	/// Selector for the initial expansion state of the data item.<br/>
	/// The default state is <c>false</c> (collapsed).
	/// </summary>
	[Parameter] public Func<TItem, bool> ItemInitialExpandedSelector { get; set; }

	/// <summary>
	/// Item CSS class (same for all items).
	/// </summary>
	[Parameter] public string ItemCssClass { get; set; }

	/// <summary>
	/// Selector for the item CSS class.
	/// </summary>
	[Parameter] public Func<TItem, string> ItemCssClassSelector { get; set; }

	/// <summary>
	/// Selector to display the children collection for the current data item. The children collection should have the same type as the current item.
	/// </summary>
	[Parameter] public Func<TItem, IEnumerable<TItem>> ItemChildrenSelector { get; set; }

	/// <summary>
	/// Template for the item content.
	/// </summary>
	[Parameter] public RenderFragment<TItem> ItemTemplate { get; set; }

	/// <summary>
	/// Additional CSS class to be applied.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	private async Task HandleItemSelected(TItem newSelectedItem)
	{
		SelectedItem = newSelectedItem;
		await InvokeSelectedDataItemChangedAsync(SelectedItem);
	}
}
