using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/drawer/">Bootstrap Drawer</see> component (aka Drawer).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxDrawer">https://havit.blazor.eu/components/HxDrawer</see>
/// </summary>
public partial class HxDrawer : IAsyncDisposable
{
	/// <summary>
	/// A value that is passed to the drawer constructor (in JS), when <see cref="Backdrop"/> is set to <see cref="DrawerBackdrop.Static"/>.
	/// </summary>
	private const string StaticBackdropValue = "static";

	/// <summary>
	/// Application-wide defaults for the <see cref="HxDrawer"/> and derived components.
	/// </summary>
	public static DrawerSettings Defaults { get; }

	static HxDrawer()
	{
		Defaults = new DrawerSettings()
		{
			ShowCloseButton = true,
			Backdrop = DrawerBackdrop.True,
			Placement = DrawerPlacement.End,
			ResponsiveBreakpoint = DrawerResponsiveBreakpoint.None,
			Size = DrawerSize.Regular,
			CloseOnEscape = true,
			ScrollingEnabled = false,
			Sheet = false,
		};
	}

	/// <summary>
	/// Returns application-wide defaults for the component.
	/// Enables overriding defaults in descendants (use separate set of defaults).
	/// </summary>
	protected virtual DrawerSettings GetDefaults() => Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public DrawerSettings Settings { get; set; }

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual DrawerSettings GetSettings() => Settings;

	/// <summary>
	/// Text for the title in the header.
	/// (Is rendered into <c>&lt;h5 class="drawer-title"&gt;</c> - in contrast to <see cref="HeaderTemplate"/> which is rendered directly into <c>drawer-header</c>).
	/// </summary>
	[Parameter] public string Title { get; set; }

	/// <summary>
	/// Content for the header.
	/// (Is rendered directly into <c>drawer-header</c> - in contrast to <see cref="Title"/> which is rendered into <c>&lt;h5 class="drawer-title"&gt;</c>).
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
	/// Placement of the drawer. The default is <see cref="DrawerPlacement.End"/> (right).
	/// </summary>
	[Parameter] public DrawerPlacement? Placement { get; set; }
	protected DrawerPlacement PlacementEffective => Placement ?? GetSettings()?.Placement ?? GetDefaults().Placement ?? throw new InvalidOperationException(nameof(Placement) + " default for " + nameof(HxDrawer) + " has to be set.");

	/// <summary>
	/// Breakpoint below which the contents are rendered outside the viewport in an drawer (above this breakpoint, the drawer body is rendered inside the viewport).
	/// </summary>
	[Parameter] public DrawerResponsiveBreakpoint? ResponsiveBreakpoint { get; set; }
	protected DrawerResponsiveBreakpoint ResponsiveBreakpointEffective => ResponsiveBreakpoint ?? GetSettings()?.ResponsiveBreakpoint ?? GetDefaults().ResponsiveBreakpoint ?? throw new InvalidOperationException(nameof(ResponsiveBreakpoint) + " default for " + nameof(HxDrawer) + " has to be set.");

	/// <summary>
	/// Determines whether the content is always rendered or only if the drawer is open.<br />
	/// The default is <see cref="DrawerRenderMode.OpenOnly"/>.<br />
	/// Please note, this setting applies only when <see cref="DrawerResponsiveBreakpoint.None"/> is set. For all other values, the content is always rendered (to be available for the mobile version).
	/// </summary>
	[Parameter] public DrawerRenderMode RenderMode { get; set; } = DrawerRenderMode.OpenOnly;

	/// <summary>
	/// Size of the drawer. The default is <see cref="DrawerSize.Regular"/>.
	/// </summary>
	[Parameter] public DrawerSize? Size { get; set; }
	protected DrawerSize SizeEffective => Size ?? GetSettings()?.Size ?? GetDefaults().Size ?? throw new InvalidOperationException(nameof(Size) + " default for " + nameof(HxDrawer) + " has to be set.");

	/// <summary>
	/// Indicates whether the drawer shows a close button in the header.
	/// The default value is <c>true</c>.
	/// Use <see cref="CloseButtonIcon"/> to change the shape of the button.
	/// </summary>
	[Parameter] public bool? ShowCloseButton { get; set; }
	protected bool ShowCloseButtonEffective => ShowCloseButton ?? GetSettings()?.ShowCloseButton ?? GetDefaults().ShowCloseButton ?? throw new InvalidOperationException(nameof(ShowCloseButton) + " default for " + nameof(HxDrawer) + " has to be set.");

	/// <summary>
	/// Indicates whether the drawer closes when the escape key is pressed.
	/// The default value is <c>true</c>.
	/// </summary>
	[Parameter] public bool? CloseOnEscape { get; set; }
	protected bool CloseOnEscapeEffective => CloseOnEscape ?? GetSettings()?.CloseOnEscape ?? GetDefaults().CloseOnEscape ?? throw new InvalidOperationException(nameof(CloseOnEscape) + " default for " + nameof(HxDrawer) + " has to be set.");

