namespace Havit.Blazor.ApplicationInsights.TestApp.Client;

public static class NavigationRoutes
{
	public static class BlazorApplicationInsightsScriptTests
	{
		private const string Base = "/blazorapplicationinsightsscript";
		public const string RouteTemplate = Base + "/{mode}";
		public const string ServerSideRendering = Base + "/ssr";
		public const string InteractiveServer = Base + "/interactive-server";
		public const string InteractiveServerPrerendering = Base + "/interactive-server-prerendering";
		public const string InteractiveWebAssembly = Base + "/interactive-wasm";
		public const string InteractiveWebAssemblyPrerendering = Base + "/interactive-wasm-prerendering";
	}

	public static class DefaultTelemetryInitializerTests
	{
		private const string Base = "/appinsights-test-default-telemetry-initializer";
		public const string RouteTemplate = Base + "/{mode}";
		public const string ServerSideRendering = Base + "/ssr";
		public const string InteractiveServer = Base + "/interactive-server";
		public const string InteractiveServerPrerendering = Base + "/interactive-server-prerendering";
		public const string InteractiveWebAssembly = Base + "/interactive-wasm";
		public const string InteractiveWebAssemblyPrerendering = Base + "/interactive-wasm-prerendering";
	}
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
