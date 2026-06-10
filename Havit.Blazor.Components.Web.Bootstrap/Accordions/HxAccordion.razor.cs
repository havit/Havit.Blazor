namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/accordion/">Bootstrap accordion</see> component (built on the native <c>&lt;details&gt;</c>/<c>&lt;summary&gt;</c> disclosure elements, no JavaScript needed).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxAccordion">https://havit.blazor.eu/components/HxAccordion</see>
/// </summary>
public partial class HxAccordion
{
	/// <summary>
	/// Additional CSS classes for the accordion container.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Theme color of the accordion (renders the <c>theme-*</c> class applied to open items).
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }

	/// <summary>
	/// IDs of the expanded items (if <see cref="StayOpen"/> is <code>true</code>, several items can be expanded).
	/// Do not use a constant value as it resets the accordion on every roundtrip. Use <see cref="InitialExpandedItemIds"/> to set the initial state.
	/// When <see cref="StayOpen"/> is <c>true</c>, this parameter must be used to manage expanded items, and the <see cref="ExpandedItemIdsChanged"/> event callback will be invoked for changes.
	/// </summary>
	[Parameter] public List<string> ExpandedItemIds { get; set; } = new List<string>();
	[Parameter] public EventCallback<List<string>> ExpandedItemIdsChanged { get; set; }
	protected virtual Task InvokeExpandedItemIdsChangedAsync(List<string> newExpandedItemIds) => ExpandedItemIdsChanged.InvokeAsync(newExpandedItemIds);

#pragma warning disable BL0007 // Component parameter 'Havit.Blazor.Components.Web.Bootstrap.HxAccordion.ExpandedItemId' should be auto property
	/// <summary>
	/// ID of the single expanded item. If <see cref="StayOpen"/> is <c>true</c>, this parameter is ignored, and you must use <see cref="ExpandedItemIds"/> instead.
	/// When <see cref="StayOpen"/> is <c>true</c>, the <see cref="ExpandedItemIdChanged"/> event callback will not be invoked.
	/// Do not use a constant value as it reverts the accordion to that item on every roundtrip. Use <see cref="InitialExpandedItemId"/> to set the initial state.
	/// </summary>
	[Parameter]
	public string ExpandedItemId
	{
		get
		{
			// We do not want the component to fail, so we return the first expanded item if there are more (StayOpen).
			return ExpandedItemIds.FirstOrDefault();
		}
		set
		{
			// Setting the value forces all the items to collapse except the one set.
			ExpandedItemIds.Clear();
			if (value is not null)
			{
				ExpandedItemIds.Add(value);
			}
		}
	}
#pragma warning restore BL0007 // Component parameter 'Havit.Blazor.Components.Web.Bootstrap.HxAccordion.ExpandedItemId' should be auto property
	[Parameter] public EventCallback<string> ExpandedItemIdChanged { get; set; }
	protected virtual Task InvokeExpandedItemIdChangedAsync(string newExpandedItemId) => ExpandedItemIdChanged.InvokeAsync(newExpandedItemId);

	/// <summary>
	/// Set to <c>true</c> to make accordion items stay open when another item is opened.
	/// The default is <c>false</c>, opening another item collapses the current item.
	/// When <c>true</c>, <see cref="ExpandedItemIds"/> must be used to manage expanded items.
	/// </summary>
	[Parameter] public bool StayOpen { get; set; }

	/// <summary>
	/// ID of the item you want to expand at the very beginning (overwrites <see cref="ExpandedItemId"/> if set).
	/// </summary>
	[Parameter] public string InitialExpandedItemId { get; set; }

	/// <summary>
	/// IDs of the items you want to expand at the very beginning (overwrites <see cref="ExpandedItemIds"/> if set).
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

	private readonly List<HxAccordionItem> _items = new();

	internal void RegisterItem(HxAccordionItem item) => _items.Add(item);

	/// <summary>
	/// Called by an expanding <see cref="HxAccordionItem"/>. For exclusive accordions (<see cref="StayOpen"/> is <c>false</c>),
	/// collapses all other expanded items first (replicates the native <c>name</c>-attribute exclusivity Blazor-side,
	/// as the open state is managed by Blazor).
	/// </summary>
	internal async Task NotifyItemExpandedAsync(HxAccordionItem expandedItem)
	{
		if (!StayOpen)
		{
			foreach (HxAccordionItem otherItem in _items.Where(i => !ReferenceEquals(i, expandedItem) && i.IsExpanded).ToList())
			{
				await otherItem.CollapseCoreAsync(invokeExpandedItemIdChanged: false);
			}
		}
		await SetItemExpandedAsync(expandedItem.Id);
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

	internal async Task SetItemCollapsedAsync(string itemId, bool invokeExpandedItemIdChanged = true)
	{
		if (ExpandedItemIds.Contains(itemId))
		{
			ExpandedItemIds.Remove(itemId);
			if (!StayOpen && invokeExpandedItemIdChanged)
			{
				await InvokeExpandedItemIdChangedAsync(null);
			}
			await InvokeExpandedItemIdsChangedAsync(ExpandedItemIds);
		}
	}
}
