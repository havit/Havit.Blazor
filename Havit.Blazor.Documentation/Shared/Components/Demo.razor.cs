using Havit.Blazor.Documentation.Pages;
using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Shared.Components;

public partial class Demo : PerformanceLoggingComponentBase
{
	[Parameter] public Type Type { get; set; }

	[Parameter] public bool Tabs { get; set; } = true;

	[Parameter] public string DemoCardCssClass { get; set; }

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private bool showingDemo = true;
	private string code;

	protected override async Task OnParametersSetAsync()
	{
		if (!Tabs)
		{
			await LoadCodeAsync();
		}
	}

	private async Task LoadCodeAsync()
	{
		if (code is null)
		{
			var resourceName = Type.FullName + ".razor";

			using (Stream stream = Type.Assembly.GetManifestResourceStream(resourceName))
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					code = await reader.ReadToEndAsync();
				}
			}
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await JSRuntime.InvokeVoidAsync("highlightCode");
	}

	private Task HandleDemoTabActivated()
	{
		showingDemo = true;
		return Task.CompletedTask;
	}

	private async Task HandleCodeTabActivated()
	{
		showingDemo = false;
		await LoadCodeAsync();
	}
}
