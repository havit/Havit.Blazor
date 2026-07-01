using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Component for rendering a dialog as a Bootstrap 6 Dialog (built on the native <c>&lt;dialog&gt;</c> element).
/// Visit <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/dialog/">Bootstrap Dialog</see> for more information.
/// Full documentation and demos available at <see href="https://havit.blazor.eu/components/HxDialog">https://havit.blazor.eu/components/HxDialog</see>
/// </summary>
public partial class HxDialog : IAsyncDisposable
{
	/// <summary>
	/// Specifies a value for the <c>data-bs-backdrop</c> attribute when <see cref="Backdrop"/> is set to <see cref="DialogBackdrop.Static"/>.
	/// </summary>
	private const string StaticBackdropValue = "static";

	/// <summary>
	/// Application-wide default settings for the <see cref="HxDialog"/>.
	/// </summary>
	public static DialogSettings Defaults { get; }

	static HxDialog()
	{
		Defaults = new DialogSettings()
		{
			Animation = DialogAnimation.Fade,
			ShowCloseButton = true,
			CloseOnEscape = true,
			Size = DialogSize.Regular,
			Fullscreen = DialogFullscreen.Disabled,
			Backdrop = DialogBackdrop.True,
			Scrollable = false,
			NonModal = false,
		};
	}

	/// <summary>
	/// Provides application-wide default settings for the component.
	/// Allows descendants to override these defaults with a separate set of values.
	/// </summary>
	protected virtual DialogSettings GetDefaults() => Defaults;

	/// <summary>
	/// A set of settings applied to this component instance. Overrides <see cref="Defaults"/> and is itself overridden by individual parameters.
	/// </summary>
	[Parameter] public DialogSettings Settings { get; set; }

	/// <summary>
	/// Open/close animation of the dialog. Default is <see cref="DialogAnimation.Fade"/> (Bootstrap default).
	/// </summary>
	[Parameter] public DialogAnimation? Animation { get; set; }
	protected DialogAnimation AnimationEffective => Animation ?? GetSettings()?.Animation ?? GetDefaults().Animation ?? throw new InvalidOperationException(nameof(Animation) + " default for " + nameof(HxDialog) + " has to be set.");

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual DialogSettings GetSettings() => Settings;


	/// <summary>
	/// Title in dialog header.
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
	/// Size of the dialog. Default is <see cref="DialogSize.Regular"/>.
	/// </summary>
	[Parameter] public DialogSize? Size { get; set; }
	protected DialogSize SizeEffective => Size ?? GetSettings()?.Size ?? GetDefaults().Size ?? throw new InvalidOperationException(nameof(Size) + " default for " + nameof(HxDialog) + " has to be set.");

	/// <summary>
	/// Fullscreen behavior of the dialog. Default is <see cref="DialogFullscreen.Disabled"/>.
	/// </summary>
	[Parameter] public DialogFullscreen? Fullscreen { get; set; }
	protected DialogFullscreen FullscreenEffective => Fullscreen ?? GetSettings()?.Fullscreen ?? GetDefaults().Fullscreen ?? throw new InvalidOperationException(nameof(Fullscreen) + " default for " + nameof(HxDialog) + " has to be set.");

	/// <summary>
	/// Determines whether the content is always rendered or only if the dialog is open.<br />
	/// The default is <see cref="DialogRenderMode.OpenOnly"/>.<br />
	/// </summary>
	[Parameter] public DialogRenderMode RenderMode { get; set; } = DialogRenderMode.OpenOnly;

	/// <summary>
	/// Indicates whether the dialog shows close button in header.
	/// Default value is <c>true</c>.
	/// </summary>
	[Parameter] public bool? ShowCloseButton { get; set; }
	protected bool ShowCloseButtonEffective => ShowCloseButton ?? GetSettings()?.ShowCloseButton ?? GetDefaults().ShowCloseButton ?? throw new InvalidOperationException(nameof(ShowCloseButton) + " default for " + nameof(HxDialog) + " has to be set.");

