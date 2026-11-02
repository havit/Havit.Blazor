using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see href="https://getbootstrap.com/docs/6.0/components/scrollspy/">Bootstrap Scrollspy</see> that automatically highlights the navigation link matching the section currently scrolled into view.<br />
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

	/// <summary>
	/// Position of the activation line, measured from the top of the scroll container.
	/// A section becomes active once its top scrolls above this line.
	/// Accepts a percentage (e.g. <c>12%</c>) or pixels (e.g. <c>96px</c>, to sit below a sticky header).<br />
	/// Ignored when <see cref="RootMargin"/> is set.<br />
	/// Default is <c>null</c> (Bootstrap default <c>12%</c> is used).
	/// </summary>
	[Parameter] public string TopMargin { get; set; }

	/// <summary>
	/// Advanced override for the <c>IntersectionObserver</c> <see href="https://developer.mozilla.org/en-US/docs/Web/API/IntersectionObserver/rootMargin">rootMargin</see> (e.g. <c>0px 0px -40%</c>).
	/// When set, it takes precedence over <see cref="TopMargin"/> and is passed straight to the observer.<br />
	/// Default is <c>null</c> (the activation line is derived from <see cref="TopMargin"/>).
	/// </summary>
	[Parameter] public string RootMargin { get; set; }

	/// <summary>
	/// <c>IntersectionObserver</c> <see href="https://developer.mozilla.org/en-US/docs/Web/API/IntersectionObserver/IntersectionObserver#threshold">threshold</see> values (between <c>0</c> and <c>1</c>) used when calculating the scroll position.<br />
	/// Default is <c>null</c> (Bootstrap default <c>[0]</c> is used).
	/// </summary>
	[Parameter] public IReadOnlyList<double> Threshold { get; set; }

	/// <summary>
	/// Enables smooth scrolling when the user clicks a navigation link that refers to a spied section.
	/// Once the scroll settles, the URL fragment and focus are moved to the target section.<br />
	/// Respects the <c>prefers-reduced-motion</c> user preference.<br />
	/// Default is <c>false</c>.
	/// </summary>
	[Parameter] public bool SmoothScroll { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference _jsModule;
	private ElementReference _scrollspyElement;
	private bool _initialized;
	private bool _disposed;
	private Dictionary<string, object> _lastJsOptions;

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
			_lastJsOptions = GetJsOptions();
			await _jsModule.InvokeVoidAsync("initialize", _scrollspyElement, TargetId, _lastJsOptions);
			_initialized = true;
		}
		else if (_initialized)
		{
			// Bootstrap ScrollSpy reads its configuration only when created, so it has to be recreated when the options change.
			var jsOptions = GetJsOptions();
			if (!AreJsOptionsEqual(jsOptions, _lastJsOptions))
			{
				await EnsureJsModuleAsync();
				if (_disposed)
				{
					return;
				}
				_lastJsOptions = jsOptions;
				await _jsModule.InvokeVoidAsync("reinitialize", _scrollspyElement, TargetId, _lastJsOptions);
			}
		}
	}

	private Dictionary<string, object> GetJsOptions()
	{
		var options = new Dictionary<string, object>
		{
			["smoothScroll"] = SmoothScroll
		};
		if (!String.IsNullOrWhiteSpace(TopMargin))
		{
			options["topMargin"] = TopMargin;
		}
		if (!String.IsNullOrWhiteSpace(RootMargin))
		{
			options["rootMargin"] = RootMargin;
		}
		if (Threshold is not null)
		{
			options["threshold"] = Threshold.ToArray();
		}
		return options;
	}

	private static bool AreJsOptionsEqual(Dictionary<string, object> options1, Dictionary<string, object> options2)
	{
		if ((options1 is null) || (options2 is null))
		{
			return options1 == options2;
		}

		return (options1.Count == options2.Count)
			&& options1.All(item => options2.TryGetValue(item.Key, out var value2)
				&& ((item.Value is double[] array1 && value2 is double[] array2)
					? array1.SequenceEqual(array2)
					: Object.Equals(item.Value, value2)));
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
