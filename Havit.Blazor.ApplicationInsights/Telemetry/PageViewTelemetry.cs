// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/IPageViewTelemetry.ts

using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Telemetry;

/// <summary>
/// Page view telemetry.
/// C# representation of the JavaScript <c>IPageViewTelemetry</c> interface.
/// </summary>
public class PageViewTelemetry
{
	/// <summary>
	/// The name used in <c>startTrackPage</c>. Defaults to the document title.
	/// </summary>
	[JsonPropertyName("name")]
	public string Name { get; set; }

	/// <summary>
	/// Relative or absolute URL that identifies the page. Defaults to the window location.
	/// </summary>
	[JsonPropertyName("uri")]
	public string Uri { get; set; }

	/// <summary>URL of the source page this page was loaded from.</summary>
	[JsonPropertyName("refUri")]
	public string RefUri { get; set; }

	/// <summary>Page type.</summary>
	[JsonPropertyName("pageType")]
	public string PageType { get; set; }

	/// <summary>Whether the user is logged in.</summary>
	[JsonPropertyName("isLoggedIn")]
	public bool? IsLoggedIn { get; set; }

	/// <summary>
	/// Additional custom properties. May include <c>duration</c> (ms) as a well-known key;
	/// if omitted the SDK calculates page load time internally.
	/// </summary>
	[JsonPropertyName("properties")]
	public Dictionary<string, object> Properties { get; set; }

	/// <summary>Custom defined iKey.</summary>
	[JsonPropertyName("iKey")]
	public string IKey { get; set; }

	/// <summary>Time the first page view was triggered.</summary>
	[JsonPropertyName("startTime")]
	public DateTimeOffset? StartTime { get; set; }
}
