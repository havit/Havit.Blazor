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
	// TODO: Jak se má správně renderovat HeaderTemplate?
	// TODO: Uspořádání - jak umožnit použít "hezky" jakožto vlastní komponentu?
	public partial class HxModal : ComponentBase, IAsyncDisposable
	{
		[Parameter] public string CssClass { get; set; }

		[Parameter] public string Header { get; set; }

		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		[Parameter] public RenderFragment BodyTemplate { get; set; }

		[Parameter] public RenderFragment FooterTemplate { get; set; }

		[Parameter] public bool ShowCloseButton { get; set; } = true;

		[Parameter] public bool CloseOnEscape { get; set; } = true;

		[Parameter] public bool UseStaticBackdrop { get; set; } = true;

		[Parameter] public EventCallback Closed { get; set; }

		[Inject] public IJSRuntime JSRuntime { get; set; }

		private bool opened = false;
		private bool shouldOpenModal = false;
		DotNetObjectReference<HxModal> dotnetObjectReference;
		private ElementReference modalElement;

		public HxModal()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		public Task ShowAsync()
		{
			Contract.Requires(!opened);

			opened = true;
			shouldOpenModal = true;

			StateHasChanged();

			return Task.CompletedTask;
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);
			if (shouldOpenModal)
			{
				shouldOpenModal = false;
				await JSRuntime.InvokeVoidAsync("hxModal_show", modalElement, dotnetObjectReference, UseStaticBackdrop, CloseOnEscape);
			}
		}

		[JSInvokable]
		public async Task HxModal_HandleModalHidden()
		{
			opened = false;
			await Closed.InvokeAsync();
		}

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			if (opened)
			{
				// TODO: Má to význam? Asi ne (ničíme modal, který se zničí sám).
				await JSRuntime.InvokeVoidAsync("hxModal_hide", modalElement);
			}
			dotnetObjectReference?.Dispose();
		}
	}
}
