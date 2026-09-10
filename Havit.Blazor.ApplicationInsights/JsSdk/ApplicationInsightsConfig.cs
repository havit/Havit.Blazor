// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/IConfig.ts

using System.ComponentModel;
using System.Text.Json.Serialization;
using Havit.Blazor.ApplicationInsights.Options;

namespace Havit.Blazor.ApplicationInsights.JsSdk;

/// <summary>
/// Configuration settings for how telemetry is sent by the Application Insights JS SDK.
/// C# representation of the JavaScript <c>IConfig</c> interface, extending <see cref="ApplicationInsightsConfiguration"/>.
/// </summary>
/// <remarks>
/// Properties already present in <see cref="ApplicationInsightsConfiguration"/> (<c>enableDebug</c>,
/// <c>disableCookiesUsage</c>, <c>cookieDomain</c>, <c>cookiePath</c>) are inherited and not repeated here.
/// <br/>
/// Skipped properties (no suitable C# / JSON representation):
/// <list type="bullet">
///   <item><c>addRequestContext</c> – callback function</item>
///   <item><c>bufferOverride</c> – <c>IStorageBuffer</c> (interface type)</item>
///   <item><c>excludeRequestFromAutoTrackingPatterns</c> – <c>string[] | RegExp[]</c> (RegExp not JSON-serializable)</item>
///   <item><c>correlationHeaderExcludePatterns</c> – <c>RegExp[]</c> (RegExp not JSON-serializable)</item>
///   <item><c>convertUndefined</c> – <c>any</c> (no C# equivalent)</item>
///   <item><c>throttleMgrCfg</c> – <c>{ [key: number]: IThrottleMgrConfig }</c> (interface type values)</item>
/// </list>
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public class ApplicationInsightsConfig : ApplicationInsightsConfiguration
{
	/// <summary>
	/// Use line-delimited JSON format instead of normal JSON. Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("emitLineDelimitedJson")]
	public bool? EmitLineDelimitedJson { get; set; }

	/// <summary>
	/// Optional account ID if your app groups users into accounts.
	/// No spaces, commas, semicolons, equals, or vertical bars.
	/// </summary>
	[JsonPropertyName("accountId")]
	public string AccountId { get; set; }

	/// <summary>
	/// A session is logged if the user is inactive for this amount of time (ms).
	/// Default: 30 min (1 800 000 ms).
	/// </summary>
	[JsonPropertyName("sessionRenewalMs")]
	public int? SessionRenewalMs { get; set; }

	/// <summary>
	/// A session is logged if it has continued for this amount of time (ms).
	/// Default: 24 h (86 400 000 ms).
	/// </summary>
	[JsonPropertyName("sessionExpirationMs")]
	public int? SessionExpirationMs { get; set; }

	/// <summary>
	/// Maximum telemetry batch size in bytes. When exceeded the batch is sent and a new one starts.
	/// Default: 100 000.
	/// </summary>
	[JsonPropertyName("maxBatchSizeInBytes")]
	public int? MaxBatchSizeInBytes { get; set; }

	/// <summary>
	/// How long to batch telemetry before sending (ms). Default: 15 000 ms.
	/// </summary>
	[JsonPropertyName("maxBatchInterval")]
	public int? MaxBatchInterval { get; set; }

	/// <summary>
	/// If <c>true</c>, exceptions are not auto-collected. Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("disableExceptionTracking")]
	public bool? DisableExceptionTracking { get; set; }

	/// <summary>
	/// If <c>true</c>, telemetry is neither collected nor sent. Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("disableTelemetry")]
	public bool? DisableTelemetry { get; set; }

	/// <summary>
	/// Percentage of events that will be sent. Default: 100 (all events).
	/// </summary>
	[JsonPropertyName("samplingPercentage")]
	public int? SamplingPercentage { get; set; }

	/// <summary>
	/// If enabled, tracks visit time of the previous page view and sends it as a custom metric
	/// named <c>PageVisitTime</c> (milliseconds). Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("autoTrackPageVisitTime")]
	public bool? AutoTrackPageVisitTime { get; set; }

	/// <summary>
	/// Automatically tracks route changes in single-page applications and emits a new page view for each route.
	/// </summary>
	[JsonPropertyName("enableAutoRouteTracking")]
	public bool? EnableAutoRouteTracking { get; set; }

	/// <summary>
	/// If <c>true</c>, AJAX calls are not auto-collected. Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("disableAjaxTracking")]
	public bool? DisableAjaxTracking { get; set; }

	/// <summary>
	/// If <c>true</c>, Fetch requests are not auto-collected. Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("disableFetchTracking")]
	public bool? DisableFetchTracking { get; set; }

	/// <summary>
	/// If enabled, <c>trackPageView</c> records duration as the interval ending at call time
	/// instead of relying on Navigation Timing when no custom duration is provided.
	/// </summary>
	[JsonPropertyName("overridePageViewDuration")]
	public bool? OverridePageViewDuration { get; set; }

	/// <summary>
	/// Maximum number of AJAX calls monitored per page view. Use <c>-1</c> for unlimited.
	/// Default: 500.
	/// </summary>
	[JsonPropertyName("maxAjaxCallsPerView")]
	public int? MaxAjaxCallsPerView { get; set; }

	/// <summary>
	/// If <c>false</c>, unsent sender buffers are checked at startup to reduce telemetry loss.
	/// Default: <c>true</c>.
	/// </summary>
	[JsonPropertyName("disableDataLossAnalysis")]
	public bool? DisableDataLossAnalysis { get; set; }

	/// <summary>
	/// If <c>false</c>, correlation headers are added to dependency requests for server-side correlation.
	/// Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("disableCorrelationHeaders")]
	public bool? DisableCorrelationHeaders { get; set; }

	/// <summary>
	/// Distributed tracing mode. In W3C-capable modes, <c>traceparent</c>/<c>tracestate</c>
	/// headers are added to outgoing requests.
	/// </summary>
	[JsonPropertyName("distributedTracingMode")]
	public DistributedTracingModes? DistributedTracingMode { get; set; }

	/// <summary>
	/// Domains for which correlation headers are disabled.
	/// </summary>
	[JsonPropertyName("correlationHeaderExcludedDomains")]
	public string[] CorrelationHeaderExcludedDomains { get; set; }

	/// <summary>
	/// If <c>true</c>, suppresses automatic <c>flush</c> on unload/page-hide/visibility-change events.
	/// Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("disableFlushOnBeforeUnload")]
	public bool? DisableFlushOnBeforeUnload { get; set; }

	/// <summary>
	/// Back-compat alias for <see cref="DisableFlushOnBeforeUnload"/> for page-hide/visibility-change handling.
	/// </summary>
	[JsonPropertyName("disableFlushOnUnload")]
	public bool? DisableFlushOnUnload { get; set; }

	/// <summary>
	/// If enabled, unsent telemetry is buffered in session storage and restored on next page load.
	/// Default: <c>true</c>.
	/// </summary>
	[JsonPropertyName("enableSessionStorageBuffer")]
	public bool? EnableSessionStorageBuffer { get; set; }

	/// <summary>
	/// Legacy alias for <see cref="ApplicationInsightsConfiguration.DisableCookiesUsage"/>.
	/// </summary>
	[JsonPropertyName("isCookieUseDisabled")]
	public bool? IsCookieUseDisabled { get; set; }

	/// <summary>
	/// If <c>true</c>, disables retry attempts for transient ingestion errors (for example 206/408/429/500/503).
	/// </summary>
	[JsonPropertyName("isRetryDisabled")]
	public bool? IsRetryDisabled { get; set; }

	/// <summary>
	/// Sender endpoint URL (legacy; prefer <see cref="ApplicationInsightsConfiguration.EndpointUrl"/>).
	/// </summary>
	[JsonPropertyName("url")]
	public string Url { get; set; }

	/// <summary>
	/// If <c>true</c>, disables reading/writing local and session storage. Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("isStorageUseDisabled")]
	public bool? IsStorageUseDisabled { get; set; }

	/// <summary>
	/// If <c>false</c>, telemetry sending prefers Beacon API transport. Default: <c>true</c>.
	/// </summary>
	[JsonPropertyName("isBeaconApiDisabled")]
	public bool? IsBeaconApiDisabled { get; set; }

	/// <summary>
	/// Disables XHR/XDomainRequest as primary transport and prefers <c>fetch()</c>/<c>sendBeacon</c>.
	/// </summary>
	[JsonPropertyName("disableXhr")]
	public bool? DisableXhr { get; set; }

	/// <summary>
	/// If enabled, does not use <c>fetch keepalive</c> during unload sending.
	/// </summary>
	[JsonPropertyName("onunloadDisableFetch")]
	public bool? OnunloadDisableFetch { get; set; }

	/// <summary>
	/// SDK extension name prefixing <c>ai.internal.sdkVersion</c> (alphabetic only).
	/// </summary>
	[JsonPropertyName("sdkExtension")]
	public string SdkExtension { get; set; }

	/// <summary>
	/// If enabled, Browser Link requests are tracked. Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("isBrowserLinkTrackingEnabled")]
	public bool? IsBrowserLinkTrackingEnabled { get; set; }

	/// <summary>
	/// Optional AppId for client/server dependency correlation, typically for Beacon scenarios.
	/// </summary>
	[JsonPropertyName("appId")]
	public string AppId { get; set; }

	/// <summary>
	/// If enabled, adds correlation headers to CORS dependency requests. Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("enableCorsCorrelation")]
	public bool? EnableCorsCorrelation { get; set; }

	/// <summary>
	/// Optional postfix used in storage and cookie names.
	/// </summary>
	[JsonPropertyName("namePrefix")]
	public string NamePrefix { get; set; }

	/// <summary>
	/// Optional postfix used for the session cookie name; falls back to <see cref="NamePrefix"/> when omitted.
	/// </summary>
	[JsonPropertyName("sessionCookiePostfix")]
	public string SessionCookiePostfix { get; set; }

	/// <summary>
	/// Optional postfix used for the user cookie name.
	/// </summary>
	[JsonPropertyName("userCookiePostfix")]
	public string UserCookiePostfix { get; set; }

	/// <summary>
	/// If enabled, request headers are captured for dependency telemetry.
	/// </summary>
	[JsonPropertyName("enableRequestHeaderTracking")]
	public bool? EnableRequestHeaderTracking { get; set; }

	/// <summary>
	/// If enabled, response headers are captured for dependency telemetry.
	/// </summary>
	[JsonPropertyName("enableResponseHeaderTracking")]
	public bool? EnableResponseHeaderTracking { get; set; }

	/// <summary>
	/// If enabled, dependency error status text is captured.
	/// </summary>
	[JsonPropertyName("enableAjaxErrorStatusText")]
	public bool? EnableAjaxErrorStatusText { get; set; }

	/// <summary>
	/// Enables collecting additional browser performance timings for AJAX/fetch dependencies.
	/// Defaults to <c>false</c>.
	/// </summary>
	[JsonPropertyName("enableAjaxPerfTracking")]
	public bool? EnableAjaxPerfTracking { get; set; }

	/// <summary>
	/// Maximum retry attempts when looking up performance timing details. Defaults to 3.
	/// </summary>
	[JsonPropertyName("maxAjaxPerfLookupAttempts")]
	public int? MaxAjaxPerfLookupAttempts { get; set; }

	/// <summary>
	/// Delay in milliseconds between performance timing lookup attempts. Defaults to 25 ms.
	/// </summary>
	[JsonPropertyName("ajaxPerfLookupDelay")]
	public int? AjaxPerfLookupDelay { get; set; }

	/// <summary>
	/// If <c>false</c> during tab close, remaining telemetry is sent using Beacon API.
	/// Default: <c>false</c>.
	/// </summary>
	[JsonPropertyName("onunloadDisableBeacon")]
	public bool? OnunloadDisableBeacon { get; set; }

	/// <summary>
	/// Internal flag used by the SDK exception instrumentation pipeline.
	/// </summary>
	[JsonPropertyName("autoExceptionInstrumented")]
	public bool? AutoExceptionInstrumented { get; set; }

	/// <summary>
	/// Optional allow-list of domains that may receive correlation headers on dependency requests.
	/// When set, headers are included only for matching domains (wildcards such as <c>*.example.com</c> are supported).
	/// Can be combined with <see cref="CorrelationHeaderExcludedDomains"/> to exclude specific subsets.
	/// </summary>
	[JsonPropertyName("correlationHeaderDomains")]
	public string[] CorrelationHeaderDomains { get; set; }

	/// <summary>
	/// Internal flag used by the SDK unhandled-promise instrumentation pipeline.
	/// </summary>
	[JsonPropertyName("autoUnhandledPromiseInstrumented")]
	public bool? AutoUnhandledPromiseInstrumented { get; set; }

	/// <summary>
	/// If enabled, unhandled promise rejections are tracked and reported as JavaScript errors.
	/// </summary>
	[JsonPropertyName("enableUnhandledPromiseRejectionTracking")]
	public bool? EnableUnhandledPromiseRejectionTracking { get; set; }

	/// <summary>
	/// Custom HTTP headers added to every outgoing telemetry request.
	/// </summary>
	[JsonPropertyName("customHeaders")]
	public List<CustomHeader> CustomHeaders { get; set; }

	/// <summary>
	/// Maximum number of events kept in memory. Default: SDK internal limit.
	/// </summary>
	[JsonPropertyName("eventsLimitInMem")]
	public int? EventsLimitInMem { get; set; }

	/// <summary>
	/// Disables iKey deprecation warning messages. Default: <c>true</c>.
	/// </summary>
	[JsonPropertyName("disableIkeyDeprecationMessage")]
	public bool? DisableIkeyDeprecationMessage { get; set; }

	/// <summary>
	/// Disables sending internal initialization message <c>SendBrowserInfoOnUserInit</c>.
	/// </summary>
	[JsonPropertyName("disableUserInitMessage")]
	public bool? DisableUserInitMessage { get; set; }

	/// <summary>
	/// If enabled, SDK internal endpoints are auto-added to request auto-tracking exclusions.
	/// </summary>
	[JsonPropertyName("addIntEndpoints")]
	public bool? AddIntEndpoints { get; set; }

	/// <summary>
	/// Overrides the resolved sender endpoint URL entirely.
	/// </summary>
	[JsonPropertyName("userOverrideEndpointUrl")]
	public string UserOverrideEndpointUrl { get; set; }
}
