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
	/// Raised after the transition to this item (the animation is finished).
	/// Is not raised for the initial rendering even if the item is not collapsed (no transition happened).
	/// </summary>
	[Parameter] public EventCallback<string> OnExpanded { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnExpanded"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnExpandedAsync(string expandedItemId) => OnExpanded.InvokeAsync(expandedItemId);

	/// <summary>
	/// Raised after the transition from this item (the animation is finished).
	/// Is not raised for the initial rendering even if the item is collapsed (no transition happened).
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

	private string currentId;
	private string idEffective;
	private bool lastKnownStateIsExpanded;
	private bool isInitialized;
	private HxCollapse collapseComponent;

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
		if ((currentId is not null) && (this.Id != currentId))
		{
			throw new InvalidOperationException("HxAccordionItem.Id cannot be changed. Use @key with same value as Id to help Blazor mapping the right components.");
		}
		else
		{
			currentId = this.Id;
		}

		idEffective = ParentAccordion.Id + "-" + this.Id;

		if (isInitialized)
		{
			if (!lastKnownStateIsExpanded && IsSetToBeExpanded())
			{
				await ExpandAsync();
			}
			else if (lastKnownStateIsExpanded && !IsSetToBeExpanded())
			{
				await CollapseAsync();
			}
		}
		else
		{
			lastKnownStateIsExpanded = IsSetToBeExpanded();
		}
	}

	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		isInitialized = true;
	}

	/// <summary>
	/// Expands the item.
	/// </summary>
	public async Task ExpandAsync()
	{
		await collapseComponent.ShowAsync();
		lastKnownStateIsExpanded = true;
	}

	/// <summary>
	/// Collapses the item.
	/// </summary>
	public async Task CollapseAsync()
	{
		await collapseComponent.HideAsync();
		lastKnownStateIsExpanded = false;
	}

	private async Task HandleCollapseShown()
	{
		lastKnownStateIsExpanded = true;

		await ParentAccordion.SetItemExpandedAsync(this.Id);

		await InvokeOnExpandedAsync(this.Id);
	}

	private async Task HandleCollapseHidden()
	{
		lastKnownStateIsExpanded = false;

		await ParentAccordion.SetItemCollapsedAsync(this.Id);

		await InvokeOnCollapsedAsync(this.Id);
	}

	private bool IsSetToBeExpanded() => ParentAccordion.ExpandedItemIds.Contains(this.Id);
}
