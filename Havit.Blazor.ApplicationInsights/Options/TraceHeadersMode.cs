// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/applicationinsights-web-basic/src/Enums.ts

namespace Havit.Blazor.ApplicationInsights.Options;

/// <summary>
/// Controls whether the SDK looks for <c>traceparent</c> and/or <c>tracestate</c> values
/// from service timing headers or meta tags on the initial page load.
/// C# representation of the JavaScript <c>eTraceHeadersMode</c> enum.
/// Values are bitwise-combinable flags; serialized as an integer as expected by the Application Insights JS SDK.
/// </summary>
[Flags]
public enum TraceHeadersMode
{
	/// <summary>Don't look for any trace headers.</summary>
	None = 0x00,

	/// <summary>Look for <c>traceparent</c> header/meta tag.</summary>
	TraceParent = 0x01,

	/// <summary>Look for <c>tracestate</c> header/meta tag.</summary>
	TraceState = 0x02,

	/// <summary>Look for both <c>traceparent</c> and <c>tracestate</c> headers/meta tags. (Default)</summary>
	All = TraceParent | TraceState
}
