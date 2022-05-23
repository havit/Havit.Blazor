using Havit.Diagnostics.Contracts;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.0/components/offcanvas/">Bootstrap Offcanvas</see> component (aka Drawer).
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxOffcanvas">https://havit.blazor.eu/components/HxOffcanvas</see>
	/// </summary>
	public partial class HxOffcanvas : IAsyncDisposable
	{
		/// <summary>
		/// Application-wide defaults for the <see cref="HxOffcanvas"/> and derived components.
		/// </summary>
		public static OffcanvasSettings Defaults { get; }

		static HxOffcanvas()
		{
			Defaults = new OffcanvasSettings()
			{
				ShowCloseButton = true,
				BackdropEnabled = true,
				Placement = OffcanvasPlacement.End,
				Size = OffcanvasSize.Regular,
				CloseOnEscape = true,
				ScrollingEnabled = false,
			};
		}

		/// <summary>
		/// Returns application-wide defaults for the component.
		/// Enables overriding defaults in descandants (use separate set of defaults).
		/// </summary>
		protected virtual OffcanvasSettings GetDefaults() => Defaults;

		/// <summary>
		/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overriden by individual parameters).
		/// </summary>
		[Parameter] public OffcanvasSettings Settings { get; set; }

		/// <summary>
		/// Returns optional set of component settings.
		/// </summary>
		/// <remarks>
		/// Simmilar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descandants (by returning a derived settings class).
		/// </remarks>
		protected virtual OffcanvasSettings GetSettings() => this.Settings;


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
		[Parameter] public OffcanvasPlacement? Placement { get; set; }
		protected OffcanvasPlacement PlacementEffective => this.Placement ?? this.GetSettings()?.Placement ?? GetDefaults().Placement ?? throw new InvalidOperationException(nameof(Placement) + " default for " + nameof(HxOffcanvas) + " has to be set.");

		/// <summary>
		/// Determines whether the content is always rendered or only if the offcanvas is open. Default is <see cref="OffcanvasRenderMode.OpenOnly"/>.
		/// </summary>
		[Parameter] public OffcanvasRenderMode RenderMode { get; set; } = OffcanvasRenderMode.OpenOnly;

		/// <summary>
		/// Size of the offcanvas. Default is <see cref="OffcanvasSize.Regular"/>.
		/// </summary>
		[Parameter] public OffcanvasSize? Size { get; set; }
		protected OffcanvasSize SizeEffective => this.Size ?? this.GetSettings()?.Size ?? GetDefaults().Size ?? throw new InvalidOperationException(nameof(Size) + " default for " + nameof(HxOffcanvas) + " has to be set.");

		/// <summary>
		/// Indicates whether the modal shows close button in header.
		/// Default value is <c>true</c>.
		/// Use <see cref="CloseButtonIcon"/> to change shape of the button.
		/// </summary>
		[Parameter] public bool? ShowCloseButton { get; set; }
		protected bool ShowCloseButtonEffective => this.ShowCloseButton ?? this.GetSettings()?.ShowCloseButton ?? GetDefaults().ShowCloseButton ?? throw new InvalidOperationException(nameof(ShowCloseButton) + " default for " + nameof(HxOffcanvas) + " has to be set.");

		/// <summary>
		/// Indicates whether the offcanvas closes when escape key is pressed.
		/// Default value is <c>true</c>.
		/// </summary>
		[Parameter] public bool? CloseOnEscape { get; set; }
		protected bool CloseOnEscapeEffective => this.CloseOnEscape ?? this.GetSettings()?.CloseOnEscape ?? GetDefaults().CloseOnEscape ?? throw new InvalidOperationException(nameof(CloseOnEscape) + " default for " + nameof(HxOffcanvas) + " has to be set.");

		/// <summary>
		/// Close icon to be used in header.
		/// If set to <c>null</c>, Bootstrap default close-button will be used.
		/// </summary>
		[Parameter] public IconBase CloseButtonIcon { get; set; }
		protected IconBase CloseButtonIconEffective => this.CloseButtonIcon ?? this.GetSettings()?.CloseButtonIcon ?? GetDefaults().CloseButtonIcon;

		/// <summary>
		/// Indicates whether to apply a backdrop on body while offcanvas is open.
		/// Default value (from <see cref="Defaults"/>) is <c>true</c>.
		/// </summary>
		[Parameter] public bool? BackdropEnabled { get; set; }
		protected bool BackdropEnabledEffective => this.BackdropEnabled ?? this.GetSettings()?.BackdropEnabled ?? GetDefaults().BackdropEnabled ?? throw new InvalidOperationException(nameof(BackdropEnabled) + " default for " + nameof(HxOffcanvas) + " has to be set.");

		/// <summary>
		/// Indicates whether body (page) scrolling is allowed while offcanvas is open.
		/// Default value (from <see cref="Defaults"/>) is <c>false</c>.
		/// </summary>
		[Parameter] public bool? ScrollingEnabled { get; set; }
		protected bool ScrollingEnabledEffective => this.ScrollingEnabled ?? this.GetSettings()?.ScrollingEnabled ?? GetDefaults().ScrollingEnabled ?? throw new InvalidOperationException(nameof(ScrollingEnabled) + " default for " + nameof(HxOffcanvas) + " has to be set.");

		/// <summary>
		/// Offcanvas additional CSS class. Added to root <c>div</c> (<c>.offcanvas</c>).
		/// </summary>
		[Parameter] public string CssClass { get; set; }
		protected string CssClassEffective => this.CssClass ?? this.GetSettings()?.CssClass ?? GetDefaults().CssClass;

		/// <summary>
		/// Additional header CSS class.
		/// </summary>
		[Parameter] public string HeaderCssClass { get; set; }
		protected string HeaderCssClassEffective => this.HeaderCssClass ?? this.GetSettings()?.HeaderCssClass ?? GetDefaults().HeaderCssClass;

		/// <summary>
		/// Additional body CSS class.
		/// </summary>
		[Parameter] public string BodyCssClass { get; set; }
		protected string BodyCssClassEffective => this.BodyCssClass ?? this.GetSettings()?.BodyCssClass ?? GetDefaults().BodyCssClass;

		/// <summary>
		/// Additional footer CSS class.
		/// </summary>
		[Parameter] public string FooterCssClass { get; set; }
		protected string FooterCssClassEffective => this.FooterCssClass ?? this.GetSettings()?.FooterCssClass ?? GetDefaults().FooterCssClass;

		/// <summary>
		/// This event is fired when an offcanvas element has been hidden from the user (will wait for CSS transitions to complete).
		/// </summary>
		[Parameter] public EventCallback OnClosed { get; set; }
		/// <summary>
		/// Triggers the <see cref="OnClosed"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeOnClosedAsync() => OnClosed.InvokeAsync();

		/// <summary>
		/// This event is fired when an offcanvas element has been made visible to the user (will wait for CSS transitions to complete).
		/// </summary>
		[Parameter] public EventCallback OnShown { get; set; }
		/// <summary>
		/// Triggers the <see cref="OnShown"/> event. Allows interception of the event in derived components.
		/// </summary>
		protected virtual Task InvokeOnShownAsync() => OnShown.InvokeAsync();


		[Inject] protected IJSRuntime JSRuntime { get; set; }

		private bool opened = false; // indicates whether the offcanvas is open
		private bool shouldOpenOffcanvas = false; // indicates whether the offcanvas is going to be opened
		private DotNetObjectReference<HxOffcanvas> dotnetObjectReference;
		private ElementReference offcanvasElement;
		private IJSObjectReference jsModule;
		private bool disposed;

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

			StateHasChanged(); // ensures rendering offcanvas HTML

			return Task.CompletedTask;
		}

		/// <summary>
		/// Hides the offcanvas (if opened).
		/// </summary>
		public async Task HideAsync()
		{
			if (!opened)
			{
				// this might be a minor PERF benefit, if it turns out to be causing troubles, we can remove this or make it configurable through optional method parameter
				return;
			}
			await jsModule.InvokeVoidAsync("hide", offcanvasElement);
		}

		[JSInvokable("HxOffcanvas_HandleOffcanvasHidden")]
		public async Task HandleOffcanvasHidden()
		{
			opened = false;
			await InvokeOnClosedAsync();
			StateHasChanged(); // ensures rerender to remove the control from HTML
		}

		/// <summary>
		/// Receives notification from JS for <c>shown.bs.offcanvas</c> event.
		/// </summary>
		[JSInvokable("HxOffcanvas_HandleOffcanvasShown")]
		public async Task HandleOffcanvasShown()
		{
			await InvokeOnShownAsync();
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

				// Running JS interop is postponed to OnAfterRenderAsync to ensure offcanvasElement is set.
				jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxOffcanvas));
				if (disposed)
				{
					return;
				}
				await jsModule.InvokeVoidAsync("show", offcanvasElement, dotnetObjectReference, BackdropEnabledEffective, CloseOnEscapeEffective, ScrollingEnabledEffective);
			}
		}

		/// <inheritdoc />

		public async ValueTask DisposeAsync()
		{
			await DisposeAsyncCore();

			//Dispose(disposing: false);
		}

		protected virtual async ValueTask DisposeAsyncCore()
		{
			disposed = true;

			if (jsModule != null)
			{
				if (opened)
				{
					// We need to remove backdrop when leaving "page" when HxOffcanvas is shown (opened).
					await jsModule.InvokeVoidAsync("dispose", offcanvasElement);
				}

				await jsModule.DisposeAsync();
			}

			dotnetObjectReference.Dispose();
		}

		private string GetPlacementCssClass()
		{
			return this.PlacementEffective switch
			{
				OffcanvasPlacement.Start => "offcanvas-start",
				OffcanvasPlacement.End => "offcanvas-end",
				OffcanvasPlacement.Bottom => "offcanvas-bottom",
				OffcanvasPlacement.Top => "offcanvas-top",
				_ => throw new InvalidOperationException($"Unknown {nameof(HxOffcanvas)}.{nameof(Placement)} value {this.PlacementEffective:g}.")
			};
		}

		private string GetSizeCssClass()
		{
			return this.SizeEffective switch
			{
				OffcanvasSize.Regular => null,
				OffcanvasSize.Small => "hx-offcanvas-sm",
				OffcanvasSize.Large => "hx-offcanvas-lg",
				_ => throw new InvalidOperationException($"Unknown {nameof(HxOffcanvas)}.{nameof(Size)} value {this.SizeEffective:g}.")
			};
		}
	}
}