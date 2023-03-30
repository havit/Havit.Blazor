namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.2/components/accordion/">Bootstrap accordion</see> component.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxAccordion">https://havit.blazor.eu/components/HxAccordion</see>
/// </summary>
public partial class HxAccordion
{
	/// <summary>
	/// Additional CSS classes for the accordion container.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// IDs of the expanded items (if <see cref="StayOpen"/> is <code>true</code>, several items can be expanded).
	/// Do not use constant value as it resets the accordion on every roundtrip. Use <see cref="InitialExpandedItemIds"/> to set the initial state.
	/// </summary>
	[Parameter] public List<string> ExpandedItemIds { get; set; } = new List<string>();
	[Parameter] public EventCallback<List<string>> ExpandedItemIdsChanged { get; set; }
	protected virtual Task InvokeExpandedItemIdsChangedAsync(List<string> newExpandedItemIds) => ExpandedItemIdsChanged.InvokeAsync(newExpandedItemIds);

	/// <summary>
	/// ID of the single expanded item (if <see cref="StayOpen"/> is <code>true</code>, use <see cref="ExpandedItemIds" />, several items can be expanded).
	/// Do not use constant value as it reverts the accordion to that item on every roundtrip. Use <see cref="InitialExpandedItemId"/> to set the initial state.
	/// </summary>
	[Parameter]
	public string ExpandedItemId
	{
		get
		{
			// we do not want the component to fail, so we return first expanded item if there are more (StayOpen)
			return ExpandedItemIds.FirstOrDefault();
		}
		set
		{
			// setting the value forces all the items to collapse except the one set
			ExpandedItemIds.Clear();
			if (value is not null)
			{
				ExpandedItemIds.Add(value);
			}
		}
	}
	[Parameter] public EventCallback<string> ExpandedItemIdChanged { get; set; }
	protected virtual Task InvokeExpandedItemIdChangedAsync(string newExpandedItemId) => ExpandedItemIdChanged.InvokeAsync(newExpandedItemId);

	/// <summary>
	/// Set to <c>true</c> to make accordion items stay open when another item is opened.
	/// Default is <c>false</c>, openning another item collapses current item.
	/// </summary>
	[Parameter] public bool StayOpen { get; set; }

	/// <summary>
	/// ID of the item you want to expand at the very beginning (overwrites <see cref="ExpandedItemId"/> if set).
	/// </summary>
	[Parameter] public string InitialExpandedItemId { get; set; }

	/// <summary>
	/// IDs of the item you want to expand at the very beginning (overwrites <see cref="ExpandedItemIds"/> if set).
	/// </summary>
	[Parameter] public List<string> InitialExpandedItemIds { get; set; } = new();

	[Parameter] public RenderFragment ChildContent { get; set; }

	protected internal string Id { get; set; } = "hx" + Guid.NewGuid().ToString("N");

	protected override void OnInitialized()
	{
		if (StayOpen && InitialExpandedItemIds.Count > 0)
		{
			ExpandedItemIds.AddRange(InitialExpandedItemIds);
		}
		else if (!StayOpen && !String.IsNullOrWhiteSpace(InitialExpandedItemId))
		{
			ExpandedItemId = InitialExpandedItemId;
		}
	}

	internal async Task SetItemExpandedAsync(string itemId)
	{
		if (!ExpandedItemIds.Contains(itemId))
		{
			ExpandedItemIds.Add(itemId);
			if (!StayOpen)
			{
				await InvokeExpandedItemIdChangedAsync(itemId);
			}
			await InvokeExpandedItemIdsChangedAsync(ExpandedItemIds);
		}
	}

	internal async Task SetItemCollapsedAsync(string itemId)
	{
		if (ExpandedItemIds.Contains(itemId))
		{
			ExpandedItemIds.Remove(itemId);
			if (!StayOpen)
			{
				await InvokeExpandedItemIdChangedAsync(null);
			}
			await InvokeExpandedItemIdsChangedAsync(ExpandedItemIds);
		}
	}
}
