using Havit.Blazor.Documentation.Pages;
using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Shared.Components;

public partial class Demo : PerformanceLoggingComponentBase, IDisposable
{
	[Parameter] public Type Type { get; set; }
	[Parameter] public bool Tabs { get; set; } = true;
	[Parameter] public string DemoCardCssClass { get; set; }

	/// <summary>
	/// Wraps the demo in a horizontally resizable container so the reader can drag the lower-right
	/// corner to preview container-query driven responsive behavior (e.g. <c>HxListLayout</c>).
	/// </summary>
	[Parameter] public bool Resizable { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private string _code;
	private ElementReference _codeElement;
	private bool _copied;
	private CancellationTokenSource _copiedCts;
	private readonly RenderFragment _renderDemo;
	private readonly RenderFragment _renderCode;

	private async Task CopyCodeAsync()
	{
		await JSRuntime.InvokeVoidAsync("copyTextFromElement", _codeElement);

		// Cancel any in-flight revert before starting a new one.
		if (_copiedCts is not null)
		{
			await _copiedCts.CancelAsync();
			_copiedCts.Dispose();
		}
		_copiedCts = new CancellationTokenSource();

		_copied = true;
		// Fire-and-forget the revert so the click handler returns immediately (an awaited delay would show a spinner on the button).
		_ = RevertCopiedAfterDelayAsync(_copiedCts.Token);
	}

	private async Task RevertCopiedAfterDelayAsync(CancellationToken cancellationToken)
	{
		try
		{
			await Task.Delay(2000, cancellationToken);
		}
		catch (OperationCanceledException)
		{
			return;
		}
		_copied = false;
		await InvokeAsync(StateHasChanged);
	}

	public void Dispose()
	{
		_copiedCts?.Cancel();
		_copiedCts?.Dispose();
	}

	private readonly CardSettings _cardSettings = new()
	{
		CssClass = "card-demo",
		BodyCssClass = "p-0"
	};

	public Demo()
	{
		_renderDemo = RenderDemo;
		_renderCode = RenderCode;
	}

	protected override async Task OnParametersSetAsync()
	{
		if (!Tabs)
		{
			await LoadCodeAsync();
		}
	}

	private async Task LoadCodeAsync()
	{
		if (_code is null)
		{
			var resourceName = Type.FullName + ".razor";

			using (Stream stream = Type.Assembly.GetManifestResourceStream(resourceName))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					_code = await reader.ReadToEndAsync();
				}
			}
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await JSRuntime.InvokeVoidAsync("highlightCode");
	}
}
