using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Off-canvas <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/drawer/">Bootstrap Drawer</see> panel that slides in from any edge of the screen — ideal for navigation, filters, or supplementary content.<br />
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
			Instant = false,
			Translucent = false,
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
	/// In combination with <see cref="DrawerBackdrop.False"/>, the drawer opens as non-modal, i.e. users can interact with the rest of the page.
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
	/// When <c>true</c>, the drawer opens and closes without the slide animation (<c>drawer-instant</c>).
	/// Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Instant { get; set; }
	protected bool InstantEffective => Instant ?? GetSettings()?.Instant ?? GetDefaults().Instant ?? throw new InvalidOperationException(nameof(Instant) + " default for " + nameof(HxDrawer) + " has to be set.");

	/// <summary>
	/// When <c>true</c>, renders the drawer with a translucent, blurred background (<c>drawer-translucent</c>, new in Bootstrap 6).
	/// Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Translucent { get; set; }
	protected bool TranslucentEffective => Translucent ?? GetSettings()?.Translucent ?? GetDefaults().Translucent ?? throw new InvalidOperationException(nameof(Translucent) + " default for " + nameof(HxDrawer) + " has to be set.");

	/// <summary>
	/// Drawer additional CSS class. Added to the root <c>&lt;dialog&gt;</c> element (<c>.drawer</c>).
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional attributes to be splatted onto the underlying <c>&lt;dialog&gt;</c> element (e.g. <c>data-bs-theme</c>).
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

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

	/// <summary>
	/// Fired when the drawer refuses to close, i.e. on a click on the <see cref="DrawerBackdrop.Static"/> backdrop
	/// or on <kbd>Esc</kbd> with <see cref="CloseOnEscape"/> set to <c>false</c> (Bootstrap <c>hidePrevented.bs.drawer</c> event).
	/// </summary>
	/// <remarks>
	/// The <code>hidePrevented.bs.drawer</code> event is only subscribed to when the <see cref="OnHidePrevented"/> callback is set.
	/// </remarks>
	[Parameter] public EventCallback OnHidePrevented { get; set; }


	[Inject] protected IJSRuntime JSRuntime { get; set; }


	private bool _opened = false; // indicates whether the drawer is open

	// Bootstrap Drawer (DialogBase) internally tracks _isTransitioning and silently drops show()/hide() calls while a CSS transition
	// is in progress. We compensate by tracking the transition state on the C# side and deferring the next operation until the current
	// transition completes (via shown.bs.drawer / hidden.bs.drawer callbacks). Same as in HxDialog.
	private bool _transitionInProgress;
	private PendingOperation _pendingOperation = PendingOperation.None;

	private string _titleId = "hx" + Guid.NewGuid().ToString("N");
	private DotNetObjectReference<HxDrawer> _dotnetObjectReference;
	private ElementReference _drawerElement;
	private IJSObjectReference _jsModule;
	private Queue<Func<Task>> _onAfterRenderTasksQueue = new();
	private bool _disposed;

	private enum PendingOperation { None, Show, Hide }

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
		if (_transitionInProgress)
		{
			_pendingOperation = PendingOperation.Show;
			_opened = true; // ensure HTML content is rendered while waiting
			StateHasChanged();
			return Task.CompletedTask;
		}

		if (_opened)
		{
			return Task.CompletedTask;
		}

		_pendingOperation = PendingOperation.None;
		_transitionInProgress = true;

		_onAfterRenderTasksQueue.Enqueue(async () =>
		{
			// Running JS interop is postponed to OnAfterRenderAsync to ensure drawerElement is set
			// and correct order of commands (Show/Hide) is preserved
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxDrawer));
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("show", _drawerElement, _dotnetObjectReference, CloseOnEscapeEffective, ScrollingEnabledEffective, OnHiding.HasDelegate, OnHidePrevented.HasDelegate);
		});
		_opened = true; // mark drawer as opened

		StateHasChanged(); // ensures rendering drawer HTML

		return Task.CompletedTask;
	}

	/// <summary>
	/// Closes the drawer from the header close button.
	/// Not using <c>data-bs-dismiss</c> - the C# side has to know about the hide to track the transition (see <see cref="_transitionInProgress"/>).
	/// </summary>
	private Task HandleCloseButtonClick() => HideAsync();

	/// <summary>
	/// Hides the drawer (if opened).
	/// </summary>
	public Task HideAsync()
	{
		if (_transitionInProgress)
		{
			_pendingOperation = PendingOperation.Hide;
			return Task.CompletedTask;
		}

		if (!_opened)
		{
			// this might be a minor PERF benefit, if it turns out to be causing troubles, we can remove this or make it configurable through optional method parameter
			return Task.CompletedTask;
		}

		_pendingOperation = PendingOperation.None;
		_transitionInProgress = true;

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

		if (eventArgs.Cancel)
		{
			// The hide was canceled — hidden.bs.drawer will not fire,
			// so we must reset _transitionInProgress here to avoid blocking future operations.
			_transitionInProgress = false;

			if (_pendingOperation == PendingOperation.Hide)
			{
				_pendingOperation = PendingOperation.None;
				await HideAsync();
			}
			else if (_pendingOperation == PendingOperation.Show)
			{
				// Drawer is already shown, just clear the pending operation.
				_pendingOperation = PendingOperation.None;
			}
		}

		return eventArgs.Cancel;
	}

	/// <summary>
	/// Receives notification from JS for <c>hidePrevented.bs.drawer</c> event.
	/// </summary>
	[JSInvokable("HxDrawer_HandleDrawerHidePrevented")]
	public Task HandleDrawerHidePrevented() => OnHidePrevented.InvokeAsync();

	/// <summary>
	/// Receives notification from JS for <c>hidden.bs.drawer</c> event.
	/// </summary>
	[JSInvokable("HxDrawer_HandleDrawerHidden")]
	public async Task HandleDrawerHidden()
	{
		_opened = false;
		_transitionInProgress = false;

		if (_pendingOperation == PendingOperation.Show)
		{
			_pendingOperation = PendingOperation.None;
			await ShowAsync();
			return;
		}

		_pendingOperation = PendingOperation.None;
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
		_transitionInProgress = false;

		if (_pendingOperation == PendingOperation.Hide)
		{
			_pendingOperation = PendingOperation.None;
			await HideAsync();
			return;
		}

		_pendingOperation = PendingOperation.None;
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
		_transitionInProgress = false;
		_pendingOperation = PendingOperation.None;

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
			DrawerPlacement.Fullscreen => "drawer-fullscreen",
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