	/// <summary>
	/// Close icon to be used in header.
	/// If set to <c>null</c>, Bootstrap default close-button will be used.
	/// </summary>
	[Parameter] public IconBase CloseButtonIcon { get; set; }
	protected IconBase CloseButtonIconEffective => CloseButtonIcon ?? GetSettings()?.CloseButtonIcon ?? GetDefaults().CloseButtonIcon;

	/// <summary>
	/// Indicates whether the dialog closes when escape key is pressed.
	/// Default value is <c>true</c>.
	/// </summary>
	[Parameter] public bool? CloseOnEscape { get; set; }
	protected bool CloseOnEscapeEffective => CloseOnEscape ?? GetSettings()?.CloseOnEscape ?? GetDefaults().CloseOnEscape ?? throw new InvalidOperationException(nameof(CloseOnEscape) + " default for " + nameof(HxDialog) + " has to be set.");

	/// <summary>
	/// Indicates whether to apply a backdrop on body while the dialog is open.
	/// If set to <see cref="DialogBackdrop.Static"/>, the dialog cannot be closed by clicking on the backdrop.
	/// Default value (from <see cref="Defaults"/>) is <see cref="DialogBackdrop.True"/>.
	/// </summary>
	[Parameter] public DialogBackdrop? Backdrop { get; set; }
	protected DialogBackdrop BackdropEffective => Backdrop ?? GetSettings()?.Backdrop ?? GetDefaults().Backdrop ?? throw new InvalidOperationException(nameof(Backdrop) + " default for " + nameof(HxDialog) + " has to be set.");

	/// <summary>
	/// Allows scrolling the dialog body. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Scrollable { get; set; }
	protected bool ScrollableEffective => Scrollable ?? GetSettings()?.Scrollable ?? GetDefaults().Scrollable ?? throw new InvalidOperationException(nameof(Scrollable) + " default for " + nameof(HxDialog) + " has to be set.");

	/// <summary>
	/// When <c>true</c>, the dialog opens as non-modal (browser-native <c>show()</c> instead of <c>showModal()</c>):
	/// no backdrop, no focus trap, no blocking of the rest of the page. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? NonModal { get; set; }
	protected bool NonModalEffective => NonModal ?? GetSettings()?.NonModal ?? GetDefaults().NonModal ?? throw new InvalidOperationException(nameof(NonModal) + " default for " + nameof(HxDialog) + " has to be set.");

	/// <summary>
	/// Additional CSS class for the main element (<c>div.dialog</c>).
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional header CSS class (<c>div.dialog-header</c>).
	/// </summary>
	[Parameter] public string HeaderCssClass { get; set; }
	protected string HeaderCssClassEffective => HeaderCssClass ?? GetSettings()?.HeaderCssClass ?? GetDefaults().HeaderCssClass;

	/// <summary>
	/// Additional body CSS class (<c>div.dialog-body</c>).
	/// </summary>
	[Parameter] public string BodyCssClass { get; set; }
	protected string BodyCssClassEffective => BodyCssClass ?? GetSettings()?.BodyCssClass ?? GetDefaults().BodyCssClass;

	/// <summary>
	/// Additional footer CSS class (<c>div.dialog-footer</c>).
	/// </summary>
	[Parameter] public string FooterCssClass { get; set; }
	protected string FooterCssClassEffective => FooterCssClass ?? GetSettings()?.FooterCssClass ?? GetDefaults().FooterCssClass;

