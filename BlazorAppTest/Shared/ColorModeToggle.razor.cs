using Microsoft.JSInterop;

namespace BlazorAppTest.Shared;

public partial class ColorModeToggle
{
	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference jsModule;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender)
		{
			jsModule = await JSRuntime.ImportModuleAsync("./Shared/ColorModeToggle.razor.js", typeof(ColorMode).Assembly);
		}
	}

	private ValueTask SetColorModeAsync(ColorMode colorMode)
	{
		return jsModule.InvokeVoidAsync("setTheme", colorMode.ToString("f").ToLowerInvariant());
	}
}
