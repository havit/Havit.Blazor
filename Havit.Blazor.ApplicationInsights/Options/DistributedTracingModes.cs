// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/applicationinsights-web-basic/src/Enums.ts

namespace Havit.Blazor.ApplicationInsights.Options;

/// <summary>
/// Controls which distributed tracing correlation headers are sent with outgoing requests.
/// C# representation of the JavaScript <c>eDistributedTracingModes</c> enum.
/// Serialized as an integer value as expected by the Application Insights JS SDK.
/// </summary>
public enum DistributedTracingModes
{
	/// <summary>
	/// Send only the legacy Application Insights correlation header (<c>Request-Id</c>).
	/// </summary>
	AI = 0,

	/// <summary>
	/// (Default) Send both W3C <c>traceparent</c> header and the legacy <c>Request-Id</c> header.
	/// </summary>
	AiAndW3C = 1,

	/// <summary>
	/// Send only the W3C <c>traceparent</c> header.
	/// </summary>
	W3C = 2,

	/// <summary>
	/// Send all W3C Trace Context headers (traceparent + tracestate) together with the legacy AI header.
	/// </summary>
	AiAndW3CTrace = 17,

	/// <summary>
	/// Send all W3C Trace Context headers (traceparent + tracestate).
	/// </summary>
	W3CTrace = 18
}
