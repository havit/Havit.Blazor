using System.Text.Json;
using Havit.Blazor.ApplicationInsights.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Havit.Blazor.ApplicationInsights.Components;

/// <summary>
/// Bootstraps the Application Insights JavaScript SDK on the client. The bootstrap itself lives in
/// <c>wwwroot/HxApplicationInsights.js</c>: it installs the readiness gate (<c>window.havitBlazorAppInsights</c>),
/// runs the SDK snippet, registers the <see cref="BlazorApplicationInsightsOptions.DefaultTelemetryInitializer"/>
/// and tracks the initial page view.
/// <para>
/// The component covers three rendering scenarios:
/// <list type="bullet">
///   <item><description>
///     <b>Static SSR and prerendering:</b> The bootstrap is emitted inline as a classic <c>&lt;script&gt;</c> tag
///     directly into the HTML response (see the <c>.razor</c> file — rendered when not yet interactive).
///   </description></item>
///   <item><description>
///     <b>Interactive rendering after prerendering (Blazor Server / WASM):</b> The bootstrap already ran inline
///     during prerendering, so no additional action is taken on hydration.
///     A <see cref="PersistentComponentState"/> flag signals this to the interactive phase.
///   </description></item>
///   <item><description>
///     <b>Interactive rendering without prior prerendering:</b> The bootstrap is imported as an ES module
///     (<see cref="BootstrapModulePath"/>) and initialized via JS interop in <see cref="OnInitializedAsync"/>.
///     No <c>eval()</c> is involved, so a Content Security Policy does not need <c>'unsafe-eval'</c>.
///   </description></item>
/// </list>
/// It is the same file text in all three cases; the module has no exports and keeps its state on <c>window</c>,
/// so an inline copy and an imported copy share one gate.
/// </para>
/// <para>
/// Every <see cref="IBlazorApplicationInsights"/> call goes through the gate, which holds the call until the SDK
/// reports initialization via the snippet's <c>onInit</c> callback and then runs the calls in the order received.
/// Without it, calls made during the (potentially second-long) window between the snippet running and
/// <c>ai.3.gbl.min.js</c> finishing its download would hit the snippet's stub methods, which can throw while the
/// stubs are being swapped for the real SDK. <see cref="BrowserBlazorApplicationInsights"/> imports the module
/// itself before its first call, so a call made before this component got to initialize the SDK is held as well
/// instead of failing. If the SDK never initializes (download failure, or the component is not rendered at all),
/// the gate opens after 15 seconds and the held calls are dropped — the snippet's stubs are never called.
/// </para>
/// </summary>
public partial class HxApplicationInsights : IDisposable
{
	/// <summary>
	/// Path of the bootstrap module, as an <c>import</c> specifier. The <c>./</c> prefix matters: Blazor resolves such specifiers
	/// against <c>document.baseURI</c>, so the module gets the same URL (and therefore a single instance) regardless of the app's base path.
	/// </summary>
	internal const string BootstrapModulePath = "./_content/Havit.Blazor.ApplicationInsights/HxApplicationInsights.js";

	/// <summary>
	/// Logical name of the embedded resource holding the bootstrap (see the <c>EmbeddedResource</c> item in the project file).
	/// </summary>
	private const string BootstrapResourceName = "HxApplicationInsights.js";

	/// <summary>
	/// Key to prerender state to store whether the bootstrap was emitted during prerendering.
	/// </summary>
	private const string PrerenderedPersistentStateKey = nameof(PrerenderedPersistentStateKey);

	private static readonly Lazy<string> s_bootstrapModuleText = new Lazy<string>(ReadBootstrapModuleText);

	/// <summary>
	/// Optional CSP nonce to include on the inline <c>&lt;script&gt;</c> tag (SSR scenario) and on the SDK script tag.
	/// Required when a Content Security Policy with <c>nonce-*</c> is in use.
	/// </summary>
	[Parameter] public string Nonce { get; set; }

	[Inject] private IOptions<BlazorApplicationInsightsOptions> BlazorApplicationInsightsOptions { get; set; }
	[Inject] private PersistentComponentState PersistentState { get; set; }
	[Inject] private IJSRuntime JSRuntime { get; set; }

	private PersistingComponentStateSubscription _persistingSubscription;

