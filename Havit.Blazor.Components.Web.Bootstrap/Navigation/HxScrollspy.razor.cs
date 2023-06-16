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
	/// Scrollspy additional CSS class. Added to main div (.hx-scrollspy).
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Content to be spied. Elements with IDs are required (corresponding IDs to be used in <see cref="HxNavLink.Href"/>).
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference jsModule;
	private ElementReference scrollspyElement;
	private bool initialized;
	private bool disposed;

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender && !initialized)
		{
			await EnsureJsModuleAsync();
			if (disposed)
			{
				return;
			}
			await jsModule.InvokeVoidAsync("initialize", scrollspyElement, TargetId);
			initialized = true;
		}
	}

	/// <summary>
	/// When using scrollspy in conjunction with adding or removing of elements from the DOM (e.g. asynchronous data load), you’ll need to refresh the scrollspy explicitly.
	/// </summary>
	/// <returns></returns>
	public async Task RefreshAsync()
	{
		if (initialized)
		{
			await EnsureJsModuleAsync();
			if (disposed)
			{
				return;
			}
			await jsModule.InvokeVoidAsync("refresh", scrollspyElement);
		}
		else
		{
			// NOOP - will be initialized OnAfterRenderAsync (a therefor the refresh is not needed)
		}
	}

	private async Task EnsureJsModuleAsync()
	{
		jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxScrollspy));
	}


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
			try
			{
				await jsModule.InvokeVoidAsync("dispose", scrollspyElement);
				await jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
		}
	}
}
