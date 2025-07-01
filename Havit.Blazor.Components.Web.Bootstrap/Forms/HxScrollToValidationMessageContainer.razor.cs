using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// When used in an <see cref="EditForm"/> (or given an EditContext),
/// when validation messages appear this component will scroll the page to the first validation error message.
/// When using Hx input components, like <see cref="HxInputText"/> or <see cref="HxInputNumber"/> etc., the scrolling works automatically.
/// When used with custom components, make sure you wrap the component in <see cref="HxValidationScrollableGroup"/>.
/// <br/><br/> See also <a href="https://developer.mozilla.org/en-US/docs/Web/CSS/scroll-margin">css scroll margins</a>.
/// </summary>
public partial class HxScrollToValidationMessageContainer : IAsyncDisposable
{
	[CascadingParameter] public EditContext EditContext { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// Class by which to search for validation error messages in DOM
	/// so we can scroll to them.
	/// </summary>
	[Parameter] public string ValidationMessageClass { get; set; } = HxInputBase<object>.InvalidCssClass;

	[Inject] protected IJSRuntime JSRuntime { get; set; }
	private IJSObjectReference _jsModule;

	/// <summary>
	/// Validation message elements shall be found as nested children of this element.
	/// </summary>
	private ElementReference messagesWrapperRef;
	private bool scrollOnNextRender;

	private CancellationTokenSource _disposeCts = new(); // Used to cancel JS interop.

	protected override void OnInitialized()
	{
		EditContext.OnValidationStateChanged += HandleValidationStateChanged;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (scrollOnNextRender)
		{
			// Load module (if needed)
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxScrollToValidationMessageContainer));

			// Try scroll
			await _jsModule.InvokeVoidAsync("tryScrollToFirstError", _disposeCts.Token, messagesWrapperRef, ValidationMessageClass);
			scrollOnNextRender = false;
		}
	}

	private void HandleValidationStateChanged(object sender, ValidationStateChangedEventArgs e)
	{
		scrollOnNextRender = true;
		StateHasChanged(); // <- Maybe unnecessary, but i feel like rendering isn't guaranteed here
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		EditContext.OnValidationStateChanged -= HandleValidationStateChanged;

		await _disposeCts.CancelAsync();
		_disposeCts.Dispose();

		if (_jsModule is not null)
		{
			try
			{
				await _jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
		}
	}
}