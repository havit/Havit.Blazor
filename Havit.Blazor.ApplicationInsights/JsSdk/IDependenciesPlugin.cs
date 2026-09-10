// Based on the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/extensions/applicationinsights-dependencies-js/src/IDependenciesPlugin.ts

using System.ComponentModel;
using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights.JsSdk;

/// <summary>
/// Dependency tracking interface.
/// C# representation of the JavaScript <c>IDependenciesPlugin</c> interface
/// from <c>@microsoft/applicationinsights-dependencies-js</c>.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IDependenciesPlugin
{
	/// <summary>
	/// Manually tracks an outgoing dependency call (HTTP request, SQL query, or other external call)
	/// that was not captured automatically by the JavaScript SDK.
	/// </summary>
	Task TrackDependencyDataAsync(DependencyTelemetry dependency);
}
