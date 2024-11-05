using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Pages.Premium;

public partial class GatewayToPremium : IAsyncDisposable
{
	[SupplyParameterFromQuery] public string Url { get; set; }

	[Inject] private NavigationManager NavigationManager { get; set; }
	[Inject] private IJSRuntime JSRuntime { get; set; }

	[CascadingParameter] private HttpContext HttpContext { get; set; }

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
			if (HttpContext.Request.Cookies.Any(c => c.Key.StartsWith("﻿SkipGatewayPage")))
			{
				NavigationManager.NavigateTo(Url);
			}
		}
		else
		{
			await EnsureJsModuleAsync();
			string skipGatewayPage = await _jsModule.InvokeAsync<string>("getSkipGatewayPage");
			if (!string.IsNullOrEmpty(skipGatewayPage))
			{
				NavigationManager.NavigateTo(Url);
			}
		}
	}

	private async Task ContinueToPremiumContent()
	{
		if (_skipGatewayPage)
		{
			await EnsureJsModuleAsync();
			await _jsModule.InvokeVoidAsync("setSkipGatewayPage", true);
		}
		NavigationManager.NavigateTo(Url);
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportModuleAsync($"./Pages/Premium/{nameof(GatewayToPremium)}.razor.js");
	}

	public async ValueTask DisposeAsync()
	{
		if (_jsModule != null)
		{
			await _jsModule.DisposeAsync();
		}
	}
}
