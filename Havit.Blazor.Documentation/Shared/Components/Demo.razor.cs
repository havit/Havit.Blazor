using Havit.Blazor.Documentation.Pages;
using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Shared.Components;

public partial class Demo : PerformanceLoggingComponentBase
{
	[Parameter] public Type Type { get; set; }
	[Parameter] public bool Tabs { get; set; } = true;
	[Parameter] public string DemoCardCssClass { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private string _code;
	private ElementReference _codeElement;
	private bool _copied;
	private readonly RenderFragment _renderDemo;
	private readonly RenderFragment _renderCode;

	private async Task CopyCodeAsync()
	{
		await JSRuntime.InvokeVoidAsync("copyTextFromElement", _codeElement);
		_copied = true;
		// Fire-and-forget the revert so the click handler returns immediately (an awaited delay would show a spinner on the button).
		_ = RevertCopiedAfterDelayAsync();
	}

	private async Task RevertCopiedAfterDelayAsync()
	{
		await Task.Delay(2000);
		_copied = false;
		await InvokeAsync(StateHasChanged);
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
