using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO RH: OnExpanded na firstRender?
	// TODO RH: OnExpanded+OnCollapsed na nastavení ExpandedItemId
	// TODO RH: Animace na HxAccordition.ExpandedItemId změnu (ponecháme known-issue, jinak přechod na Collapse-JS)
	// TODO RH: Dokumentace
	public partial class HxAccordionItem : ComponentBase, IAsyncDisposable
	{
		[CascadingParameter] protected HxAccordion ParentAccordition { get; set; }

		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		[Parameter] public RenderFragment BodyTemplate { get; set; }

		[Parameter] public string Id { get; set; } = Guid.NewGuid().ToString("N");

		[Parameter] public EventCallback<string> OnExpanded { get; set; }

		[Parameter] public EventCallback<string> OnCollapsed { get; set; }

		[Inject] protected IJSRuntime JSRuntime { get; set; }
		[Inject] protected ILogger<HxAccordionItem> Logger { get; set; }

		private string idEffective;
		private ElementReference collapseHtmlElement;
		private DotNetObjectReference<HxAccordionItem> dotnetObjectReference;
		private IJSObjectReference jsModule;
		private bool lastKnownStateIsExpanded;
		private bool isInitialized;
		private bool isInTransition;

		public HxAccordionItem()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		protected override async Task OnParametersSetAsync()
		{
			await base.OnParametersSetAsync();

			Contract.Requires<InvalidOperationException>(ParentAccordition is not null, "<HxAccordionItem /> component has to be placed inside <HxAccordition />.");

			idEffective = ParentAccordition.Id + "-" + this.Id;

			if (isInitialized)
			{
				if (!isInTransition)
				{
					if (!lastKnownStateIsExpanded && IsSetExpanded())
					{
						Logger.LogDebug($"OnParametersSetAsync_Expand[{idEffective}]");
						await Expand();
					}
					else if (lastKnownStateIsExpanded && String.IsNullOrEmpty(ParentAccordition.ExpandedItemId))
					{
						Logger.LogDebug($"OnParametersSetAsync_Collapse[{idEffective}]");
						await Collapse();
					}
				}
			}
			else
			{
				lastKnownStateIsExpanded = IsSetExpanded();
			}
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender)
			{
				Logger.LogDebug($"OnAfterRenderAsync_create[{idEffective}]");
				await EnsureJsModuleAsync();
				await jsModule.InvokeVoidAsync("create", collapseHtmlElement, dotnetObjectReference, "#" + ParentAccordition.Id);
			}

			isInitialized = true;
		}
		public async Task Expand()
		{
			isInTransition = true;
			await EnsureJsModuleAsync();
			await jsModule.InvokeVoidAsync("show", collapseHtmlElement);
		}

		public async Task Collapse()
		{
			isInTransition = true;
			await EnsureJsModuleAsync();
			await jsModule.InvokeVoidAsync("hide", collapseHtmlElement);
		}

		/// <summary>
		/// Receive notification from javascript when item is shown.
		/// </summary>
		/// <remarks>
		/// the shown-event gets raised as the "show" CSS class is added to the HTML element and the transition is completed
		/// this covers both user-interaction (DOM state) and Blazor-interaction (HxAccordition.ExpandedItemId change)
		/// </remarks>
		[JSInvokable("HxAccordionItem_HandleJsShown")]
		public async Task HandleJsShown()
		{
			Logger.LogDebug($"HandleJsShown[{idEffective}]");

			lastKnownStateIsExpanded = true;
			isInTransition = false;

			if (!IsSetExpanded())
			{
				await ParentAccordition.SetExpandedItemIdAsync(this.Id);
			}

			await OnExpanded.InvokeAsync(this.Id);

			StateHasChanged();
		}

		/// <summary>
		/// Receive notification from javascript when item is hidden.
		/// </summary>
		[JSInvokable("HxAccordionItem_HandleJsHidden")]
		public async Task HandleJsHidden()
		{
			Logger.LogDebug($"HandleJsHidden[{idEffective}]");

			lastKnownStateIsExpanded = false;
			isInTransition = false;

			// hidden-event usually comes AFTER the shown-event of the other HxAccorditionItem
			if (IsSetExpanded())
			{
				// if there has been no other HxAccorditionItem set as expanded yet, clear the selection
				await ParentAccordition.SetExpandedItemIdAsync(null);
			}
			await OnCollapsed.InvokeAsync(this.Id);

			StateHasChanged();
		}

		/// <inheritdoc/>
		public async ValueTask DisposeAsync()
		{
			if (jsModule != null)
			{
				await jsModule.InvokeVoidAsync("dispose", collapseHtmlElement);
				await jsModule.DisposeAsync();
			}

			dotnetObjectReference.Dispose();
		}

		private bool IsSetExpanded()
		{
			return this.Id == ParentAccordition.ExpandedItemId;
		}

		private async Task EnsureJsModuleAsync()
		{
			jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/hxaccordion.js");
		}

	}
}
