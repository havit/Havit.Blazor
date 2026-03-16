using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Component for rendering a modal dialog as a Bootstrap Modal.
/// Visit <see href="https://getbootstrap.com/docs/5.3/components/modal/">Bootstrap Modal</see> for more information.
/// Full documentation and demos available at <see href="https://havit.blazor.eu/components/HxModal">https://havit.blazor.eu/components/HxModal</see>
/// </summary>
public partial class HxModal : IAsyncDisposable
{
	/// <summary>
	/// Specifies a value for the <c>data-bs-backdrop</c> attribute when <see cref="Backdrop"/> is set to <see cref="ModalBackdrop.Static"/>.
	/// </summary>
	private const string StaticBackdropValue = "static";

	/// <summary>
	/// Application-wide default settings for the <see cref="HxModal"/>.
	/// </summary>
	public static ModalSettings Defaults { get; }

	static HxModal()
	{
		Defaults = new ModalSettings()
		{
			Animated = true,
			ShowCloseButton = true,
			CloseOnEscape = true,
			Size = ModalSize.Regular,
			Fullscreen = ModalFullscreen.Disabled,
			Backdrop = ModalBackdrop.True,
			Scrollable = false,
			Centered = false,
		};
	}

	/// <summary>
	/// Provides application-wide default settings for the component.
	/// Allows descendants to override these defaults with a separate set of values.
	/// </summary>
	protected virtual ModalSettings GetDefaults() => Defaults;

	/// <summary>
	/// A set of settings applied to this component instance. Overrides <see cref="Defaults"/> and is itself overridden by individual parameters.
	/// </summary>
	[Parameter] public ModalSettings Settings { get; set; }

	/// <summary>
	/// Determines whether modals appear without fading in. Setting to <c>false</c> removes the <c>.fade</c> class from the modal markup.
	/// Default value is <c>true</c>.
	/// </summary>
	[Parameter] public bool? Animated { get; set; }
	protected bool AnimatedEffective => Animated ?? GetSettings()?.Animated ?? GetDefaults().Animated ?? throw new InvalidOperationException(nameof(Animated) + " default for " + nameof(HxModal) + " has to be set.");

	/// <summary>
	/// Returns optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> in components descendants (by returning a derived settings class).
	/// </remarks>
	protected virtual ModalSettings GetSettings() => Settings;


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
	protected ModalSize SizeEffective => Size ?? GetSettings()?.Size ?? GetDefaults().Size ?? throw new InvalidOperationException(nameof(Size) + " default for " + nameof(HxModal) + " has to be set.");

	/// <summary>
	/// Fullscreen behavior of the modal. Default is <see cref="ModalFullscreen.Disabled"/>.
	/// </summary>
	[Parameter] public ModalFullscreen? Fullscreen { get; set; }
	protected ModalFullscreen FullscreenEffective => Fullscreen ?? GetSettings()?.Fullscreen ?? GetDefaults().Fullscreen ?? throw new InvalidOperationException(nameof(Fullscreen) + " default for " + nameof(HxModal) + " has to be set.");

	/// <summary>
	/// Determines whether the content is always rendered or only if the modal is open.<br />
	/// The default is <see cref="ModalRenderMode.OpenOnly"/>.<br />
	/// </summary>
	[Parameter] public ModalRenderMode RenderMode { get; set; } = ModalRenderMode.OpenOnly;

	/// <summary>
	/// Indicates whether the modal shows close button in header.
	/// Default value is <c>true</c>.
	/// </summary>
	[Parameter] public bool? ShowCloseButton { get; set; }
	protected bool ShowCloseButtonEffective => ShowCloseButton ?? GetSettings()?.ShowCloseButton ?? GetDefaults().ShowCloseButton ?? throw new InvalidOperationException(nameof(ShowCloseButton) + " default for " + nameof(HxModal) + " has to be set.");

	/// <summary>
	/// Close icon to be used in header.
	/// If set to <c>null</c>, Bootstrap default close-button will be used.
	/// </summary>
	[Parameter] public IconBase CloseButtonIcon { get; set; }
	protected IconBase CloseButtonIconEffective => CloseButtonIcon ?? GetSettings()?.CloseButtonIcon ?? GetDefaults().CloseButtonIcon;

	/// <summary>
	/// Indicates whether the modal closes when escape key is pressed.
	/// Default value is <c>true</c>.
	/// </summary>
	[Parameter] public bool? CloseOnEscape { get; set; }
	protected bool CloseOnEscapeEffective => CloseOnEscape ?? GetSettings()?.CloseOnEscape ?? GetDefaults().CloseOnEscape ?? throw new InvalidOperationException(nameof(CloseOnEscape) + " default for " + nameof(HxModal) + " has to be set.");

