using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	/// <summary>
	/// Common implementation for <see cref="HxTooltip"/> and <see cref="HxPopover"/>.
	/// </summary>
	/// <remarks>
	/// We do not want HxPopover to derive from HxTooltip directly as we want
	/// to keep the API consistent, e.g. HxPopover.Placement = PopoverPlacement.Auto, not TooltipPlacement.Auto or anything else.
	/// </remarks>
	public abstract class HxTooltipInternalBase : ComponentBase, IAsyncDisposable
	{
		protected string TitleInternal { get; set; }
		protected string ContentInternal { get; set; }
		protected TooltipPlacement PlacementInternal { get; set; }
		protected TooltipTrigger TriggerInternal { get; set; }

		/// <summary>
		/// Allows you to insert HTML. If <c>false</c>, <c>innerText</c> property will be used to insert content into the DOM.
		/// Use text if you're worried about XSS attacks.
		/// </summary>
		[Parameter] public bool Html { get; set; }

		/// <summary>
		/// Custom CSS class to add.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Custom CSS class to render with the <c>span</c> wrapper of the child-content.
		/// </summary>
		[Parameter] public string WrapperCssClass { get; set; }

		/// <summary>
		/// Child content to wrap.
		/// </summary>
		[Parameter] public RenderFragment ChildContent { get; set; }

		[Inject] public IJSRuntime JSRuntime { get; set; }

		protected abstract string JsModuleName { get; }
		protected abstract string DataBsToggle { get; }

		private IJSObjectReference jsModule;
		private ElementReference spanElement;
		private string lastTitle;
		private string lastContent;
		private bool shouldRenderSpan;

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			// Once the span is rendered it does not disapper to enable spanElement to be used at OnAfterRender to safely remove a tooltip/popover.
			// It is not a common situation to remove a tooltip/popover.
			shouldRenderSpan |= !String.IsNullOrEmpty(TitleInternal)
								|| !String.IsNullOrWhiteSpace(this.WrapperCssClass)
								|| !String.IsNullOrWhiteSpace(this.ContentInternal);
			if (shouldRenderSpan)
			{
				builder.OpenElement(1, "span");
				builder.AddAttribute(2, "class", CssClassHelper.Combine("d-inline-block", WrapperCssClass));
				builder.AddAttribute(3, "data-bs-container", "body");
				builder.AddAttribute(4, "data-bs-trigger", GetTriggers());
				builder.AddAttribute(5, "data-bs-placement", PlacementInternal.ToString().ToLower());
				builder.AddAttribute(6, "data-bs-custom-class", CssClass);
				builder.AddAttribute(7, "title", TitleInternal);
				if (!String.IsNullOrWhiteSpace(ContentInternal))
				{
					// used only by HxPopover
					builder.AddAttribute(8, "data-bs-content", ContentInternal);
				}
				if (Html)
				{
					builder.AddAttribute(9, "data-bs-html", "true");
				}
				builder.AddAttribute(10, "data-bs-toggle", DataBsToggle);
				builder.AddElementReferenceCapture(11, element => spanElement = element);
			}

			builder.AddContent(20, ChildContent);

			if (shouldRenderSpan)
			{
				builder.CloseElement();
			}
		}

		protected string GetTriggers()
		{
			string result = null;
			foreach (var flag in Enum.GetValues<TooltipTrigger>())
			{
				if (this.TriggerInternal.HasFlag(flag))
				{
					result = result + " " + flag.ToString().ToLower();
				}
			}
			return result?.Trim();
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if ((lastTitle != TitleInternal) || (lastContent != ContentInternal))
			{
				// carefully, lastText can be null but Text empty string

				bool shouldCreateOrUpdateTooltip = !String.IsNullOrEmpty(TitleInternal) || !String.IsNullOrEmpty(ContentInternal);
				bool shouldDestroyTooltip = String.IsNullOrEmpty(TitleInternal)
					&& String.IsNullOrEmpty(ContentInternal)
					&& (!String.IsNullOrEmpty(lastTitle) || !String.IsNullOrEmpty(lastContent));

				lastTitle = TitleInternal;
				lastContent = ContentInternal;

				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/" + JsModuleName + ".js");

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
				if (!String.IsNullOrEmpty(TitleInternal) || !String.IsNullOrEmpty(ContentInternal))
				{
					await jsModule.InvokeVoidAsync("destroy", spanElement);
				}
				await jsModule.DisposeAsync();
				jsModule = null;
			}
		}
	}
}
