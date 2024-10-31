namespace Havit.Blazor.Documentation.Pages.Premium;

public partial class GatewayToGithub
{
	[SupplyParameterFromQuery] public string Url { get; set; }
	[Inject] private NavigationManager NavigationManager { get; set; }

	private bool _skipGatewayPage = false;

	private void LearnMoreAboutPremium()
	{
		NavigationManager.NavigateTo("/premium");
	}

	private void ContinueToGitHub()
	{
		if (_skipGatewayPage)
		{
			// TODO: save preference into a cookie and redirect user right away if the cookie is present
		}

		var unescaped = Uri.UnescapeDataString(Url);
		NavigationManager.NavigateTo(unescaped);
	}
}
