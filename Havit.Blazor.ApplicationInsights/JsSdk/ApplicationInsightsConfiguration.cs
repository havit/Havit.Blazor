// This file is generated from the Application Insights JS SDK TypeScript source.
// Source: https://github.com/microsoft/ApplicationInsights-JS/blob/main/shared/AppInsightsCore/src/interfaces/ai/IConfiguration.ts

using System.ComponentModel;
using System.Text.Json.Serialization;
using Havit.Blazor.ApplicationInsights.Options;

namespace Havit.Blazor.ApplicationInsights.JsSdk;

/// <summary>
/// Base configuration provided to the Application Insights SDK core.
/// C# representation of the JavaScript <c>IConfiguration</c> interface.
/// </summary>
/// <remarks>
/// Skipped properties (no suitable C# / JSON representation):
/// <list type="bullet">
///   <item><c>extensionConfig</c> – <c>{ [key: string]: any }</c> (dynamic extension config dict)</item>
///   <item><c>extensions</c> – <c>ITelemetryPlugin[]</c> (runtime plugin instances)</item>
///   <item><c>channels</c> – <c>IChannelControls[][]</c> (runtime channel instances)</item>
///   <item><c>cookieCfg</c> – <c>ICookieMgrConfig</c> (interface type)</item>
///   <item><c>featureOptIn</c> – <c>IFeatureOptIn</c> (interface type)</item>
///   <item><c>expCfg</c> – <c>IExceptionConfig</c> (interface type)</item>
///   <item><c>createPerfMgr</c> – callback function</item>
/// </list>
/// </remarks>
[EditorBrowsable(EditorBrowsableState.Never)]
public class ApplicationInsightsConfiguration
{
	/// <summary>
	/// Instrumentation key of the resource. Either this or <see cref="ConnectionString"/> must be specified.
	/// </summary>
	[JsonPropertyName("instrumentationKey")]
	public string InstrumentationKey { get; set; }

	/// <summary>
	/// Connection string of the resource. Either this or <see cref="InstrumentationKey"/> must be specified.
	/// </summary>
	[JsonPropertyName("connectionString")]
	public string ConnectionString { get; set; }

	/// <summary>
	/// Timer interval (in ms) for the internal logging queue — how long to wait after
	/// <c>logger.queue</c> messages are detected before sending them.
	/// </summary>
	[JsonPropertyName("diagnosticLogInterval")]
	public int? DiagnosticLogInterval { get; set; }

	/// <summary>
	/// Maximum number of iKey-transmitted logging telemetry items per page view.
	/// </summary>
	[JsonPropertyName("maxMessageLimit")]
	public int? MaxMessageLimit { get; set; }

	/// <summary>
	/// Console logging level.
	/// <list type="bullet">
	///   <item>0 – all console logging off</item>
	///   <item>1 – severity &gt;= CRITICAL</item>
	///   <item>2 – severity &gt;= WARNING</item>
	/// </list>
	/// </summary>
	[JsonPropertyName("loggingLevelConsole")]
	public int? LoggingLevelConsole { get; set; }

	/// <summary>
	/// Telemetry logging level sent to the instrumentation key.
	/// <list type="bullet">
	///   <item>0 – all iKey logging off</item>
	///   <item>1 – severity &gt;= CRITICAL</item>
	///   <item>2 – severity &gt;= WARNING</item>
	/// </list>
	/// </summary>
	[JsonPropertyName("loggingLevelTelemetry")]
	public int? LoggingLevelTelemetry { get; set; }

	/// <summary>
	/// If enabled, uncaught exceptions will be thrown to help with debugging.
	/// </summary>
	[JsonPropertyName("enableDebug")]
	public bool? EnableDebug { get; set; }

	/// <summary>
	/// Endpoint where telemetry data is sent.
	/// </summary>
	[JsonPropertyName("endpointUrl")]
	public string EndpointUrl { get; set; }

	/// <summary>
	/// Disables Instrumentation Key validation.
	/// </summary>
	[JsonPropertyName("disableInstrumentationKeyValidation")]
	public bool? DisableInstrumentationKeyValidation { get; set; }