	/// <summary>
	/// The close icon to be used in the header.
	/// If set to <c>null</c>, the Bootstrap default close button will be used.
	/// </summary>
	[Parameter] public IconBase CloseButtonIcon { get; set; }
	protected IconBase CloseButtonIconEffective => CloseButtonIcon ?? GetSettings()?.CloseButtonIcon ?? GetDefaults().CloseButtonIcon;

	/// <summary>
	/// Indicates whether to apply a backdrop on the body while the drawer is open.
	/// If set to <see cref="DrawerBackdrop.Static"/>, the drawer cannot be closed by clicking on the backdrop.
	/// The default value (from <see cref="Defaults"/>) is <see cref="DrawerBackdrop.True"/>.
	/// </summary>
	[Parameter] public DrawerBackdrop? Backdrop { get; set; }
	protected DrawerBackdrop BackdropEffective => Backdrop ?? GetSettings()?.Backdrop ?? GetDefaults().Backdrop ?? throw new InvalidOperationException(nameof(Backdrop) + " default for " + nameof(HxDrawer) + " has to be set.");

	/// <summary>
	/// Indicates whether body (page) scrolling is allowed while the drawer is open.
	/// The default value (from <see cref="Defaults"/>) is <c>false</c>.
	/// </summary>
	[Parameter] public bool? ScrollingEnabled { get; set; }
	protected bool ScrollingEnabledEffective => ScrollingEnabled ?? GetSettings()?.ScrollingEnabled ?? GetDefaults().ScrollingEnabled ?? throw new InvalidOperationException(nameof(ScrollingEnabled) + " default for " + nameof(HxDrawer) + " has to be set.");

	/// <summary>
	/// When <c>true</c>, renders the drawer as a flush-to-edge sheet (no inset, border radius, or shadow) using the <c>drawer-sheet</c> variant (new in Bootstrap 6).
	/// Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Sheet { get; set; }
	protected bool SheetEffective => Sheet ?? GetSettings()?.Sheet ?? GetDefaults().Sheet ?? throw new InvalidOperationException(nameof(Sheet) + " default for " + nameof(HxDrawer) + " has to be set.");

	/// <summary>
	/// Drawer additional CSS class. Added to root <c>div</c> (<c>.drawer</c>).
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional header CSS class.
	/// </summary>
	[Parameter] public string HeaderCssClass { get; set; }
	protected string HeaderCssClassEffective => HeaderCssClass ?? GetSettings()?.HeaderCssClass ?? GetDefaults().HeaderCssClass;

	/// <summary>
	/// Additional body CSS class.
	/// </summary>
	[Parameter] public string BodyCssClass { get; set; }
	protected string BodyCssClassEffective => BodyCssClass ?? GetSettings()?.BodyCssClass ?? GetDefaults().BodyCssClass;

	/// <summary>
	/// Additional footer CSS class.
	/// </summary>
	[Parameter] public string FooterCssClass { get; set; }
	protected string FooterCssClassEffective => FooterCssClass ?? GetSettings()?.FooterCssClass ?? GetDefaults().FooterCssClass;

	/// <summary>
	/// This event is fired when an drawer element has been hidden from the user (will wait for CSS transitions to complete).
	/// </summary>
	[Parameter] public EventCallback OnClosed { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClosed"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClosedAsync() => OnClosed.InvokeAsync();

	/// <summary>
	/// This event is fired when an drawer element has been made visible to the user (will wait for CSS transitions to complete).
	/// </summary>
	[Parameter] public EventCallback OnShown { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnShown"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnShownAsync() => OnShown.InvokeAsync();

	/// <summary>
	/// Fired immediately when the 'hide' instance method is called.
	/// To cancel hiding, set <see cref="DrawerHidingEventArgs.Cancel"/> to <c>true</c>.
	/// </summary>
	/// <remarks>
	/// 1) This event should probably be named <c>OnClosing</c> to be consistent with other members, but "hide" is the Bootstrap event name and we should stick to it.
	/// We should consider renaming the other members in the future.
	/// 2) There is intentionally no <c>virtual InvokeOnHidingAsync()</c> method to override to avoid confusion.
	/// The <code>hide.bs.drawer</code> event is only subscribed to when the <see cref="OnHiding"/> callback is set.
	/// </remarks>
	[Parameter] public EventCallback<DrawerHidingEventArgs> OnHiding { get; set; }


	[Inject] protected IJSRuntime JSRuntime { get; set; }


	private bool _opened = false; // indicates whether the drawer is open
	private string _drawerId = "hx" + Guid.NewGuid().ToString("N");
	private DotNetObjectReference<HxDrawer> _dotnetObjectReference;
	private ElementReference _drawerElement;
	private IJSObjectReference _jsModule;
	private Queue<Func<Task>> _onAfterRenderTasksQueue = new();
	private bool _disposed;

	/// <summary>
	/// Constructor.
	/// </summary>
	public HxDrawer()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	/// <summary>
	/// Shows the drawer.
	/// </summary>
	public Task ShowAsync()
	{
		if (!_opened)
		{
			_onAfterRenderTasksQueue.Enqueue(async () =>
			{
				// Running JS interop is postponed to OnAfterRenderAsync to ensure drawerElement is set
				// and correct order of commands (Show/Hide) is preserved
				_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxDrawer));
				if (_disposed)
				{
					return;
				}
				await _jsModule.InvokeVoidAsync("show", _drawerElement, _dotnetObjectReference, CloseOnEscapeEffective, ScrollingEnabledEffective, OnHiding.HasDelegate);
			});
		}
		_opened = true; // mark drawer as opened

		StateHasChanged(); // ensures rendering drawer HTML

		return Task.CompletedTask;
	}

