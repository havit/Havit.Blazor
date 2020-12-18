using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Toast. Not intented to be used in user code, use <see cref="HxMessenger"/>.
	/// After first render component never updates.
	/// </summary>
	public partial class HxToast : ComponentBase, IAsyncDisposable
	{
#pragma warning disable CS0649 // assigned by Blazor
		private ElementReference toastElement;
#pragma warning restore CS0649

		private DotNetObjectReference<HxToast> dotnetObjectReference;

		/// <summary>
		/// JS Runtime.
		/// </summary>
		[Inject] protected IJSRuntime JSRuntime { get; set; }

		/// <summary>
		/// Delay in miliseconds to automatically hide toast.
		/// </summary>
		[Parameter] public int? AutohideDelay { get; set; }

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
		/// Content (body) icon.
		/// </summary>
		[Parameter] public IconBase ContentIcon { get; set; }

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

		/// <summary>
		/// Fires when toast is hidden (button or autohide).
		/// </summary>
		[Parameter] public EventCallback ToastHidden { get; set; }

		public HxToast()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			base.BuildRenderTree(builder);

			bool renderHeader = !String.IsNullOrEmpty(HeaderText) || (HeaderTemplate != null);
			bool renderContent = !String.IsNullOrEmpty(ContentText) || (ContentTemplate != null) || (ShowCloseButton && !renderHeader);

			builder.OpenElement(100, "div");
			builder.AddAttribute(101, "role", "alert");
			builder.AddAttribute(102, "aria-live", "assertive");
			builder.AddAttribute(103, "aria-atomic", "true");
			builder.AddAttribute(104, "class", CssClassHelper.Combine("toast", CssClass));

			if (AutohideDelay != null)
			{
				builder.AddAttribute(110, "data-bs-delay", AutohideDelay);
			}
			else
			{
				builder.AddAttribute(111, "data-bs-autohide", "false");
			}
			builder.AddElementReferenceCapture(120, referenceCapture => toastElement = referenceCapture);

			if (renderHeader)
			{
				builder.OpenElement(200, "div");
				builder.AddAttribute(201, "class", "toast-header");
				builder.OpenElement(202, "strong");
				builder.AddContent(203, HeaderText);
				builder.AddContent(204, HeaderTemplate);
				builder.CloseElement(); // strong

				if (ShowCloseButton)
				{
					builder.OpenRegion(210);
					BuildRenderTree_CloseButton(builder);
					builder.CloseRegion();
				}
				builder.CloseElement(); // toast-header				

			}

			if (renderContent)
			{
				builder.OpenElement(300, "div");
				builder.AddAttribute(301, "class", "toast-body");

				if (ContentIcon != null)
				{
					builder.OpenComponent(302, typeof(HxIcon));
					builder.AddAttribute(303, nameof(HxIcon.Icon), ContentIcon);
					builder.CloseComponent();
				}

				builder.AddContent(304, ContentText);
				builder.AddContent(305, ContentTemplate);

				if (!renderHeader && ShowCloseButton)
				{
					builder.OpenRegion(306);
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
			builder.AddAttribute(102, "class", "btn-close");
			builder.AddAttribute(103, "data-bs-dismiss", "toast");
			builder.AddAttribute(104, "aria-label", "Close");
			builder.CloseElement(); // button
		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (firstRender)
			{
				// we need to manualy setup the toast.
				await JSRuntime.InvokeVoidAsync("hxToast_show", toastElement, dotnetObjectReference);
			}
		}

		protected override bool ShouldRender()
		{
			// never update content to avoid collision with javascript
			return false;
		}

		/// <summary>
		/// Receive notification from javascript when message is hidden.
		/// </summary>
		[JSInvokable("HxToast_HandleToastHidden")]
		public async Task HandleToastHidden()
		{
			await ToastHidden.InvokeAsync(null);
		}

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			await JSRuntime.InvokeVoidAsync("hxToast_dispose", toastElement);
			dotnetObjectReference?.Dispose();
		}
	}
}
