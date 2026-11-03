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
	/// Raised when the item is clicked (before the link scrolls to <see cref="Href"/> and the URL fragment is changed).
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

	private Task<IJSObjectReference> _jsModuleTask;
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
		if (_disposed)
		{
			// OnClick might have removed the link
			return;
		}

		// a single (shared) import, even for clicks while the import is pending
		_jsModuleTask ??= JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxScrollspy)).AsTask();
		var jsModule = await _jsModuleTask;
		if (_disposed)
		{
			// disposed during the import, DisposeAsyncCore could not release the module (disposing it repeatedly is a no-op)
			await DisposeJsModuleAsync(jsModule);
			return;
		}
		await jsModule.InvokeVoidAsync("navigateToFragment", Href);
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		_disposed = true;

		if (_jsModuleTask is { IsCompletedSuccessfully: true })
		{
			await DisposeJsModuleAsync(await _jsModuleTask); // already completed
		}
		// else: a pending import is released by HandleClick once completed
	}

	private static async Task DisposeJsModuleAsync(IJSObjectReference jsModule)
	{
		try
		{
			await jsModule.DisposeAsync();
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
