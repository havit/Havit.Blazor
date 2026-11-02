using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Temporary (?) NavLink component to be used with <see cref="HxScrollspy"/> where <c>#id</c> anchors are required and <c>page-route#id</c> cannot be used.<br />
/// When clicked, it scrolls only the scrollable container of the target element (not the whole page) and updates the URL fragment (without adding a browser history entry).
/// </summary>
public partial class HxScrollspyNavLink : IAsyncDisposable
{
	/// <summary>
	/// The navigation target in <c>#id</c> form.
	/// </summary>
	[Parameter, EditorRequired] public string Href { get; set; }

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Raised when the item is clicked (before the link scrolls to <see cref="Href"/> and the URL fragment is changed).<br />
	/// When <see cref="HxScrollspy.SmoothScroll"/> is enabled, Bootstrap handles the click on its own as well and starts the smooth scroll before the callback is invoked.
	/// </summary>
	[Parameter] public EventCallback<MouseEventArgs> OnClick { get; set; }
	/// <summary>
	/// Triggers the <see cref="OnClick"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeOnClickAsync(MouseEventArgs args) => OnClick.InvokeAsync(args);

	/// <summary>
	/// Additional attributes to be splatted onto an underlying <c>&lt;a&gt;</c> element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference _jsModule;
	private bool _disposed;

	protected override void OnParametersSet()
	{
		Contract.Requires<InvalidOperationException>((Href is not null) && (Href.StartsWith("#")), $"{nameof(HxScrollspyNavLink)}.{nameof(Href)} has to start with #. Use only for local elements.");
	}

	private async Task HandleClick(MouseEventArgs args)
	{
		await InvokeOnClickAsync(args);

		// NavigationManager.NavigateTo() is not used: for the same page, Blazor scrolls the target element into view by scrollIntoView(),
		// which scrolls all the scrollable ancestors, including the whole page.
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxScrollspy));
		if (_disposed)
		{
			return;
		}
		await _jsModule.InvokeVoidAsync("navigateToFragment", Href);
	}

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