	/// <summary>
	/// Fired immediately when the 'hide' instance method is called.
	/// This can be triggered by <see cref="HideAsync"/>, the close button, the <kbd>Esc</kbd> key, or other interactions.
	/// To cancel hiding, set <see cref="DialogHidingEventArgs.Cancel"/> to <c>true</c>.
	/// </summary>
	/// <remarks>
	/// There is intentionally no <c>virtual InvokeOnHidingAsync()</c> method to override to avoid confusion.
	/// The <code>hide.bs.dialog</code> event is only subscribed to when the <see cref="OnHiding"/> callback is set.
	/// </remarks>
	[Parameter] public EventCallback<DialogHidingEventArgs> OnHiding { get; set; }

	/// <summary>
	/// Fired when the dialog has finished hiding from the user, after CSS transitions complete.
	/// Triggered by <see cref="HideAsync"/>, the close button, the <kbd>Esc</kbd> key, or other interactions.
	/// </summary>
	[Parameter] public EventCallback OnClosed { get; set; }

	/// <summary>
	/// Triggers the <see cref="OnClosed"/> event. Enables derived components to intercept the event.
	/// </summary>
	protected virtual Task InvokeOnClosedAsync() => OnClosed.InvokeAsync();

	/// <summary>
	/// Fired when a dialog element becomes visible to the user, after CSS transitions complete.
	/// </summary>
	[Parameter] public EventCallback OnShown { get; set; }

	/// <summary>
	/// Triggers the <see cref="OnShown"/> event. Enables derived components to intercept the event.
	/// </summary>
	protected virtual Task InvokeOnShownAsync() => OnShown.InvokeAsync();


	[Inject] protected IJSRuntime JSRuntime { get; set; }


	private bool _opened = false; // indicates whether the dialog is open

	// Bootstrap Dialog internally tracks _isTransitioning and silently drops show()/hide() calls while a CSS transition
	// is in progress. This means a rapid sequence like HideAsync → ShowAsync → HideAsync can lose operations.
	// We compensate by tracking the transition state on the C# side and deferring the next operation until the current
	// transition completes (via shown.bs.dialog / hidden.bs.dialog callbacks).
	// Note: HxOffcanvas does NOT need this — Bootstrap Offcanvas does not check _isTransitioning,
	// so rapid sequences are handled correctly by Bootstrap itself.
	private bool _transitionInProgress;
	private PendingOperation _pendingOperation = PendingOperation.None;

	private DotNetObjectReference<HxDialog> _dotnetObjectReference;
	private ElementReference _dialogElement;
	private IJSObjectReference _jsModule;
	private Queue<Func<Task>> _onAfterRenderTasksQueue = new();
	private bool _disposed;

	private enum PendingOperation { None, Show, Hide }

