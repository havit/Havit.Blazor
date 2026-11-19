using Havit.Blazor.Documentation.Services;
using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Pages.Premium;

#pragma warning disable BL0016 // JS interop call is not guarded with a try/catch block: Documentation/test app code; JS interop exceptions are intentionally propagated to the caller or to the renderer.

public partial class GatewayToPremium(
	IHttpContextProxy httpContextProxy,
	NavigationManager navigationManager,
	IJSRuntime jSRuntime) : IAsyncDisposable
{
	[SupplyParameterFromQuery] public string Url { get; set; }

	private const string SkipGatewayPageCookieEnabledValue = "1";
	private readonly NavigationManager _navigationManager = navigationManager;
	private readonly IJSRuntime _jSRuntime = jSRuntime;
	private readonly IHttpContextProxy _httpContextProxy = httpContextProxy;

	private IJSObjectReference _jsModule;
	private bool _skipGatewayPage = true;

	protected override async Task OnInitializedAsync()
	{
		await RedirectToPremiumContentIfCookieIsSet();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender)
		{
			await RedirectToPremiumContentIfCookieIsSet();
		}
	}

	private async Task RedirectToPremiumContentIfCookieIsSet()
	{
		if (!RendererInfo.IsInteractive)
		{
			if (_httpContextProxy.GetCookieValue("SkipGatewayPage") == SkipGatewayPageCookieEnabledValue)
			{
				_navigationManager.NavigateTo(Url);
			}
		}
		else
		{
			await EnsureJsModuleAsync();
			string skipGatewayPage = await _jsModule.InvokeAsync<string>("getSkipGatewayPage");
			if (skipGatewayPage == SkipGatewayPageCookieEnabledValue)
			{
				_navigationManager.NavigateTo(Url);
			}
		}
	}

	private async Task ContinueToPremiumContent()
	{
		if (_skipGatewayPage)
		{
			await EnsureJsModuleAsync();
			await _jsModule.InvokeVoidAsync("setSkipGatewayPage", SkipGatewayPageCookieEnabledValue);
		}
		_navigationManager.NavigateTo(Url);
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await _jSRuntime.ImportModuleAsync($"./Pages/Premium/{nameof(GatewayToPremium)}.razor.js");
	}

	public async ValueTask DisposeAsync()
	{
		if (_jsModule != null)
		{
			await _jsModule.DisposeAsync();
		}
	}
}
