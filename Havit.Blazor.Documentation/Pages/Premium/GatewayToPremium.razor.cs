using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Pages.Premium;

public partial class GatewayToPremium : IAsyncDisposable
{
	[SupplyParameterFromQuery] public string Url { get; set; }

	[Inject] private NavigationManager NavigationManager { get; set; }
	[Inject] private IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference _jsModule;
	private bool _skipGatewayPage = true;

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

	private MarkupString GenerateHeadContent()
	{
		return (MarkupString)$$"""
			<script>
				if (document.cookie.split(';').some((item) => item.trim() === 'SkipGatewayPage=true')) {
					window.location.href = '{{Url}}'
				}
			</script>
			""";
	}

	public async ValueTask DisposeAsync()
	{
		if (_jsModule != null)
		{
			await _jsModule.DisposeAsync();
		}
	}
}
