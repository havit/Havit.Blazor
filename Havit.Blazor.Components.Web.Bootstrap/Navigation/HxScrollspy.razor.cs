using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/5.3/components/scrollspy/">Bootstrap Scrollspy</see> component.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxScrollspy">https://havit.blazor.eu/components/HxScrollspy</see>
/// </summary>
public partial class HxScrollspy : IAsyncDisposable
{
	/// <summary>
	/// ID of the <see cref="HxNav"/> or list-group with scrollspy navigation.
	/// </summary>
	[Parameter, EditorRequired] public string TargetId { get; set; }

	/// <summary>
	/// Scrollspy additional CSS class. Added to the main div (.hx-scrollspy).
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Content to be spied on. Elements with IDs are required (corresponding IDs to be used in <see cref="HxNavLink.Href"/>).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference _jsModule;
	private ElementReference _scrollspyElement;
	private bool _initialized;
	private bool _disposed;

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender && !_initialized)
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("initialize", _scrollspyElement, TargetId);
			_initialized = true;
		}
	}

	/// <summary>
	/// When using scrollspy in conjunction with adding or removing elements from the DOM (e.g. asynchronous data load), you’ll need to refresh the scrollspy explicitly.
	/// </summary>
	/// <returns></returns>
	public async Task RefreshAsync()
	{
		if (_initialized)
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}
			await _jsModule.InvokeVoidAsync("refresh", _scrollspyElement);
		}
		else
		{
			// NOOP - will be initialized OnAfterRenderAsync (and therefore the refresh is not needed)
		}
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxScrollspy));
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
				await _jsModule.InvokeVoidAsync("dispose", _scrollspyElement);
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
	}
}
