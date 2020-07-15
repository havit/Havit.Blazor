using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap.Toasts
{
	/// <summary>
	/// Toast. After first render component never updates.
	/// </summary>
	public partial class HxToast : ComponentBase
	{
#pragma warning disable CS0649 // assigned by Blazor
		private ElementReference toastElement;
#pragma warning restore CS0649

		/// <summary>
		/// JS Runtime.
		/// </summary>
		[Inject] protected IJSRuntime JSRuntime { get; set; }

		/// <summary>
		/// Delay in miliseconds to automatically hide toast.
		/// </summary>
		[Parameter] public int? AutohideDelayMs { get; set; }

		/// <summary>
		/// Css class to render with toast.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Header text.
		/// </summary>
		[Parameter] public string HeaderText { get; set; }
		
		/// <summary>
		/// Header template.
		/// </summary>
		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		/// <summary>
		/// Content (body) text.
		/// </summary>
		[Parameter] public string ContentText { get; set; }

		/// <summary>
		/// Content (body) template.
		/// </summary>
		[Parameter] public RenderFragment ContentTemplate { get; set; }

		/// <summary>
		/// Indicates whether to show close button.
		/// </summary>
		[Parameter] public bool ShowCloseButton { get; set; } = true;

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			base.BuildRenderTree(builder);

			//<div @ref="toastElement" class="toast" data-delay="5000" data-autohide="true">
			//	<div class="toast-header">
			//		...
			//		<button type="button" class="ml-2 mb-1 close" data-dismiss="toast" aria-label="Close">
			//		</button>
			//	</div>
			//	<div class="toast-body">
			//		...
			//	</div>
			//</div>

			bool renderHeader = !String.IsNullOrEmpty(HeaderText) || (HeaderTemplate != null);
			bool renderContent = !String.IsNullOrEmpty(ContentText) || (ContentTemplate != null) || (ShowCloseButton && !renderHeader);

			builder.OpenElement(100, "div");
			builder.AddAttribute(101, "class", CssClassHelper.Combine("toast", CssClass));

			if (AutohideDelayMs != null)
			{
				builder.AddAttribute(102, "data-delay", AutohideDelayMs);
			}
			else
			{
				builder.AddAttribute(103, "data-autohide", "false");
			}
			builder.AddElementReferenceCapture(104, referenceCapture => toastElement = referenceCapture);

			if (renderHeader)
			{
				builder.OpenElement(200, "div");
				builder.AddAttribute(201, "class", "toast-header");
				builder.AddContent(202, HeaderText);
				builder.AddContent(203, HeaderTemplate);

				if (ShowCloseButton)
				{
					builder.OpenRegion(204);
					BuildRenderTree_CloseButton(builder);
					builder.CloseRegion();
				}
				builder.CloseElement(); // toast-header				

			}

			if (renderContent)
			{
				builder.OpenElement(300, "div");
				builder.AddAttribute(301, "class", "toast-body");
				builder.AddContent(302, ContentText);
				builder.AddContent(303, ContentTemplate);

				if (!renderHeader && ShowCloseButton)
				{
					builder.OpenRegion(304);
					BuildRenderTree_CloseButton(builder);
					builder.CloseRegion();
				}
				builder.CloseElement(); // toast-header
			}

			builder.CloseElement(); // toast
		}

		private void BuildRenderTree_CloseButton(RenderTreeBuilder builder)
		{
			builder.OpenElement(100, "button");
			builder.AddAttribute(101, "type", "button");
			builder.AddAttribute(102, "class", "close");
			builder.AddAttribute(103, "data-dismiss", "toast");
			
			builder.OpenElement(200, "span");
			builder.AddMarkupContent(201, "&times;");
			builder.CloseElement(); // span

			builder.CloseElement(); // button
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender)
			{
				// we need to manualy setup the toast.
				await JSRuntime.InvokeVoidAsync("hxToast_show", toastElement);
			}
		}
		
		protected override bool ShouldRender()
		{
			// never update content to avoid collision with javascript
			return false;
		}
	}
}
