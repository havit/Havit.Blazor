using Havit.Blazor.ApplicationInsights.Options;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Havit.Blazor.ApplicationInsights.Components;

// see https://github.com/microsoft/ApplicationInsights-JS?tab=readme-ov-file#snippet-setup-ignore-if-using-npm-setup

/// <summary>
/// Renders the Application Insights JavaScript SDK snippet and initializes the client-side telemetry.
/// <para>
/// The component covers three rendering scenarios:
/// <list type="bullet">
///   <item><description>
///     <b>Static SSR and prerendering:</b> The script is emitted inline as a <c>&lt;script&gt;</c> tag
///     directly into the HTML response (see the <c>.razor</c> file — rendered when not yet interactive).
///   </description></item>
///   <item><description>
///     <b>Interactive rendering after prerendering (Blazor Server / WASM):</b> The script was already
///     injected during prerendering, so no additional action is taken on hydration.
///     A <see cref="PersistentComponentState"/> flag signals this to the interactive phase.
///   </description></item>
///   <item><description>
///     <b>Interactive rendering without prior prerendering:</b> The script is injected at runtime via
///     <c>eval()</c> in <see cref="OnInitializedAsync"/>, because no inline tag was emitted earlier.
///   </description></item>
/// </list>
/// In all three cases it is the same bootstrap text (<see cref="GetBootstrapScript"/>): gate, snippet, default
/// telemetry initializer, initial page view.
/// </para>
/// <para>
/// Alongside the snippet the component installs <c>window.havitBlazorAppInsights</c> — a thin wrapper
/// every <see cref="IBlazorApplicationInsights"/> call goes through. The wrapper holds each call until the
/// SDK reports initialization via the snippet's <c>onInit</c> callback, and then runs the calls in the order
/// they were received. Without it, calls made during the (potentially second-long) window between the snippet
/// being evaluated and <c>ai.3.gbl.min.js</c> finishing its download hit the snippet's stub methods, which can
/// throw while the stubs are being swapped for the real SDK. If the SDK script never loads (all CDN fallbacks
/// exhausted), the gate opens anyway after 15 seconds so the calls do not hang — from that point on they pass
/// straight through to the snippet's no-op stubs.
/// </para>
/// </summary>
public partial class HxApplicationInsights : IDisposable
{
	/// <summary>
	/// Key to prerender state to store whether the script was emitted during prerendering.	
	/// </summary>
	private const string PrerenderedPersistentStateKey = nameof(PrerenderedPersistentStateKey);

	/// <summary>
	/// Optional CSP nonce to include on the inline <c>&lt;script&gt;</c> tag (SSR scenario).
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
		// during prerendering, so the interactive phase knows the script was already emitted inline.
		_persistingSubscription = PersistentState.RegisterOnPersisting(PersistBlazorApplicationInsightsOptionsAsync);

