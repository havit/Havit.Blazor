using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Component to render modal dialog as a <a href="https://getbootstrap.com/docs/5.1/components/modal/">Bootstrap Modal</a>.
	/// </summary>
	public partial class HxModal : IAsyncDisposable
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxGrid{TItem}"/>.
		/// </summary>
		public static ModalDefaults Defaults { get; } = new ModalDefaults();

		/// <summary>
		/// Title in modal header.
		/// </summary>
		[Parameter] public string Title { get; set; }

		/// <summary>
		/// Header template.
		/// </summary>
		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		/// <summary>
		/// Body template.
		/// </summary>
		[Parameter] public RenderFragment BodyTemplate { get; set; }

		/// <summary>
		/// Footer template.
		/// </summary>
		[Parameter] public RenderFragment FooterTemplate { get; set; }

		/// <summary>
		/// Size of the modal. Default is <see cref="ModalSize.Regular"/>.
		/// </summary>
		[Parameter] public ModalSize? Size { get; set; }

		/// <summary>
		/// Fullscreen behavior of the modal. Default is <see cref="ModalFullscreen.Disabled"/>.
		/// </summary>
		[Parameter] public ModalFullscreen? Fullscreen { get; set; }

		/// <summary>
		/// Indicates whether the modal shows close button in header.
		/// Default value is <c>true</c>.
		/// </summary>
		[Parameter] public bool? ShowCloseButton { get; set; }

		/// <summary>
		/// Close icon to be used in header.
		/// If set to <c>null</c>, Bootstrap default close-button will be used.
		/// </summary>
		[Parameter] public IconBase CloseButtonIcon { get; set; }

		/// <summary>
		/// Indicates whether the modal closes when escape key is pressed.
		/// Default value is <c>true</c>.
		/// </summary>
		[Parameter] public bool CloseOnEscape { get; set; } = true;

		/// <summary>
		/// Indicates whether the modal uses a static backdrop.
		/// Default value is <c>true</c>.
		/// </summary>
		[Parameter] public bool UseStaticBackdrop { get; set; } = true;

		/// <summary>
		/// Allows scrolling the modal body. Default is <c>false</c>.
		/// </summary>
		[Parameter] public bool? Scrollable { get; set; }

		/// <summary>
		/// Allows vertical centering of the modal.<br/>
		/// Default is <c>false</c> (horizontal only).
		/// </summary>
		[Parameter] public bool? Centered { get; set; }

		/// <summary>
		/// Additional CSS class for the main element (<c>div.modal</c>).
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Additional CSS class for the dialog (<c>div.modal-dialog</c> element).
		/// </summary>
		[Parameter] public string DialogCssClass { get; set; }

		/// <summary>
		/// Additional header CSS class.
		/// </summary>
		[Parameter] public string HeaderCssClass { get; set; }

		/// <summary>
		/// Additional body CSS class.
		/// </summary>
		[Parameter] public string BodyCssClass { get; set; }

		/// <summary>
		/// Footer css class.
		/// </summary>
		[Parameter] public string FooterCssClass { get; set; }

		/// <summary>
		/// Raised when modal is closed (whatever reason it is).
		/// </summary>
		[Parameter] public EventCallback OnClosed { get; set; }


		[Inject] protected IJSRuntime JSRuntime { get; set; }


		protected virtual ModalDefaults GetDefaults() => HxModal.Defaults;

		protected IconBase CloseButtonIconEffective => CloseButtonIcon ?? GetDefaults().CloseButtonIcon;
		protected bool ShowCloseButtonEffective => ShowCloseButton ?? GetDefaults().ShowCloseButton;

		private bool opened = false; // indicates whether the modal is open
		private bool shouldOpenModal = false; // indicates whether the modal is going to be opened
		private DotNetObjectReference<HxModal> dotnetObjectReference;
		private ElementReference modalElement;
		private IJSObjectReference jsModule;

		public HxModal()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		/// <summary>
		/// Opens the modal.
		/// </summary>
		public Task ShowAsync()
		{
			opened = true; // mark modal as opened
			shouldOpenModal = true; // mark modal to be shown in OnAfterRender			

			StateHasChanged(); // ensures render modal HTML

			return Task.CompletedTask;
		}

		/// <summary>
		/// Closes the modal.
		/// </summary>
		public async Task HideAsync()
		{
			await jsModule.InvokeVoidAsync("hide", modalElement);
		}

		[JSInvokable("HxModal_HandleModalHidden")]
		public async Task HandleModalHidden()
		{
			opened = false;
			await OnClosed.InvokeAsync();
			StateHasChanged(); // ensures rerender to remove dialog from HTML
		}

		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (shouldOpenModal)
			{
				// do not run show in every render
				// the line must be prior to JSRuntime (because BuildRenderTree/OnAfterRender[Async] is called twice; in the bad order of lines the JSRuntime would be also called twice).
				shouldOpenModal = false;

				// Running JS interop is postponed to OnAfterAsync to ensure modalElement is set.
				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/" + nameof(HxModal) + ".js");
				await jsModule.InvokeVoidAsync("show", modalElement, dotnetObjectReference, UseStaticBackdrop, CloseOnEscape);
			}
		}

		public async ValueTask DisposeAsync()
		{
			if (opened)
			{
				// We need to remove backdrop when leaving "page" when HxModal is shown (opened).
				await jsModule.InvokeVoidAsync("dispose", modalElement);
			}

			dotnetObjectReference.Dispose();

			if (jsModule != null)
			{
				await jsModule.DisposeAsync();
			}

		}

		protected string GetDialogSizeCssClass()
		{
			var sizeEffective = this.Size ?? GetDefaults().Size;
			return sizeEffective switch
			{
				ModalSize.Small => "modal-sm",
				ModalSize.Regular => null,
				ModalSize.Large => "modal-lg",
				ModalSize.ExtraLarge => "modal-xl",
				_ => throw new InvalidOperationException($"Unknown {nameof(ModalSize)} value {sizeEffective}.")
			};
		}

		protected string GetDialogFullscreenCssClass()
		{
			var fullscreenEffective = this.Fullscreen ?? GetDefaults().Fullscreen;
			return fullscreenEffective switch
			{
				ModalFullscreen.Disabled => null,
				ModalFullscreen.Always => "modal-fullscreen",
				ModalFullscreen.SmallDown => "modal-fullscreen-sm-down",
				ModalFullscreen.MediumDown => "modal-fullscreen-md-down",
				ModalFullscreen.LargeDown => "modal-fullscreen-lg-down",
				ModalFullscreen.ExtraLargeDown => "modal-fullscreen-xl-down",
				ModalFullscreen.XxlDown => "modal-fullscreen-xxl-down",
				_ => throw new InvalidOperationException($"Unknown {nameof(ModalFullscreen)} value {fullscreenEffective}.")
			};
		}

		protected string GetDialogScrollableCssClass()
		{
			var scrollableEffective = this.Scrollable ?? GetDefaults().Scrollable;
			if (scrollableEffective)
			{
				return "modal-dialog-scrollable";
			}
			return null;
		}

		protected string GetDialogCenteredCssClass()
		{
			var centeredEffective = this.Centered ?? GetDefaults().Centered;
			if (centeredEffective)
			{
				return "modal-dialog-centered";
			}
			return null;
		}
	}
}
