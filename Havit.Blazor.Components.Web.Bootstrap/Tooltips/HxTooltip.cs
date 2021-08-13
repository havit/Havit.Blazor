using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using static System.Net.WebRequestMethods;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <a href="https://getbootstrap.com/docs/5.0/components/tooltips/">Bootstrap Tooltip</a> component.
	/// Rendered as a span (see example in <a href="https://getbootstrap.com/docs/5.0/components/tooltips/#disabled-elements">Disabled elements</a> in the Bootstrap tooltip documentation).
	/// </summary>
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
		private bool shouldRenderSpan;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// Once the span is rendered it does not disapper to enable spanElement to be used at OnAfterRender to safely remove a tooltip.
			// It is not a common situation to remove a tooltip.
			shouldRenderSpan |= !String.IsNullOrEmpty(Text);
			if (shouldRenderSpan)
			{
				builder.OpenElement(1, "span");
				builder.AddAttribute(2, "class", "d-inline-block");
				builder.AddAttribute(3, "data-bs-container", "body");
				builder.AddAttribute(4, "data-bs-trigger", "hover");
				builder.AddAttribute(5, "data-bs-placement", Placement.ToString().ToLower());
				builder.AddAttribute(6, "title", Text);
				builder.AddElementReferenceCapture(7, element => spanElement = element);
			}

			builder.AddContent(8, ChildContent);

			if (shouldRenderSpan)
			{
				builder.CloseElement();
			}
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (lastText != Text)
			{
				// carefully, lastText can be null but Text empty string

				bool shouldCreateOrUpdateTooltip = !String.IsNullOrEmpty(Text); // everytime the Text changes we need to update tooltip
				bool shouldDestroyTooltip = String.IsNullOrEmpty(Text) && !String.IsNullOrEmpty(lastText); // when there is no tooltip anymore
				lastText = Text;

				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/" + nameof(HxTooltip) + ".js");

				if (shouldCreateOrUpdateTooltip)
				{
					await jsModule.InvokeVoidAsync("createOrUpdate", spanElement);
				}

				if (shouldDestroyTooltip)
				{
					await jsModule.InvokeVoidAsync("destroy", spanElement);
				}
			}

		}

		public async ValueTask DisposeAsync()
		{
			if (jsModule != null)
			{
				if (!String.IsNullOrEmpty(Text))
				{
					await jsModule.InvokeVoidAsync("destroy", spanElement);
				}
				await jsModule.DisposeAsync();
				jsModule = null;
			}
		}
	}
}
