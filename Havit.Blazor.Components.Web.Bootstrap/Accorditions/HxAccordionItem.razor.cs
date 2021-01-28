using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO RH: OnExpanded na firstRender?
	// TODO RH: Animace na HxAccordition.ExpandedItemId změnu
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

		private string idEffective;
		private ElementReference collapseHtmlElement;
		private DotNetObjectReference<HxAccordionItem> dotnetObjectReference;
		private IJSObjectReference jsModule;

		public HxAccordionItem()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			Contract.Requires<InvalidOperationException>(ParentAccordition is not null, "<HxAccordionItem /> component has to be placed inside <HxAccordition />.");

			idEffective = ParentAccordition.Id + "-" + this.Id;
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender)
			{
				// we need to manualy setup the toast.
				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/hxaccordion.js");
				await jsModule.InvokeVoidAsync("create", collapseHtmlElement, dotnetObjectReference);
			}
		}

		/// <summary>
		/// Receive notification from javascript when item is shown.
		/// </summary>
		[JSInvokable("HxAccordionItem_HandleJsShown")]
		public async Task HandleJsShown()
		{
			// the shown-event gets raised as the "show" CSS class is added to the HTML element and the transition is completed
			// this covers both user-interaction (DOM state) and Blazor-interaction (HxAccordition.ExpandedItemId change)
			if (!IsExpanded())
			{
				await ParentAccordition.SetExpandedItemIdAsync(this.Id);
				await OnExpanded.InvokeAsync(this.Id);
			}
		}

		/// <summary>
		/// Receive notification from javascript when item is hidden.
		/// </summary>
		[JSInvokable("HxAccordionItem_HandleJsHidden")]
		public async Task HandleJsHidden()
		{
			// hidden-event usually comes AFTER the shown-event of the other HxAccorditionItem
			if (IsExpanded())
			{
				// if there has been no other HxAccorditionItem set as expanded yet, clear the selection
				await ParentAccordition.SetExpandedItemIdAsync(null);
			}
			await OnCollapsed.InvokeAsync(this.Id);
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

		private bool IsExpanded()
		{
			return this.Id == ParentAccordition.ExpandedItemId;
		}
	}
}
