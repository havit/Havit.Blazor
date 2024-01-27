using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/toasts/">Bootstrap Toast</see> component. Not intended to be used in user code, use <see cref="HxMessenger"/>.
/// After the first render, the component never updates.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxToast">https://havit.blazor.eu/components/HxToast</see>
/// </summary>
public partial class HxToast : ComponentBase, IAsyncDisposable
{
	/// <summary>
	/// Color scheme.
	/// </summary>
	[Parameter] public ThemeColor? Color { get; set; }

	/// <summary>
	/// Delay in milliseconds to automatically hide the toast.
	/// </summary>
	[Parameter] public int? AutohideDelay { get; set; }

	/// <summary>
	/// CSS class to render with the toast.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Header icon.
	/// </summary>
	[Parameter] public IconBase HeaderIcon { get; set; }

	/// <summary>
	/// Header text.
	/// </summary>
	[Parameter] public string HeaderText { get; set; }

	/// <summary>
	/// Header template.
	/// </summary>
	[Parameter] public RenderFragment HeaderTemplate { get; set; }

	/// <summary>
	/// Content (body) text.
	/// </summary>
	[Parameter] public string ContentText { get; set; }

	/// <summary>
	/// Content (body) template.
	/// </summary>
	[Parameter] public RenderFragment ContentTemplate { get; set; }

	/// <summary>
	/// Indicates whether to show the close button.
	/// </summary>
	[Parameter] public bool ShowCloseButton { get; set; } = true;

	/// <summary>
	/// Fires when the toast is hidden (button or autohide).
	/// </summary>
	[Parameter] public EventCallback OnToastHidden { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnToastHidden"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnToastHiddenAsync() => OnToastHidden.InvokeAsync();

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private ElementReference _toastElement;
	private DotNetObjectReference<HxToast> _dotnetObjectReference;
	private IJSObjectReference _jsModule;
	private bool _disposed;

	public HxToast()
	{
		_dotnetObjectReference = DotNetObjectReference.Create(this);
	}

	/// <inheritdoc />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenRegion(0);
		base.BuildRenderTree(builder);
		builder.CloseRegion();

		bool renderHeader = !String.IsNullOrEmpty(HeaderText) || (HeaderTemplate != null) || (HeaderIcon != null);
		bool renderContent = !String.IsNullOrEmpty(ContentText) || (ContentTemplate != null) || (ShowCloseButton && !renderHeader);

		builder.OpenElement(100, "div");
		builder.AddAttribute(101, "role", "alert");
		builder.AddAttribute(102, "aria-live", "assertive");
		builder.AddAttribute(103, "aria-atomic", "true");
		builder.AddAttribute(104, "class", CssClassHelper.Combine("toast", Color?.ToBackgroundColorCss(), HasContrastColor() ? "text-white" : "text-dark", CssClass));

		if (AutohideDelay != null)
		{
			builder.AddAttribute(110, "data-bs-delay", AutohideDelay);
		}
		else
		{
			builder.AddAttribute(111, "data-bs-autohide", "false");
		}
		builder.AddElementReferenceCapture(120, referenceCapture => _toastElement = referenceCapture);

		if (renderHeader)
		{
			builder.OpenElement(200, "div");
			builder.AddAttribute(201, "class", "toast-header");

			if (HeaderIcon != null)
			{
				builder.OpenComponent(202, typeof(HxIcon));
				builder.AddAttribute(203, nameof(HxIcon.Icon), HeaderIcon);
				builder.AddAttribute(204, nameof(HxIcon.CssClass), "me-2");
				builder.CloseComponent();
			}

			if (!String.IsNullOrWhiteSpace(HeaderText))
			{
				builder.OpenElement(205, "strong");
				builder.AddAttribute(206, "class", "me-auto");
				builder.AddContent(207, HeaderText);
				builder.CloseElement(); // strong
			}
			builder.AddContent(208, HeaderTemplate);

			if (ShowCloseButton)
			{
				builder.OpenRegion(209);
				if (HasContrastColor())
				{
					builder.AddAttribute(210, "data-bs-theme", "dark");
				}
				BuildRenderTree_CloseButton(builder, "ms-auto");
				builder.CloseRegion();
			}
			builder.CloseElement(); // toast-header				

		}

		if (renderContent)
		{
			builder.OpenElement(300, "div");
			if (!renderHeader && ShowCloseButton)
			{
				builder.AddAttribute(301, "class", "d-flex");
			}
			builder.OpenElement(302, "div");
			builder.AddAttribute(303, "class", "toast-body");
			builder.AddContent(304, ContentText);
			builder.AddContent(305, ContentTemplate);
			builder.CloseElement(); // toast-body

			if (!renderHeader && ShowCloseButton)
			{
				builder.OpenRegion(306);
				builder.OpenElement(307, "div");
				builder.AddAttribute(308, "class", "me-3 m-auto");
				if (HasContrastColor())
				{
					builder.AddAttribute(309, "data-bs-theme", "dark");
				}
				BuildRenderTree_CloseButton(builder, null);
				builder.CloseElement();
				builder.CloseRegion();
			}

			builder.CloseElement();
		}

		builder.CloseElement(); // toast
	}

	private void BuildRenderTree_CloseButton(RenderTreeBuilder builder, string cssClass = null)
	{
		builder.OpenElement(100, "button");
		builder.AddAttribute(101, "type", "button");
		builder.AddAttribute(102, "class", CssClassHelper.Combine("btn-close", cssClass));
		builder.AddAttribute(103, "data-bs-dismiss", "toast");
		builder.CloseElement(); // button
	}

	private bool HasContrastColor()
	{
		return Color switch
		{
			null => false,
			ThemeColor.Primary => true,
			ThemeColor.Secondary => true,
			ThemeColor.Success => true,
			ThemeColor.Danger => true,
			ThemeColor.Warning => false,
			ThemeColor.Info => false,
			ThemeColor.Light => false,
			ThemeColor.Dark => true,
			_ => throw new InvalidOperationException($"Unknown {nameof(Color)}: {Color}")
		};
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender)
		{
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxToast));
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("show", _toastElement, _dotnetObjectReference);
		}
	}

	protected override bool ShouldRender()
	{
		// never update content to avoid collision with JavaScript
		return false;
	}

	/// <summary>
	/// Receive notification from JavaScript when message is hidden.
	/// </summary>
	[JSInvokable("HxToast_HandleToastHidden")]
	public async Task HandleToastHidden()
	{
		await InvokeOnToastHiddenAsync();
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
				await _jsModule.InvokeVoidAsync("dispose", _toastElement);
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
