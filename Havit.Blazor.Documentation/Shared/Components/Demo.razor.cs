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
	private readonly RenderFragment _renderDemo;
	private readonly RenderFragment _renderCode;
	private readonly CardSettings _cardSettings = new()
	{
		CssClass = "card-demo my-3",
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
