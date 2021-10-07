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
		/// Application-wide defaults for the <see cref="HxOffcanvas"/>.
		/// </summary>
		public static OffcanvasDefaults Defaults { get; } = new OffcanvasDefaults();

		/// <summary>
		/// Text for the title in header.
		/// (Is rendered into <c>&lt;h5 class="offcanvas-title"&gt;</c> - in opposite to <see cref="HeaderTemplate"/> which is rendered directly into <c>offcanvas-header</c>).
		/// </summary>
		[Parameter] public string Title { get; set; }

		/// <summary>
		/// Content for the header.
		/// (Is rendered directly into <c>offcanvas-header</c> - in opposite to <see cref="Title"/> which is rendered into <c>&lt;h5 class="offcanvas-title"&gt;</c>).
		/// </summary>
		[Parameter] public RenderFragment HeaderTemplate { get; set; }

		/// <summary>
		/// Body content.
		/// </summary>
		[Parameter] public RenderFragment BodyTemplate { get; set; }

		/// <summary>
		/// Footer content.
		/// </summary>
		[Parameter] public RenderFragment FooterTemplate { get; set; }

		/// <summary>
		/// Placement of the offcanvas. Default is <see cref="OffcanvasPlacement.End"/> (right).
		/// </summary>
		[Parameter] public OffcanvasPlacement Placement { get; set; } = OffcanvasPlacement.End;

		/// <summary>
		/// Determines whether the content is always rendered or only if the offcanvas is open. Default is <see cref="OffcanvasRenderMode.OpenOnly"/>.
		/// </summary>
		[Parameter] public OffcanvasRenderMode RenderMode { get; set; } = OffcanvasRenderMode.OpenOnly;

		/// <summary>
		/// Size of the offcanvas. Default is <see cref="OffcanvasSize.Regular"/>.
		/// </summary>
		[Parameter] public OffcanvasSize Size { get; set; } = OffcanvasSize.Regular;

		/// <summary>
		/// Indicates whether the modal shows close button in header.
		/// Default value is <c>true</c>.
		/// Use <see cref="CloseButtonIcon"/> to change shape of the button.
		/// </summary>
		[Parameter] public bool? ShowCloseButton { get; set; }

		/// <summary>
		/// Indicates whether the offcanvas closes when escape key is pressed.
		/// Default value is <c>true</c>.
		/// </summary>
		[Parameter] public bool CloseOnEscape { get; set; } = true;

		/// <summary>
		/// Close icon to be used in header.
		/// If set to <c>null</c>, Bootstrap default close-button will be used.
		/// </summary>
		[Parameter] public IconBase CloseButtonIcon { get; set; }

		/// <summary>
		/// Indicates whether to apply a backdrop on body while offcanvas is open.
		/// Default value (from <see cref="Defaults"/>) is <c>true</c>.
		/// </summary>
		[Parameter] public bool? BackdropEnabled { get; set; }

		/// <summary>
		/// Indicates whether body (page) scrolling is allowed while offcanvas is open.
		/// Default value (from <see cref="Defaults"/>) is <c>false</c>.
		/// </summary>
		[Parameter] public bool ScrollingEnabled { get; set; } = false;

		/// <summary>
		/// Offcanvas additional CSS class. Added to root <c>div</c> (<c>.offcanvas</c>).
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		/// <summary>
		/// Additional header CSS class.
		/// </summary>
		[Parameter] public string HeaderCssClass { get; set; }

		/// <summary>
		/// Additional body CSS class.
		/// </summary>
		[Parameter] public string BodyCssClass { get; set; }

		/// <summary>
		/// Additional footer CSS class.
		/// </summary>
		[Parameter] public string FooterCssClass { get; set; }

		/// <summary>
		/// Raised when the offcanvas is closed (whatever reason there is).
		/// </summary>
		[Parameter] public EventCallback OnClosed { get; set; }

		protected virtual OffcanvasDefaults GetDefaults() => HxOffcanvas.Defaults;

		[Inject] protected IJSRuntime JSRuntime { get; set; }

		protected IconBase CloseButtonIconEffective => CloseButtonIcon ?? GetDefaults().CloseButtonIcon;
		protected bool ShowCloseButtonEffective => ShowCloseButton ?? GetDefaults().ShowCloseButton;
		protected string CssClassEffective => CssClass ?? GetDefaults().CssClass;
		protected string HeaderCssClassEffective => HeaderCssClass ?? GetDefaults().HeaderCssClass;
		protected string BodyCssClassEffective => BodyCssClass ?? GetDefaults().BodyCssClass;
		protected string FooterCssClassEffective => FooterCssClass ?? GetDefaults().FooterCssClass;
		protected bool BackdropEnabledEffective => BackdropEnabled ?? GetDefaults().BackdropEnabled;

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
			Contract.Requires(opened, "Offcanvas must be open if you want to hide it.");  // We might remove this check and "do nothing" if it turns to be more convenient for consuming developers.
			await jsModule.InvokeVoidAsync("hide", offcanvasElement);
		}

		[JSInvokable("HxOffcanvas_HandleOffcanvasHidden")]
		public async Task HandleOffcanvasHidden()
		{
			opened = false;
			await OnClosed.InvokeAsync(); // fires "event" dialog has been closed
			StateHasChanged(); // ensures rerender to remove dialog from HTML
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
				jsModule ??= await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Havit.Blazor.Components.Web.Bootstrap/" + nameof(HxOffcanvas) + ".js");
				await jsModule.InvokeVoidAsync("show", offcanvasElement, dotnetObjectReference, BackdropEnabledEffective, CloseOnEscape, ScrollingEnabled);
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