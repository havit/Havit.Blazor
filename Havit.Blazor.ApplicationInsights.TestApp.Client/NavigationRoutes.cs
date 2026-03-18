namespace Havit.Blazor.ApplicationInsights.TestApp.Client;

public static class NavigationRoutes
{
	public static class BlazorApplicationInsightsStriptTests
	{
		public const string ServerSideRendering = "/blazorapplicationinsightsscript-ssr";
		public const string InteractiveServer = "/blazorapplicationinsightsscript-interactive-server";
		public const string InteractiveServerPrerendering = "/blazorapplicationinsightsscript-interactive-server-prerendering";
		public const string InteractiveWebAssembly = "/blazorapplicationinsightsscript-interactive-wasm";
		public const string InteractiveWebAssemblyPrerendering = "/blazorapplicationinsightsscript-interactive-wasm-prerendering";
	}

	public const string AppInsightsTestTrackMetric = "/appinsights-test-trackmetric";
	public const string BlazorApplicationInsightsAuthenticationUserContextTests = "/appinsights-test-authentication-user-context";
}
