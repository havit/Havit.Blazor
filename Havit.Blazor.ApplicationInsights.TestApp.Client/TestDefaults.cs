namespace Havit.Blazor.ApplicationInsights.TestApp.Client;

public static class TestDefaults
{
	public const string DefaultTelemetryInitializerCloudRoleName = "test-default-role";

	/// <summary>
	/// Names of the sentinel events the test pages track right before they flush.
	/// A test asserting that some telemetry was NOT sent waits for the sentinel first:
	/// once the sentinel is on the wire, anything tracked before it would be on the wire too.
	/// </summary>
	public static class SentinelEvents
	{
		public const string InitialPageViewTrackingPageDone = "sentinel-initial-page-view-tracking-done";
		public const string AutoRouteTrackingPage2Done = "sentinel-auto-route-tracking-page2-done";
	}
}
