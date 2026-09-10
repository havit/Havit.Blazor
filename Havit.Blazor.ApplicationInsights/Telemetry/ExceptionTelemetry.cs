// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/IExceptionTelemetry.ts

using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Exception telemetry.
/// C# representation of the JavaScript <c>IExceptionTelemetry</c> interface.
/// </summary>
public class ExceptionTelemetry
{
	/// <summary>Unique identifier for this error.</summary>
	[JsonPropertyName("id")]
	public string Id { get; set; }

	/// <summary>
	/// The exception to report, in a form serializable to the JS <c>Error</c>-like shape expected by the SDK.
	/// Use <see cref="ExceptionDetails.FromException"/> to create from a C# <see cref="System.Exception"/>.
	/// </summary>
	[JsonPropertyName("exception")]
	public ExceptionDetails Exception { get; set; }

	/// <summary>Severity level used for filtering in the portal.</summary>
	[JsonPropertyName("severityLevel")]
	public SeverityLevel? SeverityLevel { get; set; }

	/// <summary>Custom defined iKey.</summary>
	[JsonPropertyName("iKey")]
	public string IKey { get; set; }
}

/// <summary>
/// Serializable representation of an exception, matching the JS <c>Error</c> shape expected by the Application Insights SDK.
/// </summary>
public class ExceptionDetails
{
	/// <summary>Exception type name (e.g. <c>"InvalidOperationException"</c>).</summary>
	[JsonPropertyName("name")]
	public string Name { get; set; } = "Error";

	/// <summary>Exception message.</summary>
	[JsonPropertyName("message")]
	public string Message { get; set; }

	/// <summary>Stack trace.</summary>
	[JsonPropertyName("stack")]
	public string Stack { get; set; }

	/// <summary>Creates an <see cref="ExceptionDetails"/> from a C# <see cref="Exception"/>.</summary>
	public static ExceptionDetails FromException(Exception exception) => new ExceptionDetails
	{
		Name = exception.GetType().FullName,
		Message = exception.Message,
		Stack = exception.StackTrace
	};
}