		PersistentState.TryTakeFromJson<bool>(PrerenderedPersistentStateKey, out var prerendered);
		if (this.RendererInfo.IsInteractive && !prerendered)
		{
			// No prerendering occurred — the razor template did not emit the inline <script> tag,
			// so inject the very same bootstrap programmatically via eval() now.
			// It is one script on purpose (see GetBootstrapScript): the default telemetry initializer and the
			// initial page view get enqueued together with the creation of the gate, so no call made by the
			// application can be ordered before them — the same ordering the inline SSR script gives.
			await JSRuntime.InvokeVoidAsync("eval", GetBootstrapScript());
		}
	}

	/// <summary>
	/// Persists a flag into <see cref="PersistentComponentState"/> during prerendering to indicate
	/// that the SDK script was already emitted inline. The interactive phase reads this flag
	/// and skips the <c>eval()</c> injection to avoid initializing the SDK twice.
	/// </summary>
	private Task PersistBlazorApplicationInsightsOptionsAsync()
	{
		PersistentState.PersistAsJson(PrerenderedPersistentStateKey, true);
		return Task.CompletedTask;
	}

	/// <summary>
	/// The complete bootstrap: the readiness gate, the SDK snippet, the default telemetry initializer (if configured)
	/// and the initial page view (if enabled), in this order. Emitted inline in SSR/prerendering and evaluated via
	/// <c>eval()</c> in the interactive-without-prerendering scenario — the same text in both cases.
	/// </summary>
	/// <remarks>
	/// Keeping the initializer and the initial page view inside the bootstrap (rather than issuing them as separate
	/// interop calls afterwards) enqueues them in the gate atomically with its creation, so a call made by the
	/// application — which cannot reach the gate before it exists — comes after them. Not a documented contract,
	/// but the E2E tests (<c>InitialPageViewTrackingTests</c>) rely on it; revisit them if this changes.
	/// </remarks>
	private string GetBootstrapScript() => string.Concat(
		GetApplicationInsightsScript(),
		Environment.NewLine,
		GetDefaultTelemetryInitializerScript(),
		Environment.NewLine,
		GetInitialTrackPageViewScript());

	// source: https://github.com/microsoft/ApplicationInsights-JS?tab=readme-ov-file#snippet-setup-ignore-if-using-npm-setup
	private string GetApplicationInsightsScript() => $$$$"""
		window.havitBlazorAppInsights = window.havitBlazorAppInsights || (function () {
			var readyResolve;
			// Resolves to true once the SDK reports initialization, to false when the gate had to open on the fallback timeout.
			var ready = new Promise(function (resolve) { readyResolve = resolve; });

			// If the SDK script never loads (all CDN fallbacks exhausted), onInit is never invoked.
			// Open the gate anyway so that the calls do not hang forever - in that case the snippet
			// turns its stubs into no-ops, so the calls degrade to silently doing nothing.
			var readyTimeout = setTimeout(function () { readyResolve(false); }, 15000);

			// Every call runs after the SDK is initialized and in the order it was received
			// (the same ordering guarantee the snippet queue provides).
			var chain = ready;
			function enqueue(action) {
				chain = chain.then(action, action);
				return chain;
			}
			function invoke(name, args) {
				return enqueue(function () {
					var appInsights = window.appInsights;
					if (appInsights && appInsights[name]) {
						return appInsights[name].apply(appInsights, args);
					}
				});
			}

			var api = {
				ready: ready,
				markReady: function () { clearTimeout(readyTimeout); readyResolve(true); },
				addTelemetryInitializer: function (tags) {
					return invoke("addTelemetryInitializer", [function (item) {
						for (var key in tags) {
							if (Object.prototype.hasOwnProperty.call(tags, key)) { item.tags[key] = tags[key]; }
						}
					}]);
				}
			};
			["trackEvent", "trackPageView", "trackException", "trackTrace", "trackMetric", "trackPageViewPerformance", "trackDependencyData", "startTrackPage", "stopTrackPage", "startTrackEvent", "stopTrackEvent", "setAuthenticatedUserContext", "clearAuthenticatedUserContext", "flush"].forEach(function (name) {
				api[name] = function () { return invoke(name, arguments); };
			});
			return api;
		})();
		!(function (cfg){var k,x,D,E,L,C,b,U,O,A,e,t="track",n="TrackPage",i="TrackEvent",I=[t+"Event",t+"Exception",t+"PageView",t+"PageViewPerformance","addTelemetryInitializer",t+"Trace",t+"DependencyData",t+"Metric","start"+n,"stop"+n,"start"+i,"stop"+i,"setAuthenticatedUserContext","clearAuthenticatedUserContext","flush"];function a(){cfg.onInit&&cfg.onInit(e)}k=window,x=document,D=k.location,E="script",L="ingestionendpoint",C="disableExceptionTracking",b="crossOrigin",U="POST",O=cfg.pn||"aiPolicy",t="appInsightsSDK",A=cfg.name||"appInsights",(cfg.name||k[t])&&(k[t]=A),e=k[A]||function(u){var n=u.url||cfg.src,s=!1,p=!1,l={initialize:!0,queue:[],sv:"10",config:u,version:2,extensions:void 0};function d(e){var t,n,i,a,r,o,c,s;!0!==cfg.dle&&(o=(t=function(){var e,t={},n=u.connectionString;if("string"==typeof n&&n)for(var i=n.split(";"),a=0;a<i.length;a++){var r=i[a].split("=");2===r.length&&(t[r[0].toLowerCase()]=r[1])}return t[L]||(e=(n=t.endpointsuffix)?t.location:null,t[L]="https://"+(e?e+".":"")+"dc."+(n||"services.visualstudio.com")),t}()).instrumentationkey||u.instrumentationKey||"",t=(t=(t=t[L])&&"/"===t.slice(-1)?t.slice(0,-1):t)?t+"/v2/track":u.endpointUrl,t=u.userOverrideEndpointUrl||t,(n=[]).push((i="SDK LOAD Failure: Failed to load Application Insights SDK script (See stack for details)",a=e,c=t,(s=(r=f(o,"Exception")).data).baseType="ExceptionData",s.baseData.exceptions=[{typeName:"SDKLoadFailed",message:i.replace(/\./g,"-"),hasFullStack:!1,stack:i+"\nSnippet failed to load ["+a+"] -- Telemetry is disabled\nHelp Link: https://go.microsoft.com/fwlink/?linkid=2128109\nHost: "+(D&&D.pathname||"_unknown_")+"\nEndpoint: "+c,parsedStack:[]}],r)),n.push((s=e,i=t,(c=(a=f(o,"Message")).data).baseType="MessageData",(r=c.baseData).message='AI (Internal): 99 message:"'+("SDK LOAD Failure: Failed to load Application Insights SDK script (See stack for details) ("+s+")").replace(/\"/g,"")+'"',r.properties={endpoint:i},a)),e=n,o=t,JSON&&((c=k.fetch)&&!cfg.useXhr?c(o,{method:U,body:JSON.stringify(e),mode:"cors"}):XMLHttpRequest&&((s=new XMLHttpRequest).open(U,o),s.setRequestHeader("Content-type","application/json"),s.send(JSON.stringify(e)))))}function f(e,t){return e=e,t=t,i=l.sv,a=l.version,r=D,(o={})["ai.device."+"id"]="browser",o["ai.device.type"]="Browser",o["ai.operation.name"]=r&&r.pathname||"_unknown_",o["ai.internal.sdkVersion"]="javascript:snippet_"+(i||a),{time:(r=new Date).getUTCFullYear()+"-"+n(1+r.getUTCMonth())+"-"+n(r.getUTCDate())+"T"+n(r.getUTCHours())+":"+n(r.getUTCMinutes())+":"+n(r.getUTCSeconds())+"."+(r.getUTCMilliseconds()/1e3).toFixed(3).slice(2,5)+"Z",iKey:e,name:"Microsoft.ApplicationInsights."+e.replace(/-/g,"")+"."+t,sampleRate:100,tags:o,data:{baseData:{ver:2}},ver:undefined,seq:"1",aiDataContract:undefined};function n(e){e=""+e;return 1===e.length?"0"+e:e}var i,a,r,o}var i,a,t,r,g=-1,h=0,m=["js.monitor.azure.com","js.cdn.applicationinsights.io","js.cdn.monitor.azure.com","js0.cdn.applicationinsights.io","js0.cdn.monitor.azure.com","js2.cdn.applicationinsights.io","js2.cdn.monitor.azure.com","az416426.vo.msecnd.net"],o=function(){return c(n,null)};function c(t,r){if((n=navigator)&&(~(n=(n.userAgent||"").toLowerCase()).indexOf("msie")||~n.indexOf("trident/"))&&~t.indexOf("ai.3")&&(t=t.replace(/(\/)(ai\.3\.)([^\d]*)$/,function(e,t,n){return t+"ai.2"+n})),!1!==cfg.cr)for(var e=0;e<m.length;e++)if(0<t.indexOf(m[e])){g=e;break}var n,o=function(e){var a;l.queue=[],p||(0<=g&&h+1<m.length?(a=(g+h+1)%m.length,i(t.replace(/^(.*\/\/)([\w\.]*)(\/.*)$/,function(e,t,n,i){return t+m[a]+i})),h+=1):(s=p=!0,d(t)))},c=function(e,t){p||setTimeout(function(){t&&!l.core&&o()},500),s=!1},i=function(e){var n,i=x.createElement(E),e=(cfg.pl?cfg.ttp&&cfg.ttp.createScript?i.src=cfg.ttp.createScriptURL(e):i.src=(null==(n=window.trustedTypes)?void 0:n.createPolicy(O,{createScriptURL:function(e){try{var t=new URL(e);if(t.host&&"js.monitor.azure.com"===t.host)return e;a(e)}catch(n){a(e)}}})).createScriptURL(e):i.src=e,cfg.nt&&i.setAttribute("nonce",cfg.nt),r&&(i.integrity=r),i.setAttribute("data-ai-name",A),cfg[b]);function a(e){d("AI policy blocked URL: "+e)}return!e&&""!==e||"undefined"==i[b]||(i[b]=e),i.onload=c,i.onerror=o,i.onreadystatechange=function(e,t){"loaded"!==i.readyState&&"complete"!==i.readyState||c(0,t)},cfg.ld&&cfg.ld<0?x.getElementsByTagName("head")[0].appendChild(i):setTimeout(function(){x.getElementsByTagName(E)[0].parentNode.appendChild(i)},cfg.ld||0),i};i(t)}cfg.sri&&(i=n.match(/^((http[s]?:\/\/.*\/)\w+(\.\d+){1,5})\.(([\w]+\.){0,2}js)$/))&&6===i.length?(T="".concat(i[1],".integrity.json"),a="@".concat(i[4]),S=window.fetch,t=function(e){if(!e.ext||!e.ext[a]||!e.ext[a].file)throw Error("Error Loading JSON response");var t=e.ext[a].integrity||null;c(n=i[2]+e.ext[a].file,t)},S&&!cfg.useXhr?S(T,{method:"GET",mode:"cors"}).then(function(e){return e.json()["catch"](function(){return{}})}).then(t)["catch"](o):XMLHttpRequest&&((r=new XMLHttpRequest).open("GET",T),r.onreadystatechange=function(){if(r.readyState===XMLHttpRequest.DONE)if(200===r.status)try{t(JSON.parse(r.responseText))}catch(e){o()}else o()},r.send())):n&&o();try{l.cookie=x.cookie}catch(w){}function e(e){for(;e.length;)!function(t){l[t]=function(){var e=arguments;s||l.queue.push(function(){l[t].apply(l,e)})}}(e.pop())}e(I);var v,y,S=!(l.SeverityLevel={Verbose:0,Information:1,Warning:2,Error:3,Critical:4}),T=(u.extensionConfig||{}).ApplicationInsightsAnalytics||{};return(S=!0!==u[C]&&!0!==T[C]||S)&&(e(["_"+(v="onerror")]),y=k[v],k[v]=function(e,t,n,i,a){var r=y&&y(e,t,n,i,a);return!0!==r&&l["_"+v]({message:e,url:t,lineNumber:n,columnNumber:i,error:a,evt:k.event}),r},u.autoExceptionInstrumented=!0),l}(cfg.cfg),(k[A]=e).queue&&0===e.queue.length?(e.queue.push(a)):a();})({
		src: "https://js.monitor.azure.com/scripts/b/ai.3.gbl.min.js",
		crossOrigin: "anonymous", // When supplied this will add the provided value as the cross origin attribute on the script tag
		nt: "{{{{Nonce}}}}",
		onInit: function () { window.havitBlazorAppInsights.markReady(); }, // opens the readiness gate once the SDK is fully initialized
		cfg: {{{{GetSerializedApplicationInsightsConfiguration()}}}}
		});
		""";

	private string GetSerializedApplicationInsightsConfiguration()
	{
		return System.Text.Json.JsonSerializer.Serialize(BlazorApplicationInsightsOptions.Value.JsSdkOptions, BlazorApplicationInsightsJsonSerializerContext.Default.BlazorApplicationInsightsJsSdkOptions);
	}

	/// <summary>
	/// Returns a JS statement that registers the <see cref="BlazorApplicationInsightsOptions.DefaultTelemetryInitializer"/>
	/// through the gate. Part of <see cref="GetBootstrapScript"/>, placed after the gate is defined and before
	/// <c>trackPageView</c>, so the initializer is applied even to the initial page view.
	/// Returns <c>null</c> when no <see cref="BlazorApplicationInsightsOptions.DefaultTelemetryInitializer"/> is set.
	/// </summary>
	private string GetDefaultTelemetryInitializerScript()
	{
		var defaultTelemetryInitializer = BlazorApplicationInsightsOptions.Value.DefaultTelemetryInitializer;
		if (defaultTelemetryInitializer == null)
		{
			return null;
		}

		IDictionary<string, string> defaultTelemetryInitializerTags = defaultTelemetryInitializer.GetTags();
		if (defaultTelemetryInitializerTags.Count == 0)
		{
			return null;
		}

		string serializedTags = System.Text.Json.JsonSerializer.Serialize(defaultTelemetryInitializerTags, BlazorApplicationInsightsJsonSerializerContext.Default.TelemetryInitializerDictionary);
		return $"havitBlazorAppInsights.addTelemetryInitializer({serializedTags});";
	}

	private string GetInitialTrackPageViewScript()
		=> BlazorApplicationInsightsOptions.Value.EnableInitialPageViewTracking ? "window.havitBlazorAppInsights.trackPageView({});" : null;

	/// <inheritdoc />
	public void Dispose()
	{
		// Unregister the PersistentComponentState persisting callback to avoid memory leaks
		// and prevent the callback from firing after the component has been removed.
		_persistingSubscription.Dispose();
	}
}