	/// <inheritdoc />
	protected override async Task OnInitializedAsync()
	{
		await base.OnInitializedAsync();

		// Register the callback that persists a flag into PersistentComponentState
		// during prerendering, so the interactive phase knows the bootstrap already ran inline.
		_persistingSubscription = PersistentState.RegisterOnPersisting(PersistBlazorApplicationInsightsOptionsAsync);

		PersistentState.TryTakeFromJson<bool>(PrerenderedPersistentStateKey, out var prerendered);
		if (this.RendererInfo.IsInteractive && !prerendered)
		{
			// No prerendering occurred — the razor template did not emit the inline <script> tag.
			// Import the bootstrap module (side effects only: it installs window.havitBlazorAppInsights) and initialize it.
			// initialize() enqueues the default telemetry initializer and the initial page view together with running
			// the snippet, so no call made by the application can be ordered before them — the same ordering the inline
			// SSR script gives. The configuration goes over as pre-serialized JSON: the interop serializer would emit
			// every unset (null) property of the SDK configuration, ours leaves them out.
			var module = await JSRuntime.InvokeAsync<IJSObjectReference>("import", BootstrapModulePath);
			await module.DisposeAsync();

			await JSRuntime.InvokeVoidAsync(
				"havitBlazorAppInsights.initialize",
				GetSerializedApplicationInsightsConfiguration(),
				Nonce,
				GetSerializedDefaultTelemetryInitializerTags(),
				BlazorApplicationInsightsOptions.Value.EnableInitialPageViewTracking);
		}
	}

	/// <summary>
	/// Persists a flag into <see cref="PersistentComponentState"/> during prerendering to indicate
	/// that the bootstrap was already emitted inline. The interactive phase reads this flag
	/// and skips the module import to avoid initializing the SDK twice.
	/// </summary>
	private Task PersistBlazorApplicationInsightsOptionsAsync()
	{
		PersistentState.PersistAsJson(PrerenderedPersistentStateKey, true);
		return Task.CompletedTask;
	}

	/// <summary>
	/// The inline bootstrap for static SSR / prerendering: the module text followed by the <c>initialize(...)</c> call
	/// with the configuration, the nonce, the default telemetry initializer tags and the initial page view flag.
	/// </summary>
	/// <remarks>
	/// The configuration and the tags are emitted as JSON object literals (valid JavaScript), the nonce as an escaped
	/// string literal. Everything the bootstrap enqueues — initializer, initial page view — happens inside <c>initialize()</c>,
	/// atomically with the creation of the gate; the E2E tests (<c>InitialPageViewTrackingTests</c>) rely on that ordering.
	/// </remarks>
	private string GetBootstrapScript() => string.Concat(
		s_bootstrapModuleText.Value,
		Environment.NewLine,
		"window.havitBlazorAppInsights.initialize(",
		GetSerializedApplicationInsightsConfiguration(),
		", ",
		ToJavaScriptStringLiteral(Nonce),
		", ",
		GetSerializedDefaultTelemetryInitializerTags() ?? "null",
		", ",
		BlazorApplicationInsightsOptions.Value.EnableInitialPageViewTracking ? "true" : "false",
		");");

	private string GetSerializedApplicationInsightsConfiguration()
		=> JsonSerializer.Serialize(BlazorApplicationInsightsOptions.Value.JsSdkOptions, BlazorApplicationInsightsJsonSerializerContext.Default.BlazorApplicationInsightsJsSdkOptions);

	/// <summary>
	/// Tags of the <see cref="BlazorApplicationInsightsOptions.DefaultTelemetryInitializer"/> as JSON,
	/// or <c>null</c> when there is no initializer (or it has no tags).
	/// </summary>
	private string GetSerializedDefaultTelemetryInitializerTags()
	{
		var defaultTelemetryInitializer = BlazorApplicationInsightsOptions.Value.DefaultTelemetryInitializer;
		if (defaultTelemetryInitializer == null)
		{
			return null;
		}

		IDictionary<string, string> tags = defaultTelemetryInitializer.GetTags();
		if (tags.Count == 0)
		{
			return null;
		}

		return JsonSerializer.Serialize(tags, BlazorApplicationInsightsJsonSerializerContext.Default.TelemetryInitializerDictionary);
	}

	/// <summary>
	/// Renders the value as a JavaScript string literal safe to embed in an inline script (<c>null</c> for a null value).
	/// </summary>
	private static string ToJavaScriptStringLiteral(string value)
		=> (value == null) ? "null" : string.Concat("\"", JsonEncodedText.Encode(value), "\"");

	private static string ReadBootstrapModuleText()
	{
		using var stream = typeof(HxApplicationInsights).Assembly.GetManifestResourceStream(BootstrapResourceName)
			?? throw new InvalidOperationException($"Embedded resource '{BootstrapResourceName}' was not found in {typeof(HxApplicationInsights).Assembly.GetName().Name}.");
		using var reader = new StreamReader(stream);
		return reader.ReadToEnd();
	}

	/// <inheritdoc />
	public void Dispose()
	{
		// Unregister the PersistentComponentState persisting callback to avoid memory leaks
		// and prevent the callback from firing after the component has been removed.
		_persistingSubscription.Dispose();
	}
}
