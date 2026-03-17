// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/IPageViewPerformanceTelemetry.ts

using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Page view performance telemetry.
/// C# representation of the JavaScript <c>IPageViewPerformanceTelemetry</c> interface.
/// All duration values use the TimeSpan general long format <c>d:hh:mm:ss.fffffff</c>.
/// </summary>
public class PageViewPerformanceTelemetry
{
	/// <summary>Page name. Defaults to the document title.</summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>Relative or absolute URL identifying the page. Defaults to the window location.</summary>
	[JsonPropertyName("uri")]
	public string Uri { get; set; }

	/// <summary>Total performance duration in TimeSpan 'G' format (<c>d:hh:mm:ss.fffffff</c>).</summary>
	[JsonPropertyName("perfTotal")]
	public string PerfTotal { get; set; }

	/// <summary>Total page load time in TimeSpan 'G' format (<c>d:hh:mm:ss.fffffff</c>).</summary>
	[JsonPropertyName("duration")]
	public string Duration { get; set; }

	/// <summary>Network connect time in TimeSpan 'G' format (<c>d:hh:mm:ss.fffffff</c>).</summary>
	[JsonPropertyName("networkConnect")]
	public string NetworkConnect { get; set; }

	/// <summary>Sent request time in TimeSpan 'G' format (<c>d:hh:mm:ss.fffffff</c>).</summary>
	[JsonPropertyName("sentRequest")]
	public string SentRequest { get; set; }

	/// <summary>Received response time in TimeSpan 'G' format (<c>d:hh:mm:ss.fffffff</c>).</summary>
	[JsonPropertyName("receivedResponse")]
	public string ReceivedResponse { get; set; }

	/// <summary>DOM processing time in TimeSpan 'G' format (<c>d:hh:mm:ss.fffffff</c>).</summary>
	[JsonPropertyName("domProcessing")]
	public string DomProcessing { get; set; }
}