	/// <summary>
	/// When enabled, creates local perfEvents for instrumented code sections to help identify SDK performance issues.
	/// Does not send additional telemetry to the server.
	/// </summary>
	[JsonPropertyName("enablePerfMgr")]
	public bool? EnablePerfMgr { get; set; }

	/// <summary>
	/// Fire every single performance event, not just the top-level root performance event.
	/// Defaults to <c>false</c>.
	/// </summary>
	[JsonPropertyName("perfEvtsSendAll")]
	public bool? PerfEvtsSendAll { get; set; }

	/// <summary>
	/// Default length used to generate random session and user IDs.
	/// Defaults to 22.
	/// </summary>
	[JsonPropertyName("idLength")]
	public int? IdLength { get; set; }

	/// <summary>
	/// Custom cookie domain to share Application Insights cookies across subdomains.
	/// </summary>
	[JsonPropertyName("cookieDomain")]
	public string CookieDomain { get; set; }

	/// <summary>
	/// Custom cookie path for use behind an application gateway.
	/// </summary>
	[JsonPropertyName("cookiePath")]
	public string CookiePath { get; set; }

	/// <summary>
	/// Disables all cookie usage by the SDK.
	/// Can be re-enabled after initialization via <c>core.getCookieMgr().enable()</c>.
	/// </summary>
	[JsonPropertyName("disableCookiesUsage")]
	public bool? DisableCookiesUsage { get; set; }

	/// <summary>
	/// Page unload events to ignore (e.g. <c>"beforeunload"</c>, <c>"unload"</c>,
	/// <c>"visibilitychange"</c>, <c>"pagehide"</c>).
	/// At least one valid unload event must remain hooked.
	/// </summary>
	[JsonPropertyName("disablePageUnloadEvents")]
	public string[] DisablePageUnloadEvents { get; set; }

	/// <summary>
	/// Page show events to ignore (e.g. <c>"pageshow"</c>, <c>"visibilitychange"</c>).
	/// At least one valid show event must remain hooked.
	/// </summary>
	[JsonPropertyName("disablePageShowEvents")]
	public string[] DisablePageShowEvents { get; set; }

	/// <summary>
	/// Disables attempting to use the Chrome Debug Extension.
	/// </summary>
	[JsonPropertyName("disableDbgExt")]
	public bool? DisableDbgExt { get; set; }

	/// <summary>
	/// Adds <c>&amp;w=0</c> parameter to support UA parsing when web-workers don't have access to <c>Document</c>.
	/// Defaults to <c>false</c>.
	/// </summary>
	[JsonPropertyName("enableWParam")]
	public bool? EnableWParam { get; set; }

	/// <summary>
	/// Custom prefix added to storage names.
	/// </summary>
	[JsonPropertyName("storagePrefix")]
	public string StoragePrefix { get; set; }

	/// <summary>
	/// Timeout (in ms) for connection string, instrumentation key and endpoint URL resolution.
	/// Default: 50000 ms.
	/// </summary>
	[JsonPropertyName("initTimeOut")]
	public int? InitTimeOut { get; set; }

	/// <summary>
	/// Maximum number of in-memory proxy track calls buffered before async initialization completes.
	/// Default: 100.
	/// </summary>
	[JsonPropertyName("initInMemoMaxSize")]
	public int? InitInMemoMaxSize { get; set; }

	/// <summary>
	/// Enables or disables URL field redaction. Defaults to <c>true</c>.
	/// </summary>
	[JsonPropertyName("redactUrls")]
	public bool? RedactUrls { get; set; }

	/// <summary>
	/// Additional query parameter names to redact beyond the default set
	/// (<c>sig</c>, <c>Signature</c>, <c>AWSAccessKeyId</c>, <c>X-Goog-Signature</c>).
	/// </summary>
	[JsonPropertyName("redactQueryParams")]
	public string[] RedactQueryParams { get; set; }

	/// <summary>
	/// Controls whether the SDK looks for <c>traceparent</c> and/or <c>tracestate</c> values
	/// from service timing headers or meta tags on the initial page load.
	/// Default: <see cref="TraceHeadersMode.All"/>.
	/// </summary>
	[JsonPropertyName("traceHdrMode")]
	public TraceHeadersMode? TraceHdrMode { get; set; }
}
