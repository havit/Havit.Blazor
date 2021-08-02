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
	/// <summary>
	/// <a href="https://getbootstrap.com/docs/5.0/components/offcanvas/">Bootstrap Offcanvas</a> component (aka Drawer).
	/// </summary>
	public partial class HxOffcanvas : IAsyncDisposable
	{

		/// <summary>
		/// Application-wide defaults for the <see cref="HxGrid{TItem}"/>.
		/// </summary>
		public static OffcanvasDefaults Defaults { get; } = new OffcanvasDefaults();

		[Parameter] public string HeaderText { get; set; }

		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		/// <summary>
		/// Additional header CSS class.
		/// </summary>
		[Parameter] public string HeaderCssClass { get; set; }

		[Parameter] public RenderFragment BodyTemplate { get; set; }

		/// <summary>
		/// Additional body CSS class.
		/// </summary>
		[Parameter] public string BodyCssClass { get; set; }

		[Parameter] public RenderFragment FooterTemplate { get; set; }

		/// <summary>
		/// Additional footer CSS class.
		/// </summary>
		[Parameter] public string FooterCssClass { get; set; }

		/// <summary>
		/// Offcanvas additional CSS class. Added to root div (.offcanvas).
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Placement of the offcanvas. Default is <see cref="OffcanvasPlacement.End"/> (right).
		/// </summary>
		[Parameter] public OffcanvasPlacement Placement { get; set; } = OffcanvasPlacement.End;

		[Parameter] public OffcanvasRenderMode RenderMode { get; set; } = OffcanvasRenderMode.OpenOnly;

		/// <summary>
		/// Size of the offcanvas. Default is <see cref="OffcanvasSize.Regular"/>.
		/// </summary>
		[Parameter] public OffcanvasSize Size { get; set; } = OffcanvasSize.Regular;

		/// <summary>
		/// Indicates whether the offcanvas closes when escape key is pressed.
		/// Default value is true.
		/// </summary>
		[Parameter] public bool CloseOnEscape { get; set; }

		/// <summary>
		/// Close icon to be used to for close element when <b>ShouldUseCloseButton</b> is set to false.
		/// If set to null close element wont be rendered.
		/// </summary>
		[Parameter] public IconBase CloseIcon { get; set; }

		/// <summary>
		/// Switches between elements used for closing offcanvas.
		/// If true button is used otherwise HxIcon is used.
		/// </summary>
		[Parameter] public bool ShouldUseCloseButton { get; set; }

		/// <summary>
		/// Indicates whether to apply a backdrop on body while offcanvas is open.
		/// Default value is <c>true</c>.
		/// </summary>
		[Parameter] public bool BackdropEnabled { get; set; } = true;

		/// <summary>
		/// Indicates whether body (page) scrolling is allowed while offcanvas is open.
		/// Default value is <c>false</c>.
		/// </summary>
		[Parameter] public bool ScrollingEnabled { get; set; } = false;

		/// <summary>
		/// Raised when offcanvas is closed (whatever reason it is).
		/// </summary>
		[Parameter] public EventCallback OnClosed { get; set; }

		protected virtual OffcanvasDefaults GetDefaults() => HxOffcanvas.Defaults;

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private bool opened = false; // indicates whether the offcanvas is open
		private bool shouldOpenOffcanvas = false; // indicates whether the offcanvas is going to be opened
		private DotNetObjectReference<HxOffcanvas> dotnetObjectReference;
		private ElementReference offcanvasElement;
		private IJSObjectReference jsModule;

		/// <summary>
		/// Constructor.
		/// </summary>
		public HxOffcanvas()
		{
			dotnetObjectReference = DotNetObjectReference.Create(this);
		}

		/// <summary>
		/// Shows the offcanvas.
		/// </summary>
		public Task ShowAsync()
		{
			if (!opened)
			{
				shouldOpenOffcanvas = true; // mark offcanvas to be shown in OnAfterRender			
			}
			opened = true; // mark offcanvas as opened

			StateHasChanged(); // ensures render offcanvas HTML

			return Task.CompletedTask;
		}

		/// <summary>
		/// Hides the offcanvas.
		/// </summary>
		public async Task HideAsync()
		{
			Contract.Requires(opened);
			await jsModule.InvokeVoidAsync("hide", offcanvasElement);
		}

		private async Task HandleOnCloseClick()
		{
			await HideAsync();
		}

		[JSInvokable("HxOffcanvas_HandleOffcanvasHidden")]
		public async Task HandleOffcanvasHidden()
		{
			opened = false;
			await OnClosed.InvokeAsync(); // fires "event" dialog has been closed
			StateHasChanged(); // ensures rerender to remove dialog from HTML
		}


		/// <inheritdoc />
		protected override void OnInitialized()
		{
			base.OnInitialized();
			CloseIcon = GetDefaults().CloseIcon;
			ShouldUseCloseButton = GetDefaults().ShouldUseCloseButton;

		}

		/// <inheritdoc />
		protected override async Task OnAfterRenderAsync(bool firstRender)
		{
			await base.OnAfterRenderAsync(firstRender);

			if (shouldOpenOffcanvas)
			{
				// do not run show in every render
				// the line must be prior to JSRuntime (because BuildRenderTree/OnAfterRender[Async] is called twice; in the bad order of lines the JSRuntime would be also called twice).
				shouldOpenOffcanvas = false;

				// Running JS interop is postponed to OnAfterAsync to ensure offcanvasElement is set.
				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/hxoffcanvas.js");
				await jsModule.InvokeVoidAsync("show", offcanvasElement, dotnetObjectReference, BackdropEnabled, CloseOnEscape, ScrollingEnabled);
			}
		}

		/// <inheritdoc />
		public async ValueTask DisposeAsync()
		{
			if (opened)
			{
				// We need to remove backdrop when leaving "page" when HxOffcanvas is shown (opened).
				await jsModule.InvokeVoidAsync("dispose", offcanvasElement);
			}

			dotnetObjectReference.Dispose();

			if (jsModule != null)
			{
				await jsModule.DisposeAsync();
			}
		}

		private string GetPlacementCssClass()
		{
			return this.Placement switch
			{
				OffcanvasPlacement.Start => "offcanvas-start",
				OffcanvasPlacement.End => "offcanvas-end",
				OffcanvasPlacement.Bottom => "offcanvas-bottom",
				OffcanvasPlacement.Top => "offcanvas-top",
				_ => throw new InvalidOperationException($"Unknown {nameof(HxOffcanvas)}.{nameof(Placement)} value {this.Placement:g}.")
			};
		}

		private string GetSizeCssClass()
		{
			return this.Size switch
			{
				OffcanvasSize.Regular => null,
				OffcanvasSize.Small => "hx-offcanvas-sm",
				OffcanvasSize.Large => "hx-offcanvas-lg",
				_ => throw new InvalidOperationException($"Unknown {nameof(HxOffcanvas)}.{nameof(Size)} value {this.Size:g}.")
			};
		}
	}
}