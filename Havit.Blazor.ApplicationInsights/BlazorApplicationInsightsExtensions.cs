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
}
