using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Extension methods for <see cref="IBlazorApplicationInsights"/> to simplify common telemetry operations.
/// </summary>
public static class BlazorApplicationInsightsExtensions
{
	/// <summary>
	/// Tracks a C# <see cref="Exception"/> as Application Insights exception telemetry.
	/// </summary>
	public static Task TrackExceptionAsync(this IBlazorApplicationInsights blazorApplicationInsights, Exception exception, SeverityLevel? severityLevel = null, Dictionary<string, object> customProperties = null)
	{
		var telemetry = new ExceptionTelemetry
		{
			Exception = ExceptionDetails.FromException(exception),
			SeverityLevel = severityLevel
		};

		return blazorApplicationInsights.TrackExceptionAsync(telemetry, customProperties);
	}

	/// <summary>
	/// Starts timing a named event and returns a <see cref="TrackedEventScope"/> that stops timing when disposed.
	/// Calls <see cref="IBlazorApplicationInsights"/>.<c>StartTrackEventAsync</c> immediately and
	/// <see cref="IBlazorApplicationInsights"/>.<c>StopTrackEventAsync</c> on dispose.
	/// </summary>
	/// <remarks>
	/// Use with <c>await using</c> to ensure the event is always stopped — even when an exception occurs.
	/// Set <see cref="TrackedEventScope.Properties"/> and <see cref="TrackedEventScope.Measurements"/>
	/// on the returned scope to attach data to the event.
	/// </remarks>
	public static async Task<TrackedEventScope> EnterTrackEventScopeAsync(this IBlazorApplicationInsights blazorApplicationInsights, string name)
	{
		await blazorApplicationInsights.StartTrackEventAsync(name);
		return new TrackedEventScope(blazorApplicationInsights, name);
	}

	/// <summary>
	/// Starts timing a named page view and returns a <see cref="TrackedPageScope"/> that stops timing when disposed.
	/// Calls <see cref="IBlazorApplicationInsights"/>.<c>StartTrackPageAsync</c> immediately and
	/// <see cref="IBlazorApplicationInsights"/>.<c>StopTrackPageAsync</c> on dispose.
	/// </summary>
	/// <remarks>
	/// Use with <c>await using</c> to ensure the page view is always stopped — even when an exception occurs.
	/// Set <see cref="TrackedPageScope.Url"/>, <see cref="TrackedPageScope.Properties"/>, and
	/// <see cref="TrackedPageScope.Measurements"/> on the returned scope to attach data to the page view.
	/// </remarks>
	public static async Task<TrackedPageScope> EnterTrackPageScopeAsync(this IBlazorApplicationInsights blazorApplicationInsights, string name = null)
	{
		await blazorApplicationInsights.StartTrackPageAsync(name);
		return new TrackedPageScope(blazorApplicationInsights, name);
	}
}