	/// <summary>
	/// Indicates whether to apply a backdrop on body while the modal is open.
	/// If set to <see cref="ModalBackdrop.Static"/>, the modal cannot be closed by clicking on the backdrop.
	/// Default value (from <see cref="Defaults"/>) is <see cref="ModalBackdrop.True"/>.
	/// </summary>
	[Parameter] public ModalBackdrop? Backdrop { get; set; }
	protected ModalBackdrop BackdropEffective => Backdrop ?? GetSettings()?.Backdrop ?? GetDefaults().Backdrop ?? throw new InvalidOperationException(nameof(Backdrop) + " default for " + nameof(HxModal) + " has to be set.");

	/// <summary>
	/// Allows scrolling the modal body. Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Scrollable { get; set; }
	protected bool ScrollableEffective => Scrollable ?? GetSettings()?.Scrollable ?? GetDefaults().Scrollable ?? throw new InvalidOperationException(nameof(Scrollable) + " default for " + nameof(HxModal) + " has to be set.");

	/// <summary>
	/// Allows vertical centering of the modal.<br/>
	/// Default is <c>false</c> (horizontal only).
	/// </summary>
	[Parameter] public bool? Centered { get; set; }
	protected bool CenteredEffective => Centered ?? GetSettings()?.Centered ?? GetDefaults().Centered ?? throw new InvalidOperationException(nameof(Centered) + " default for " + nameof(HxModal) + " has to be set.");

	/// <summary>
	/// Additional CSS class for the main element (<c>div.modal</c>).
	/// </summary>
	[Parameter] public string CssClass { get; set; }
	protected string CssClassEffective => CssClass ?? GetSettings()?.CssClass ?? GetDefaults().CssClass;

	/// <summary>
	/// Additional CSS class for the dialog (<c>div.modal-dialog</c> element).
	/// </summary>
	[Parameter] public string DialogCssClass { get; set; }
	protected string DialogCssClassEffective => DialogCssClass ?? GetSettings()?.DialogCssClass ?? GetDefaults().DialogCssClass;

	/// <summary>
	/// Additional header CSS class (<c>div.modal-header</c>).
	/// </summary>
	[Parameter] public string HeaderCssClass { get; set; }
	protected string HeaderCssClassEffective => HeaderCssClass ?? GetSettings()?.HeaderCssClass ?? GetDefaults().HeaderCssClass;

	/// <summary>
	/// Additional body CSS class (<c>div.modal-body</c>).
	/// </summary>
	[Parameter] public string BodyCssClass { get; set; }
	protected string BodyCssClassEffective => BodyCssClass ?? GetSettings()?.BodyCssClass ?? GetDefaults().BodyCssClass;

	/// <summary>
	/// Additional content CSS class (<c>div.modal-content</c>).
	/// </summary>
	[Parameter] public string ContentCssClass { get; set; }
	protected string ContentCssClassEffective => ContentCssClass ?? GetSettings()?.ContentCssClass ?? GetDefaults().ContentCssClass;

	/// <summary>
	/// Additional footer CSS class (<c>div.modal-footer</c>).
	/// </summary>
	[Parameter] public string FooterCssClass { get; set; }
	protected string FooterCssClassEffective => FooterCssClass ?? GetSettings()?.FooterCssClass ?? GetDefaults().FooterCssClass;

	/// <summary>
	/// Fired immediately when the 'hide' instance method is called.
	/// This can be triggered by <see cref="HideAsync"/>, the close button, the <kbd>Esc</kbd> key, or other interactions.
	/// To cancel hiding, set <see cref="ModalHidingEventArgs.Cancel"/> to <c>true</c>.
	/// </summary>
	/// <remarks>
	/// There is intentionally no <c>virtual InvokeOnHidingAsync()</c> method to override to avoid confusion.
	/// The <code>hide.bs.modal</code> event is only subscribed to when the <see cref="OnHiding"/> callback is set.
	/// </remarks>
	[Parameter] public EventCallback<ModalHidingEventArgs> OnHiding { get; set; }

	/// <summary>
	/// Fired when the modal has finished hiding from the user, after CSS transitions complete.
	/// Triggered by <see cref="HideAsync"/>, the close button, the <kbd>Esc</kbd> key, or other interactions.
	/// </summary>
	[Parameter] public EventCallback OnClosed { get; set; }

	/// <summary>
	/// Triggers the <see cref="OnClosed"/> event. Enables derived components to intercept the event.
	/// </summary>
	protected virtual Task InvokeOnClosedAsync() => OnClosed.InvokeAsync();

	/// <summary>
	/// Fired when a modal element becomes visible to the user, after CSS transitions complete.
	/// </summary>
	[Parameter] public EventCallback OnShown { get; set; }

