// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/IConfig.ts

using System.Text.Json.Serialization;

namespace Havit.Blazor.ApplicationInsights.Options;

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
public record class ApplicationInsightsConfig : ApplicationInsightsConfiguration
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

	[JsonPropertyName("autoTrackPageVisitTime")]
	public bool? AutoTrackPageVisitTime { get; set; }

	[JsonPropertyName("enableAutoRouteTracking")]
	public bool? EnableAutoRouteTracking { get; set; }

	[JsonPropertyName("disableAjaxTracking")]
	public bool? DisableAjaxTracking { get; set; }

	[JsonPropertyName("disableFetchTracking")]
	public bool? DisableFetchTracking { get; set; }

	[JsonPropertyName("overridePageViewDuration")]
	public bool? OverridePageViewDuration { get; set; }

	[JsonPropertyName("maxAjaxCallsPerView")]
	public int? MaxAjaxCallsPerView { get; set; }

	[JsonPropertyName("disableDataLossAnalysis")]
	public bool? DisableDataLossAnalysis { get; set; }

	[JsonPropertyName("disableCorrelationHeaders")]
	public bool? DisableCorrelationHeaders { get; set; }

	[JsonPropertyName("distributedTracingMode")]
	public DistributedTracingModes? DistributedTracingMode { get; set; }

	[JsonPropertyName("correlationHeaderExcludedDomains")]
	public string[] CorrelationHeaderExcludedDomains { get; set; }

	[JsonPropertyName("disableFlushOnBeforeUnload")]
	public bool? DisableFlushOnBeforeUnload { get; set; }

	[JsonPropertyName("disableFlushOnUnload")]
	public bool? DisableFlushOnUnload { get; set; }

	[JsonPropertyName("enableSessionStorageBuffer")]
	public bool? EnableSessionStorageBuffer { get; set; }

	/// <summary>
	/// Legacy alias for <see cref="ApplicationInsightsConfiguration.DisableCookiesUsage"/>.
	/// </summary>
	[JsonPropertyName("isCookieUseDisabled")]
	public bool? IsCookieUseDisabled { get; set; }

	[JsonPropertyName("isRetryDisabled")]
	public bool? IsRetryDisabled { get; set; }

	/// <summary>
	/// Sender endpoint URL (legacy; prefer <see cref="ApplicationInsightsConfiguration.EndpointUrl"/>).
	/// </summary>
	[JsonPropertyName("url")]
	public string Url { get; set; }

	[JsonPropertyName("isStorageUseDisabled")]
	public bool? IsStorageUseDisabled { get; set; }

	[JsonPropertyName("isBeaconApiDisabled")]
	public bool? IsBeaconApiDisabled { get; set; }

	[JsonPropertyName("disableXhr")]
	public bool? DisableXhr { get; set; }

	[JsonPropertyName("onunloadDisableFetch")]
	public bool? OnunloadDisableFetch { get; set; }

	[JsonPropertyName("sdkExtension")]
	public string SdkExtension { get; set; }

	[JsonPropertyName("isBrowserLinkTrackingEnabled")]
	public bool? IsBrowserLinkTrackingEnabled { get; set; }

	[JsonPropertyName("appId")]
	public string AppId { get; set; }

	[JsonPropertyName("enableCorsCorrelation")]
	public bool? EnableCorsCorrelation { get; set; }

	[JsonPropertyName("namePrefix")]
	public string NamePrefix { get; set; }

	[JsonPropertyName("sessionCookiePostfix")]
	public string SessionCookiePostfix { get; set; }

	[JsonPropertyName("userCookiePostfix")]
	public string UserCookiePostfix { get; set; }

	[JsonPropertyName("enableRequestHeaderTracking")]
	public bool? EnableRequestHeaderTracking { get; set; }

	[JsonPropertyName("enableResponseHeaderTracking")]
	public bool? EnableResponseHeaderTracking { get; set; }

	[JsonPropertyName("enableAjaxErrorStatusText")]
	public bool? EnableAjaxErrorStatusText { get; set; }

	[JsonPropertyName("enableAjaxPerfTracking")]
	public bool? EnableAjaxPerfTracking { get; set; }

	[JsonPropertyName("maxAjaxPerfLookupAttempts")]
	public int? MaxAjaxPerfLookupAttempts { get; set; }

	[JsonPropertyName("ajaxPerfLookupDelay")]
	public int? AjaxPerfLookupDelay { get; set; }

	[JsonPropertyName("onunloadDisableBeacon")]
	public bool? OnunloadDisableBeacon { get; set; }

	[JsonPropertyName("autoExceptionInstrumented")]
	public bool? AutoExceptionInstrumented { get; set; }

	[JsonPropertyName("correlationHeaderDomains")]
	public string[] CorrelationHeaderDomains { get; set; }

	[JsonPropertyName("autoUnhandledPromiseInstrumented")]
	public bool? AutoUnhandledPromiseInstrumented { get; set; }

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

	[JsonPropertyName("disableIkeyDeprecationMessage")]
	public bool? DisableIkeyDeprecationMessage { get; set; }

	[JsonPropertyName("disableUserInitMessage")]
	public bool? DisableUserInitMessage { get; set; }

	[JsonPropertyName("addIntEndpoints")]
	public bool? AddIntEndpoints { get; set; }

	/// <summary>
	/// Overrides the resolved sender endpoint URL entirely.
	/// </summary>
	[JsonPropertyName("userOverrideEndpointUrl")]
	public string UserOverrideEndpointUrl { get; set; }
}
