namespace Havit.Blazor.Documentation;

public static class NavigationRoutes
{
	public static class Premium
	{
		public const string GatewayPage = "/premium/access-content";
		public static string GetGatewayPage(string targetUrl)
		{
			return GatewayPage + "?url=" + Uri.EscapeDataString(targetUrl);
		}
	}
}
