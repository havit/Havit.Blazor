using Microsoft.JSInterop;

namespace BlazorAppTest.Shared;

#pragma warning disable BL0016 // JS interop call is not guarded with a try/catch block: Documentation/test app code; JS interop exceptions are intentionally propagated to the caller or to the renderer.

public partial class ColorModeToggle
{
	[Inject] protected IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference _jsModule;

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (firstRender)
		{
			_jsModule = await JSRuntime.ImportModuleAsync("./Shared/ColorModeToggle.razor.js", typeof(ColorMode).Assembly);
		}
	}

	private ValueTask SetColorModeAsync(ColorMode colorMode)
	{
		return _jsModule.InvokeVoidAsync("setTheme", colorMode.ToString("f").ToLowerInvariant());
	}
}