	/// <summary>
	/// Hides the drawer (if opened).
	/// </summary>
	public Task HideAsync()
	{
		if (!_opened)
		{
			// this might be a minor PERF benefit, if it turns out to be causing troubles, we can remove this or make it configurable through optional method parameter
			return Task.CompletedTask;
		}

		_onAfterRenderTasksQueue.Enqueue(async () =>
		{
			// Running JS interop is postponed to OnAfterRenderAsync to ensure drawerElement is set
			// and correct order of commands (Show/Hide) is preserved
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxDrawer));
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("hide", _drawerElement);
		});
		StateHasChanged(); // enforce rendering

		return Task.CompletedTask;
	}

	/// <summary>
	/// Receives notification from JS for <c>hide.bs.drawer</c> event.
	/// </summary>
	[JSInvokable("HxDrawer_HandleDrawerHide")]
	public async Task<bool> HandleDrawerHide()
	{
		var eventArgs = new DrawerHidingEventArgs();
		await OnHiding.InvokeAsync(eventArgs);
		return eventArgs.Cancel;
	}

	[JSInvokable("HxDrawer_HandleDrawerHidden")]
	public async Task HandleDrawerHidden()
	{
		_opened = false;
		await InvokeOnClosedAsync();
		StateHasChanged(); // ensures re-render to remove the control from HTML
	}

	/// <summary>
	/// Receives notification from JS for <c>shown.bs.drawer</c> event.
	/// </summary>
	[JSInvokable("HxDrawer_HandleDrawerShown")]
	public async Task HandleDrawerShown()
	{
		_opened = true;
		await InvokeOnShownAsync();
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		while (_onAfterRenderTasksQueue.TryDequeue(out var task))
		{
			await task();
		}
	}

	/// <summary>
	/// Formats a <see cref="DrawerBackdrop"/> for supplying the value via the <c>data-bs-backdrop</c> data attribute.
	/// </summary>
	/// <param name="backdrop"></param>
	/// <returns></returns>
	private string GetBackdropSetupValue(DrawerBackdrop backdrop)
	{
		if (backdrop == DrawerBackdrop.Static)
		{
			return StaticBackdropValue;
		}
		else if (backdrop == DrawerBackdrop.True)
		{
			return "true";
		}
		else
		{
			return "false";
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
		_disposed = true;

		if (_jsModule != null)
		{
			try
			{
				await _jsModule.InvokeVoidAsync("dispose", _drawerElement, _opened);
				await _jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
			catch (TaskCanceledException)
			{
				// NOOP
			}
		}

		_dotnetObjectReference.Dispose();
	}

	private string GetDrawerResponsiveCssClass()
	{
		return ResponsiveBreakpointEffective switch
		{
			DrawerResponsiveBreakpoint.None => "drawer",
			DrawerResponsiveBreakpoint.Small => "sm:drawer",
			DrawerResponsiveBreakpoint.Medium => "md:drawer",
			DrawerResponsiveBreakpoint.Large => "lg:drawer",
			DrawerResponsiveBreakpoint.ExtraLarge => "xl:drawer",
			DrawerResponsiveBreakpoint.Xxl => "2xl:drawer",
			_ => throw new InvalidOperationException($"Unknown {nameof(HxDrawer)}.{nameof(ResponsiveBreakpoint)} value {ResponsiveBreakpointEffective:g}.")
		};
	}

	private string GetPlacementCssClass()
	{
		return PlacementEffective switch
		{
			DrawerPlacement.Start => "drawer-start",
			DrawerPlacement.End => "drawer-end",
			DrawerPlacement.Bottom => "drawer-bottom",
			DrawerPlacement.Top => "drawer-top",
			_ => throw new InvalidOperationException($"Unknown {nameof(HxDrawer)}.{nameof(Placement)} value {PlacementEffective:g}.")
		};
	}

	private string GetSizeCssClass()
	{
		return SizeEffective switch
		{
			DrawerSize.Regular => null,
			DrawerSize.Small => "hx-drawer-sm",
			DrawerSize.Large => "hx-drawer-lg",
			_ => throw new InvalidOperationException($"Unknown {nameof(HxDrawer)}.{nameof(Size)} value {SizeEffective:g}.")
		};
	}
}