using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Contextual feedback message (success, warning, danger, info, and more) with an optional icon, links, and a dismiss button.<br />
/// Renders the <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/alert/">Bootstrap alert</see> markup.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxAlert">https://havit.blazor.eu/components/HxAlert</see>
/// </summary>
public partial class HxAlert : IAsyncDisposable
{
	/// <summary>
	/// Content of the alert.
	/// The alert is a flex container - wrap mixed inline content (text with links etc.) in a <c>p</c> element.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Alert color.
	/// Default is <see cref="ThemeColor.None"/>, which renders the neutral (default) Bootstrap alert.
	/// </summary>
	[Parameter] public ThemeColor Color { get; set; }

	/// <summary>
	/// Shows the Close button and allows dismissing the alert.
	/// </summary>
	[Parameter] public bool Dismissible { get; set; }

	/// <summary>
	/// Fired when the alert has been dismissed by the Close button (<see cref="Dismissible"/>) and removed from the page (Bootstrap <c>closed.bs.alert</c> event).
	/// Use the callback to stop rendering the alert and to move focus to an appropriate element (the focused Close button is gone).
	/// </summary>
	[Parameter] public EventCallback OnClosed { get; set; }

	/// <summary>
	/// Triggers the <see cref="OnClosed"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClosedAsync() => OnClosed.InvokeAsync();

	/// <summary>
	/// Any additional CSS class to apply.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private ElementReference _alertElement;
	private DotNetObjectReference<HxAlert> _dotnetObjectReference;
	private IJSObjectReference _jsModule;
	private bool _jsInitialized;
	private bool _disposed;

	/// <inheritdoc cref="ComponentBase.OnAfterRenderAsync(bool)" />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		// The dismissal itself is handled by the Bootstrap data API (data-bs-dismiss="alert"),
		// JS keeps the closed (removed) element in the DOM for Blazor and raises OnClosed.
		if (!_jsInitialized && Dismissible)
		{
			_jsInitialized = true;
			_dotnetObjectReference ??= DotNetObjectReference.Create(this);
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxAlert));
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("initialize", _alertElement, _dotnetObjectReference);
		}
	}

	/// <summary>
	/// Receives notification from JavaScript when the alert has been closed.
	/// </summary>
	[JSInvokable("HxAlert_HandleJsClosed")]
	public async Task HandleJsClosed()
	{
		await InvokeOnClosedAsync();
	}

	public string GetColorCss()
	{
		return Color switch
		{
			ThemeColor.None => null,
			ThemeColor.Link => throw new NotSupportedException($"{nameof(ThemeColor)}.{nameof(ThemeColor.Link)} cannot be used as {nameof(HxAlert)} color."),
			_ => Color.ToThemeCss()
		};
	}

	/// <inheritdoc/>
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		_disposed = true;

		if (_jsModule != null)
		{
			try
			{
				await _jsModule.InvokeVoidAsync("dispose", _alertElement);
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

		_dotnetObjectReference?.Dispose();
	}
}
