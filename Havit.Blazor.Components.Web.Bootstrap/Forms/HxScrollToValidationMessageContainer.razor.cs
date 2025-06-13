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
	private ElementReference wrapperRef; // Messages are found inside this element.
	private bool scrollOnNextRender;

	private CancellationTokenSource _disposeCts = new(); // Used to cancel JS interop.

	protected override void OnInitialized()
	{
		EditContext.OnValidationStateChanged += HandleValidationStateChanged;
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxScrollToValidationMessageContainer));
		}

		if (scrollOnNextRender)
		{
			await _jsModule.InvokeVoidAsync("tryScrollToFirstError", _disposeCts.Token, wrapperRef, ValidationMessageClass);
			scrollOnNextRender = false;
		}
	}

	private void HandleValidationStateChanged(object sender, ValidationStateChangedEventArgs e)
	{
		scrollOnNextRender = true;
	}

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	private async ValueTask DisposeAsyncCore()
	{
		EditContext.OnValidationStateChanged -= HandleValidationStateChanged;

		await _disposeCts.CancelAsync();
		_disposeCts.Dispose();

		if (_jsModule is not null)
		{
			await _jsModule.DisposeAsync();
		}
	}
}