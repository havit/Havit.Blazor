namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Single item for <see cref="HxAccordion"/>.
/// </summary>
public partial class HxAccordionItem : ComponentBase
{
	[CascadingParameter] protected HxAccordion ParentAccordion { get; set; }

	/// <summary>
	/// Clickable header (always visible).
	/// </summary>
	[Parameter] public RenderFragment HeaderTemplate { get; set; }

	/// <summary>
	/// Body (collapsible).
	/// </summary>
	[Parameter] public RenderFragment BodyTemplate { get; set; }

	/// <summary>
	/// ID of the item (<see cref="HxAccordion.ExpandedItemId"/>). (Gets generated GUID if not set.)
	/// </summary>
	[Parameter] public string Id { get; set; } = Guid.NewGuid().ToString("N");

	/// <summary>
	/// Raised when the item is expanded.
	/// Is not raised for the initial rendering even if the item is expanded.
	/// </summary>
	[Parameter] public EventCallback<string> OnExpanded { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnExpanded"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnExpandedAsync(string expandedItemId) => OnExpanded.InvokeAsync(expandedItemId);

	/// <summary>
	/// Raised when the item is collapsed.
	/// Is not raised for the initial rendering even if the item is collapsed.
	/// </summary>
	[Parameter] public EventCallback<string> OnCollapsed { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnCollapsed"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnCollapsedAsync(string collapsedItemId) => OnCollapsed.InvokeAsync(collapsedItemId);

	/// <summary>
	/// Additional CSS class(es) for the accordion item.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the accordion item header (<c>.accordion-header</c>).
	/// </summary>
	[Parameter] public string HeaderCssClass { get; set; }

	/// <summary>
	/// Additional CSS class(es) for the accordion item body (<c>.accordion-body</c>).
	/// </summary>
	[Parameter] public string BodyCssClass { get; set; }

	internal bool IsExpanded => _lastKnownStateIsExpanded;

	private string _currentId;
	private string _idEffective;
	private bool _lastKnownStateIsExpanded;
	private bool _isInitialized;

	protected override void OnInitialized()
	{
		base.OnInitialized();
		ParentAccordion?.RegisterItem(this);
	}

	protected override async Task OnParametersSetAsync()
	{
		await base.OnParametersSetAsync();

		Contract.Requires<InvalidOperationException>(ParentAccordion is not null, "<HxAccordionItem /> has to be placed inside <HxAccordion />.");

		// Issue #182
		// If the accordion items change dynamically, the instances of HxAccordionItem get completely different parameters.
		// HxAccordionItem tracks state (expanded/collapsed) and this state would apply to completely different items after such change.
		// We can either reset the internal state when the accordion items change, but as the component with completely different parameters
		// is considered being another item, we decided to force the user to set the @key for such dynamic items
		// (and force Blazor to create new components for such scenarios).
		// If this turns out to be a problem, we can also consider extracting the whole HxAccordionItem to HxAccordionItemInternal and set the @key for it.
		if ((_currentId is not null) && (Id != _currentId))
		{
			throw new InvalidOperationException($"[{GetType().Name}] Id cannot be changed. Use @key with the same value as Id to help Blazor map the right components.");
		}
		else
		{
			_currentId = Id;
		}

		_idEffective = ParentAccordion.Id + "-" + Id;

		if (_isInitialized)
		{
			if (!_lastKnownStateIsExpanded && IsSetToBeExpanded())
			{
				await ExpandAsync();
			}
			else if (_lastKnownStateIsExpanded && !IsSetToBeExpanded())
			{
				await CollapseAsync();
			}
		}
		else
		{
			_lastKnownStateIsExpanded = IsSetToBeExpanded();
		}
	}

	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		_isInitialized = true;
	}

	/// <summary>
	/// Expands the item.
	/// </summary>
	public async Task ExpandAsync()
	{
		if (_lastKnownStateIsExpanded)
		{
			return;
		}
		_lastKnownStateIsExpanded = true;

		await ParentAccordion.NotifyItemExpandedAsync(this);
		await InvokeOnExpandedAsync(Id);

		StateHasChanged();
	}

	/// <summary>
	/// Collapses the item.
	/// </summary>
	public Task CollapseAsync() => CollapseCoreAsync(invokeExpandedItemIdChanged: true);

	internal async Task CollapseCoreAsync(bool invokeExpandedItemIdChanged)
	{
		if (!_lastKnownStateIsExpanded)
		{
			return;
		}
		_lastKnownStateIsExpanded = false;

		await ParentAccordion.SetItemCollapsedAsync(Id, invokeExpandedItemIdChanged);
		await InvokeOnCollapsedAsync(Id);

		StateHasChanged();
	}

	private Task HandleHeaderClick()
	{
		// The native <details> toggle is prevented (@onclick:preventDefault on the <summary>) - Blazor state is the source of truth
		// (a re-render must not revert a browser-toggled open attribute, and exclusivity has to update the bound parameters).
		return _lastKnownStateIsExpanded ? CollapseAsync() : ExpandAsync();
	}

	private bool IsSetToBeExpanded() => ParentAccordion.ExpandedItemIds.Contains(Id);
}
