using Microsoft.JSInterop;

namespace Havit.Blazor.Documentation.Pages.Premium;

public partial class GatewayToGitHub
{
	[SupplyParameterFromQuery] public string Url { get; set; }

	[Inject] private NavigationManager NavigationManager { get; set; }
	[Inject] private IJSRuntime JSRuntime { get; set; }

	private IJSObjectReference _jsModule;

	private bool _skipGatewayPage = false;
	private string _gitHubFolderUrl = "https://github.com/havit/Havit.Blazor.Premium";

	protected override void OnParametersSet()
	{
		_gitHubFolderUrl = Uri.UnescapeDataString(Url);
	}

	private void LearnMoreAboutPremium()
	{
		NavigationManager.NavigateTo("/premium");
	}

	private async Task ContinueToGitHub()
	{
		if (_skipGatewayPage)
		{
			await EnsureJsModuleAsync();
			await _jsModule.InvokeVoidAsync("setSkipGatewayPage", true);
		}
		NavigationManager.NavigateTo(_gitHubFolderUrl);
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportModuleAsync($"./Pages/Premium/{nameof(GatewayToGitHub)}.razor.js");
	}

	private MarkupString GenerateHeadContent()
	{
		return (MarkupString)$@"<script>
					 if (document.cookie.split(';').some((item) => item.trim() === 'SkipGatewayPage=true')) {{
					 	 window.location.href = '{_gitHubFolderUrl}'
					 }}
				  </script>";
	}
}
