// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/contracts/SeverityLevel.ts

namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Severity level for trace and exception telemetry.
/// C# representation of the JavaScript <c>SeverityLevel</c> enum.
/// Serialized as an integer as expected by the Application Insights JS SDK.
/// </summary>
public enum SeverityLevel
{
	/// <summary>
	/// Verbose.
	/// </summary>
	Verbose = 0,

	/// <summary>
	/// Information.
	/// </summary>
	Information = 1,

	/// <summary>
	/// Warning
	/// </summary>
	Warning = 2,

	/// <summary>
	/// Error.
	/// </summary>
	Error = 3,

	/// <summary>
	/// Critical.
	/// </summary>
	Critical = 4
}
