using Havit.Blazor.ApplicationInsights.Telemetry;

namespace Havit.Blazor.ApplicationInsights;

public static class BlazorApplicationInsightsExtensions
{
	/// <summary>
	/// Tracks a C# <see cref="Exception"/> as Application Insights exception telemetry.
	/// </summary>
	public static Task TrackExceptionAsync(this IBlazorApplicationInsights appInsights, Exception exception, SeverityLevel? severityLevel = null, Dictionary<string, object> customProperties = null)
	{
		var telemetry = new ExceptionTelemetry
		{
			Exception = ExceptionDetails.FromException(exception),
			SeverityLevel = severityLevel
		};

		return appInsights.TrackExceptionAsync(telemetry, customProperties);
	}

	/// <summary>
	/// Starts timing a named event and returns a <see cref="TrackedEventScope"/> that stops timing when disposed.
	/// Calls <see cref="IBlazorApplicationInsights.StartTrackEventAsync"/> immediately and
	/// <see cref="IBlazorApplicationInsights.StopTrackEventAsync"/> on dispose.
	/// </summary>
	/// <remarks>
	/// Use with <c>await using</c> to ensure the event is always stopped — even when an exception occurs.
	/// Set <see cref="TrackedEventScope.Properties"/> and <see cref="TrackedEventScope.Measurements"/>
	/// on the returned scope to attach data to the event.
	/// </remarks>
	public static async Task<TrackedEventScope> EnterTrackEventScopeAsync(this IBlazorApplicationInsights appInsights, string name)
	{
		await appInsights.StartTrackEventAsync(name);
		return new TrackedEventScope(appInsights, name);
	}

	/// <summary>
	/// Starts timing a named page view and returns a <see cref="TrackedPageScope"/> that stops timing when disposed.
	/// Calls <see cref="IBlazorApplicationInsights.StartTrackPageAsync"/> immediately and
	/// <see cref="IBlazorApplicationInsights.StopTrackPageAsync"/> on dispose.
	/// </summary>
	/// <remarks>
	/// Use with <c>await using</c> to ensure the page view is always stopped — even when an exception occurs.
	/// Set <see cref="TrackedPageScope.Url"/>, <see cref="TrackedPageScope.Properties"/>, and
	/// <see cref="TrackedPageScope.Measurements"/> on the returned scope to attach data to the page view.
	/// </remarks>
	public static async Task<TrackedPageScope> EnterTrackPageScopeAsync(this IBlazorApplicationInsights appInsights, string name = null)
	{
		await appInsights.StartTrackPageAsync(name);
		return new TrackedPageScope(appInsights, name);
	}
}
