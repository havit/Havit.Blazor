namespace Havit.Blazor.ApplicationInsights.TestApp.Client;

public static class NavigationRoutes
{
	public static class BlazorApplicationInsightsScriptTests
	{
		public const string ServerSideRendering = "/blazorapplicationinsightsscript-ssr";
		public const string InteractiveServer = "/blazorapplicationinsightsscript-interactive-server";
		public const string InteractiveServerPrerendering = "/blazorapplicationinsightsscript-interactive-server-prerendering";
		public const string InteractiveWebAssembly = "/blazorapplicationinsightsscript-interactive-wasm";
		public const string InteractiveWebAssemblyPrerendering = "/blazorapplicationinsightsscript-interactive-wasm-prerendering";
	}

	public const string AppInsightsTestTrackMetric = "/appinsights-test-trackmetric";
	public const string AuthenticationUserContextTests = "/appinsights-test-authentication-user-context";
	public const string TrackingMethodsTests = "/appinsights-test-tracking-methods";
	public const string TelemetryInitializerTests = "/appinsights-test-telemetry-initializer";

	public static class PageViewTracking
	{
		public const string InitialPageViewTrackingTest = "/appinsights-test-initial-pageview";
		public const string AutoRouteTrackingPage1 = "/appinsights-test-auto-route-tracking-1";
		public const string AutoRouteTrackingPage2 = "/appinsights-test-auto-route-tracking-2";
	}

	public static class Logging
	{
		public const string LoggingTestPage = "/appinsights-test-logging";
	}
}
