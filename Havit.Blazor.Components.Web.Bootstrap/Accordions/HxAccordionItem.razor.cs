using Havit.Diagnostics.Contracts;
using Microsoft.Extensions.Logging;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Single item for <see cref="HxAccordion"/>.
	/// </summary>
	public partial class HxAccordionItem : ComponentBase //, IAsyncDisposable
	{
		[CascadingParameter] protected HxAccordion ParentAccordition { get; set; }

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

		//[Inject] protected IJSRuntime JSRuntime { get; set; }
		[Inject] protected ILogger<HxAccordionItem> Logger { get; set; }

		private string idEffective;
		//private ElementReference collapseHtmlElement;
		//private DotNetObjectReference<HxAccordionItem> dotnetObjectReference;
		//private IJSObjectReference jsModule;
		private bool lastKnownStateIsExpanded;
		private bool isInitialized;
		private bool isInTransition;
		//private bool disposed;
		private HxCollapse collapseComponent;
		private string currentId;

		//public HxAccordionItem()
		//{
		//	dotnetObjectReference = DotNetObjectReference.Create(this);
		//}

		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			Contract.Requires<InvalidOperationException>(ParentAccordition is not null, "<HxAccordionItem /> has to be placed inside <HxAccordition />.");

			// Issue #182
			// If the accordion items change dynamically, the instances of HxAccordionItem get completely different parameters.
			// HxAccordionItem tracks state (expanded/collapsed) and this state would apply to completely different items after such change.
			// We can either reset the internal state when the accordion items change, but as the component with completely different parameters
			// is considered being another item, we decided to force the user to set the @key for such dynamic items
			// (and force Blazor to create new components for such scenarios).
			if ((currentId is not null) && (this.Id != currentId))
			{
				throw new InvalidOperationException("HxAccordionItem.Id cannot be changed. Use @key with same value as Id to force Blazor recreate the component if Id changes.");
			}
			else
			{
				currentId = this.Id;
			}

			idEffective = ParentAccordition.Id + "-" + this.Id;

			if (isInitialized)
			{
				if (!isInTransition)
				{
					if (!lastKnownStateIsExpanded && IsSetToBeExpanded())
					{
						await ExpandAsync();
					}
					else if (lastKnownStateIsExpanded && String.IsNullOrEmpty(ParentAccordition.ExpandedItemId))
					{
						await CollapseAsync();
					}
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

			//if (firstRender)
			//{
			//	await EnsureJsModuleAsync();
			//	if (disposed)
			//	{
			//		return;
			//	}
			//	await jsModule.InvokeVoidAsync("create", collapseHtmlElement, dotnetObjectReference, "#" + ParentAccordition.Id);
			//}

			isInitialized = true;
		}

		/// <summary>
		/// Expands the item.
		/// </summary>
		public async Task ExpandAsync()
		{
			isInTransition = true;
			await collapseComponent.ShowAsync();
			//await EnsureJsModuleAsync();
			//await jsModule.InvokeVoidAsync("show", collapseHtmlElement);
		}

		/// <summary>
		/// Collapses the item.
		/// </summary>
		public async Task CollapseAsync()
		{
			isInTransition = true;
			await collapseComponent.HideAsync();
			//await EnsureJsModuleAsync();
			//await jsModule.InvokeVoidAsync("hide", collapseHtmlElement);
		}

		/// <summary>
		/// Receives notification from javascript when item is shown.
		/// </summary>
		/// <remarks>
		/// the shown-event gets raised as the "show" CSS class is added to the HTML element and the transition is completed
		/// this covers both user-interaction (DOM state) and Blazor-interaction (HxAccordition.ExpandedItemId change)
		/// </remarks>
		//[JSInvokable("HxAccordionItem_HandleJsShown")]
		//public async Task HandleJsShown()
		//{
		//	Logger.LogDebug($"HandleJsShown[{idEffective}]");

		//	lastKnownStateIsExpanded = true;
		//	isInTransition = false;

		//	if (!IsSetToBeExpanded())
		//	{
		//		await ParentAccordition.SetExpandedItemIdAsync(this.Id);
		//	}

		//	await InvokeOnExpandedAsync(this.Id);

		//	StateHasChanged();
		//}

		private async Task HandleCollapseShown()
		{
			lastKnownStateIsExpanded = true;
			isInTransition = false;

			if (!IsSetToBeExpanded())
			{
				await ParentAccordition.SetExpandedItemIdAsync(this.Id);
			}

			await InvokeOnExpandedAsync(this.Id);
		}

		private async Task HandleCollapseHidden()
		{
			lastKnownStateIsExpanded = false;
			isInTransition = false;

			// hidden-event usually comes AFTER the shown-event of the other HxAccorditionItem
			if (IsSetToBeExpanded())
			{
				// if there has been no other HxAccorditionItem set as expanded yet, clear the selection
				await ParentAccordition.SetExpandedItemIdAsync(null);
			}
			await InvokeOnCollapsedAsync(this.Id);
		}

		/// <summary>
		/// Receives notification from javascript when item is hidden.
		/// </summary>
		//[JSInvokable("HxAccordionItem_HandleJsHidden")]
		//public async Task HandleJsHidden()
		//{
		//	Logger.LogDebug($"HandleJsHidden[{idEffective}]");

		//	lastKnownStateIsExpanded = false;
		//	isInTransition = false;

		//	// hidden-event usually comes AFTER the shown-event of the other HxAccorditionItem
		//	if (IsSetToBeExpanded())
		//	{
		//		// if there has been no other HxAccorditionItem set as expanded yet, clear the selection
		//		await ParentAccordition.SetExpandedItemIdAsync(null);
		//	}
		//	await InvokeOnCollapsedAsync(this.Id);

		//	StateHasChanged();
		//}

		private bool IsSetToBeExpanded()
		{
			return (this.Id == ParentAccordition.ExpandedItemId);
		}

		//private async Task EnsureJsModuleAsync()
		//{
		//	jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxAccordion));
		//}

		/// <inheritdoc/>

		//public async ValueTask DisposeAsync()
		//{
		//	await DisposeAsyncCore();

		//	//Dispose(disposing: false);
		//}

		//protected virtual async ValueTask DisposeAsyncCore()
		//{
		//	disposed = true;

		//	if (jsModule != null)
		//	{
		//		await jsModule.InvokeVoidAsync("dispose", collapseHtmlElement);
		//		await jsModule.DisposeAsync();
		//	}

		//	dotnetObjectReference.Dispose();
		//}
	}
}