	public HxDialog()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	/// <summary>
	/// Opens the dialog.
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
			// Running JS interop is postponed to OnAfterRenderAsync to ensure dialogElement is set
			// and correct order of commands (Show/Hide) is preserved
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxDialog));
			if (_disposed)
			{
				return;
			}

			await _jsModule.InvokeVoidAsync("show", _dialogElement, _dotnetObjectReference, CloseOnEscapeEffective, OnHiding.HasDelegate);
		});
		_opened = true; // mark dialog as opened

		StateHasChanged(); // ensures rendering dialog HTML

		return Task.CompletedTask;
	}

	/// <summary>
	/// Closes the dialog from the header close button.
	/// </summary>
	private Task HandleCloseButtonClick() => HideAsync();

	/// <summary>
	/// Closes the dialog.
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
			return Task.CompletedTask;
		}

		_pendingOperation = PendingOperation.None;
		_transitionInProgress = true;

		_onAfterRenderTasksQueue.Enqueue(async () =>
		{
			// Running JS interop is postponed to OnAfterRenderAsync to ensure dialogElement is set
			// and correct order of commands (Show/Hide) is preserved
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxDialog));
			if (_disposed)
			{
				return;
			}

			await _jsModule.InvokeVoidAsync("hide", _dialogElement);
		});
		StateHasChanged(); // enforce rendering

		return Task.CompletedTask;
	}

	/// <summary>
	/// Receives notification from JS for <c>hide.bs.dialog</c> event.
	/// </summary>
	[JSInvokable("HxDialog_HandleDialogHide")]
	public async Task<bool> HandleDialogHide()
	{
		var eventArgs = new DialogHidingEventArgs();
		await OnHiding.InvokeAsync(eventArgs);

		if (eventArgs.Cancel)
		{
			// The hide was canceled — hidden.bs.dialog will not fire,
			// so we must reset _transitionInProgress here to avoid blocking future operations.
			_transitionInProgress = false;

			if (_pendingOperation == PendingOperation.Hide)
			{
				_pendingOperation = PendingOperation.None;
				await HideAsync();
			}
			else if (_pendingOperation == PendingOperation.Show)
			{
				// Dialog is already shown, just clear the pending operation.
				_pendingOperation = PendingOperation.None;
			}
		}

		return eventArgs.Cancel;
	}

	/// <summary>
	/// Receives notification from JS for <c>hidden.bs.dialog</c> event.
	/// </summary>
	[JSInvokable("HxDialog_HandleDialogHidden")]
	public async Task HandleDialogHidden()
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
		StateHasChanged(); // ensures re-render to remove dialog from HTML
	}

	/// <summary>
	/// Receives notification from JS for <c>shown.bs.dialog</c> event.
	/// </summary>
	[JSInvokable("HxDialog_HandleDialogShown")]
	public async Task HandleDialogShown()
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

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		while (_onAfterRenderTasksQueue.TryDequeue(out var task))
		{
			await task();
		}
	}

	/// <summary>
	/// Formats a <see cref="DialogBackdrop"/> value for use in the <c>data-bs-backdrop</c> attribute.
	/// </summary>
	private string GetBackdropSetupValue(DialogBackdrop backdrop)
	{
		return backdrop switch
		{
			DialogBackdrop.Static => StaticBackdropValue,
			DialogBackdrop.True => "true",
			DialogBackdrop.False => "false",
			_ => throw new InvalidOperationException($"Unknown {nameof(DialogBackdrop)} value {BackdropEffective}")
		};
	}

	protected string GetSizeCssClass()
	{
		return SizeEffective switch
		{
			DialogSize.Small => "dialog-sm",
			DialogSize.Regular => null,
			DialogSize.Large => "dialog-lg",
			DialogSize.ExtraLarge => "dialog-xl",
			_ => throw new InvalidOperationException($"Unknown {nameof(DialogSize)} value {SizeEffective}.")
		};
	}

	protected string GetFullscreenCssClass()
	{
		return FullscreenEffective switch
		{
			DialogFullscreen.Disabled => null,
			DialogFullscreen.Always => "dialog-fullscreen",
			DialogFullscreen.SmallDown => "sm-down:dialog-fullscreen",
			DialogFullscreen.MediumDown => "md-down:dialog-fullscreen",
			DialogFullscreen.LargeDown => "lg-down:dialog-fullscreen",
			DialogFullscreen.ExtraLargeDown => "xl-down:dialog-fullscreen",
			DialogFullscreen.XxlDown => "2xl-down:dialog-fullscreen",
			_ => throw new InvalidOperationException($"Unknown {nameof(DialogFullscreen)} value {FullscreenEffective}.")
		};
	}

	protected string GetScrollableCssClass()
	{
		if (ScrollableEffective)
		{
			return "dialog-scrollable";
		}
		return null;
	}

	protected string GetAnimationCssClass()
	{
		return AnimationEffective switch
		{
			DialogAnimation.Fade => null,
			DialogAnimation.None => "dialog-instant",
			DialogAnimation.SlideDown => "dialog-slide-down",
			DialogAnimation.SlideUp => "dialog-slide-up",
			_ => throw new InvalidOperationException($"Unknown {nameof(DialogAnimation)} value {AnimationEffective}.")
		};
	}

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
				await _jsModule.InvokeVoidAsync("dispose", _dialogElement, _opened);
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
}
