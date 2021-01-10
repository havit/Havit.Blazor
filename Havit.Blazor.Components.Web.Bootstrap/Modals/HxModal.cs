using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO: Uspořádání - jak umožnit použít "hezky" jakožto vlastní komponentu?
	public partial class HxModal : ComponentBase, IDisposable
	{
		[Parameter] public string CssClass { get; set; }

		[Parameter] public string Header { get; set; }

		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		[Parameter] public RenderFragment BodyTemplate { get; set; }

		[Parameter] public RenderFragment FooterTemplate { get; set; }

		[Parameter] public bool ShowCloseButton { get; set; } = true;

		[Parameter] public bool CloseOnEscape { get; set; } = true;

		[Parameter] public bool UseStaticBackdrop { get; set; } = true;

		[Parameter] public EventCallback OnClosed { get; set; }

		[Inject] public IJSRuntime JSRuntime { get; set; }

		private bool opened = false;
		private bool shouldOpenModal = false;
		private DotNetObjectReference<HxModal> dotnetObjectReference;
		private ElementReference modalElement;

		public HxModal()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		public Task ShowAsync()
		{
			Contract.Requires(!opened);

			opened = true; // mark modal as opened
			shouldOpenModal = true; // mak modal to be shown in OnAfterRender

			StateHasChanged(); // ensures render modal HTML

			return Task.CompletedTask;
		}

		public async Task HideAsync()
		{
			Contract.Requires(opened);
			await JSRuntime.InvokeVoidAsync("hxModal_hide", modalElement);
		}

		[JSInvokable]
		public async Task HxModal_HandleModalHidden()
		{
			opened = false;
			await OnClosed.InvokeAsync(); // fires "event" dialog has been closed
			StateHasChanged(); // ensures rerender to remove dialog from HTML
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (shouldOpenModal)
			{
				shouldOpenModal = false; // do not run hxModal_show in every render
				// Running JS interop is postponed to OnAfterAsync to ensure modalElement is set.
				await JSRuntime.InvokeVoidAsync("hxModal_show", modalElement, dotnetObjectReference, UseStaticBackdrop, CloseOnEscape);
			}
		}

		/// <inheritdoc />
		public void Dispose()
		{
			dotnetObjectReference.Dispose();
		}
	}
}