	/// <summary>
	/// Triggers the <see cref="OnShown"/> event. Enables derived components to intercept the event.
	/// </summary>
	protected virtual Task InvokeOnShownAsync() => OnShown.InvokeAsync();


	[Inject] protected IJSRuntime JSRuntime { get; set; }


	private bool _opened = false; // indicates whether the modal is open
	private DotNetObjectReference<HxModal> _dotnetObjectReference;
	private ElementReference _modalElement;
	private IJSObjectReference _jsModule;
	private Queue<Func<Task>> _onAfterRenderTasksQueue = new();
	private bool _disposed;

	public HxModal()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	/// <summary>
	/// Opens the modal.
	/// </summary>
	public Task ShowAsync()
	{
		if (!_opened)
		{
			_onAfterRenderTasksQueue.Enqueue(async () =>
			{
				// Running JS interop is postponed to OnAfterRenderAsync to ensure modalElement is set
				// and correct order of commands (Show/Hide) is preserved
				_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxModal));
				if (_disposed)
				{
					return;
				}

				await _jsModule.InvokeVoidAsync("show", _modalElement, _dotnetObjectReference, CloseOnEscapeEffective, OnHiding.HasDelegate);
			});
		}
		_opened = true; // mark modal as opened

		StateHasChanged(); // ensures rendering modal HTML

		return Task.CompletedTask;
	}

	/// <summary>
	/// Closes the modal.
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
			// Running JS interop is postponed to OnAfterRenderAsync to ensure modalElement is set
			// and correct order of commands (Show/Hide) is preserved
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxModal));
			if (_disposed)
			{
				return;
			}

			await _jsModule.InvokeVoidAsync("hide", _modalElement);
		});
		StateHasChanged(); // enforce rendering

		return Task.CompletedTask;
	}

	/// <summary>
	/// Receives notification from JS for <c>hide.bs.modal</c> event.
	/// </summary>
	[JSInvokable("HxModal_HandleModalHide")]
	public async Task<bool> HandleModalHide()
	{
		var eventArgs = new ModalHidingEventArgs();
		await OnHiding.InvokeAsync(eventArgs);
		return eventArgs.Cancel;
	}

	/// <summary>
	/// Receives notification from JS for <c>hidden.bs.modal</c> event.
	/// </summary>
	[JSInvokable("HxModal_HandleModalHidden")]
	public async Task HandleModalHidden()
	{
		_opened = false;
		await InvokeOnClosedAsync();
		StateHasChanged(); // ensures re-render to remove dialog from HTML
	}

	/// <summary>
	/// Receives notification from JS for <c>shown.bs.modal</c> event.
	/// </summary>
	[JSInvokable("HxModal_HandleModalShown")]
	public async Task HandleModalShown()
	{
		_opened = true;
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
	/// Formats a <see cref="ModalBackdrop"/> value for use in the <c>data-bs-backdrop</c> attribute.
	/// </summary>
	private string GetBackdropSetupValue(ModalBackdrop backdrop)
	{
		return backdrop switch
		{
			ModalBackdrop.Static => StaticBackdropValue,
			ModalBackdrop.True => "true",
			ModalBackdrop.False => "false",
			_ => throw new InvalidOperationException($"Unknown {nameof(ModalBackdrop)} value {BackdropEffective}")
		};
	}

	protected string GetDialogSizeCssClass()
	{
		return SizeEffective switch
		{
			ModalSize.Small => "modal-sm",
			ModalSize.Regular => null,
			ModalSize.Large => "modal-lg",
			ModalSize.ExtraLarge => "modal-xl",
			_ => throw new InvalidOperationException($"Unknown {nameof(ModalSize)} value {SizeEffective}.")
		};
	}

	protected string GetDialogFullscreenCssClass()
	{
		return FullscreenEffective switch
		{
			ModalFullscreen.Disabled => null,
			ModalFullscreen.Always => "modal-fullscreen",
			ModalFullscreen.SmallDown => "modal-fullscreen-sm-down",
			ModalFullscreen.MediumDown => "modal-fullscreen-md-down",
			ModalFullscreen.LargeDown => "modal-fullscreen-lg-down",
			ModalFullscreen.ExtraLargeDown => "modal-fullscreen-xl-down",
			ModalFullscreen.XxlDown => "modal-fullscreen-xxl-down",
			_ => throw new InvalidOperationException($"Unknown {nameof(ModalFullscreen)} value {FullscreenEffective}.")
		};
	}

	protected string GetDialogScrollableCssClass()
	{
		if (ScrollableEffective)
		{
			return "modal-dialog-scrollable";
		}
		return null;
	}

	protected string GetDialogCenteredCssClass()
	{
		if (CenteredEffective)
		{
			return "modal-dialog-centered";
		}
		return null;
	}

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
				await _jsModule.InvokeVoidAsync("dispose", _modalElement, _opened);
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
