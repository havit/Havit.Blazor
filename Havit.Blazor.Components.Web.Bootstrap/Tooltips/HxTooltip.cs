using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class HxTooltip : ComponentBase, IAsyncDisposable
	{
		/// <summary>
		/// Tooltip text to display above the content.
		/// </summary>
		[Parameter] public string Text { get; set; }

		/// <summary>
		/// Tooltip placement.
		/// </summary>
		[Parameter] public TooltipPlacement Placement { get; set; }

		/// <summary>
		/// Child content to wrap over HxTooltip.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		[Inject] public IJSRuntime JSRuntime { get; set; }

		private IJSObjectReference jsModule;
		private ElementReference spanElement;
		private string lastText;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenElement(1, "span");
			builder.AddAttribute(2, "class", "d-inline-block");
			builder.AddAttribute(3, "data-bs-container", "body");
			builder.AddAttribute(4, "data-bs-trigger", "hover");
			builder.AddAttribute(5, "data-bs-placement", Placement.ToString().ToLower());
			builder.AddAttribute(6, "title", Text);
			builder.AddElementReferenceCapture(7, element => spanElement = element);
			builder.AddContent(8, ChildContent);
			builder.CloseElement();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (lastText != Text)
			{
				lastText = Text;

				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/hxtooltip.js");
				await jsModule.InvokeVoidAsync("createOrUpdate", spanElement); // we are handling the situation when the Text changes - we need to dispose the bootstrap tooltip first
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (jsModule != null)
			{
				await jsModule.InvokeVoidAsync("dispose", spanElement);
				await jsModule.DisposeAsync();
				jsModule = null;
			}
		}
	}
}
