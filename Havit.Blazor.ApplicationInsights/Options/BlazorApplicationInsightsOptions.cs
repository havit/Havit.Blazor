using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights.Options;

/// <summary>
/// Top-level options for the Blazor Application Insights integration.
/// </summary>
/// <remarks>
/// Combines two distinct concerns:
/// <list type="bullet">
///   <item><description>
///     <see cref="JsSdkOptions"/> — settings serialized to JSON and forwarded to the
///     Application Insights JavaScript SDK during initialization.
///   </description></item>
///   <item><description>
///     C# wrapper-level settings (e.g. <see cref="EnableInitialPageViewTracking"/>) —
///     control Blazor-side behavior and are never sent to the JS SDK.
///   </description></item>
/// </list>
/// </remarks>
public record class BlazorApplicationInsightsOptions // record for easy cloning by with { }
{
	/// <summary>
	/// Configuration options passed to the Application Insights JavaScript SDK.
	/// These are serialized to JSON and injected into the SDK initialization snippet.
	/// </summary>
	public BlazorApplicationInsightsJsSdkOptions JsSdkOptions { get; set; } = new BlazorApplicationInsightsJsSdkOptions();

	/// <summary>
	/// When <c>true</c>, the SDK snippet automatically calls <c>trackPageView({})</c> on startup.
	/// <para>
	/// Set to <c>false</c> when you need full control over page-view tracking — for example, to track
	/// only after the user is authenticated, or to include custom properties in every page view.
	/// </para>
	/// Default: <c>true</c>.
	/// </summary>
	public bool EnableInitialPageViewTracking { get; set; } = true;

	/// <summary>
	/// Gets or sets the default telemetry initializer used to customize telemetry data before it is sent.
	/// </summary>
	public TelemetryInitializer DefaultTelemetryInitializer { get; set; }
}
