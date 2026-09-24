// Havit.Blazor.ApplicationInsights - bootstrap of the Application Insights JavaScript SDK.
// Snippet source: https://github.com/microsoft/ApplicationInsights-JS?tab=readme-ov-file#snippet-setup-ignore-if-using-npm-setup
//
// Deliberately no import/export: the very same file is emitted inline as a classic <script> by HxApplicationInsights
// in static SSR / prerendering, and imported as an ES module (side effects only) by the interactive render modes.
// Everything lives on window.havitBlazorAppInsights so that both copies share one state - an inline script cannot
// reach a module's scope, and the object is also what IBlazorApplicationInsights talks to.
window.havitBlazorAppInsights = window.havitBlazorAppInsights || (function () {
	var readyResolve;
	// Resolves to true once the SDK reports initialization, to false when the gate had to open on the fallback timeout.
	var ready = new Promise(function (resolve) { readyResolve = resolve; });

	// If the SDK script never loads (all CDN fallbacks exhausted) - or HxApplicationInsights never initializes the snippet -
	// onInit is never invoked. Open the gate anyway so that the calls do not hang forever.
	var readyTimeout = setTimeout(function () { readyResolve(false); }, 15000);

	var initialized = false;

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
			// Only the real SDK is ever called (core exists once it initialized). After the fallback opened the gate, the snippet
			// may still be retrying CDN hosts with its stubs in place - a call landing on the stub-to-SDK handover is the very
			// failure this gate exists to prevent, so such calls are dropped instead. Should the SDK show up later, calls pass again.
			if (appInsights && appInsights.core && appInsights[name]) {
				return appInsights[name].apply(appInsights, args);
			}
		});
	}

	var api = {
		ready: ready,

		markReady: function () {
			clearTimeout(readyTimeout);
			readyResolve(true);
		},

		addTelemetryInitializer: function (tags) {
			return invoke("addTelemetryInitializer", [function (item) {
				for (var key in tags) {
					if (Object.prototype.hasOwnProperty.call(tags, key)) { item.tags[key] = tags[key]; }
				}
			}]);
		},

		// Runs the snippet (which downloads the SDK) and enqueues the default telemetry initializer and the initial page view.
		// Idempotent - the inline SSR script and the interactive component may both get to call it.
		// config / telemetryInitializerTags accept an object (inline script) or a JSON string (JS interop).
		initialize: function (config, nonce, telemetryInitializerTags, trackInitialPageView) {
			if (initialized) {
				return;
			}
			initialized = true;

			var snippetConfig = {
				src: "https://js.monitor.azure.com/scripts/b/ai.3.gbl.min.js",
				crossOrigin: "anonymous", // added as the crossorigin attribute of the SDK script tag
				nt: nonce, // added as the nonce attribute of the SDK script tag
				onInit: function () { api.markReady(); }, // opens the readiness gate once the SDK is fully initialized
				cfg: (typeof config === "string") ? JSON.parse(config) : config
			};
			!(function (cfg){var k,x,D,E,L,C,b,U,O,A,e,t="track",n="TrackPage",i="TrackEvent",I=[t+"Event",t+"Exception",t+"PageView",t+"PageViewPerformance","addTelemetryInitializer",t+"Trace",t+"DependencyData",t+"Metric","start"+n,"stop"+n,"start"+i,"stop"+i,"setAuthenticatedUserContext","clearAuthenticatedUserContext","flush"];function a(){cfg.onInit&&cfg.onInit(e)}k=window,x=document,D=k.location,E="script",L="ingestionendpoint",C="disableExceptionTracking",b="crossOrigin",U="POST",O=cfg.pn||"aiPolicy",t="appInsightsSDK",A=cfg.name||"appInsights",(cfg.name||k[t])&&(k[t]=A),e=k[A]||function(u){var n=u.url||cfg.src,s=!1,p=!1,l={initialize:!0,queue:[],sv:"10",config:u,version:2,extensions:void 0};function d(e){var t,n,i,a,r,o,c,s;!0!==cfg.dle&&(o=(t=function(){var e,t={},n=u.connectionString;if("string"==typeof n&&n)for(var i=n.split(";"),a=0;a<i.length;a++){var r=i[a].split("=");2===r.length&&(t[r[0].toLowerCase()]=r[1])}return t[L]||(e=(n=t.endpointsuffix)?t.location:null,t[L]="https://"+(e?e+".":"")+"dc."+(n||"services.visualstudio.com")),t}()).instrumentationkey||u.instrumentationKey||"",t=(t=(t=t[L])&&"/"===t.slice(-1)?t.slice(0,-1):t)?t+"/v2/track":u.endpointUrl,t=u.userOverrideEndpointUrl||t,(n=[]).push((i="SDK LOAD Failure: Failed to load Application Insights SDK script (See stack for details)",a=e,c=t,(s=(r=f(o,"Exception")).data).baseType="ExceptionData",s.baseData.exceptions=[{typeName:"SDKLoadFailed",message:i.replace(/\./g,"-"),hasFullStack:!1,stack:i+"\nSnippet failed to load ["+a+"] -- Telemetry is disabled\nHelp Link: https://go.microsoft.com/fwlink/?linkid=2128109\nHost: "+(D&&D.pathname||"_unknown_")+"\nEndpoint: "+c,parsedStack:[]}],r)),n.push((s=e,i=t,(c=(a=f(o,"Message")).data).baseType="MessageData",(r=c.baseData).message='AI (Internal): 99 message:"'+("SDK LOAD Failure: Failed to load Application Insights SDK script (See stack for details) ("+s+")").replace(/\"/g,"")+'"',r.properties={endpoint:i},a)),e=n,o=t,JSON&&((c=k.fetch)&&!cfg.useXhr?c(o,{method:U,body:JSON.stringify(e),mode:"cors"}):XMLHttpRequest&&((s=new XMLHttpRequest).open(U,o),s.setRequestHeader("Content-type","application/json"),s.send(JSON.stringify(e)))))}function f(e,t){return e=e,t=t,i=l.sv,a=l.version,r=D,(o={})["ai.device."+"id"]="browser",o["ai.device.type"]="Browser",o["ai.operation.name"]=r&&r.pathname||"_unknown_",o["ai.internal.sdkVersion"]="javascript:snippet_"+(i||a),{time:(r=new Date).getUTCFullYear()+"-"+n(1+r.getUTCMonth())+"-"+n(r.getUTCDate())+"T"+n(r.getUTCHours())+":"+n(r.getUTCMinutes())+":"+n(r.getUTCSeconds())+"."+(r.getUTCMilliseconds()/1e3).toFixed(3).slice(2,5)+"Z",iKey:e,name:"Microsoft.ApplicationInsights."+e.replace(/-/g,"")+"."+t,sampleRate:100,tags:o,data:{baseData:{ver:2}},ver:undefined,seq:"1",aiDataContract:undefined};function n(e){e=""+e;return 1===e.length?"0"+e:e}var i,a,r,o}var i,a,t,r,g=-1,h=0,m=["js.monitor.azure.com","js.cdn.applicationinsights.io","js.cdn.monitor.azure.com","js0.cdn.applicationinsights.io","js0.cdn.monitor.azure.com","js2.cdn.applicationinsights.io","js2.cdn.monitor.azure.com","az416426.vo.msecnd.net"],o=function(){return c(n,null)};function c(t,r){if((n=navigator)&&(~(n=(n.userAgent||"").toLowerCase()).indexOf("msie")||~n.indexOf("trident/"))&&~t.indexOf("ai.3")&&(t=t.replace(/(\/)(ai\.3\.)([^\d]*)$/,function(e,t,n){return t+"ai.2"+n})),!1!==cfg.cr)for(var e=0;e<m.length;e++)if(0<t.indexOf(m[e])){g=e;break}var n,o=function(e){var a;l.queue=[],p||(0<=g&&h+1<m.length?(a=(g+h+1)%m.length,i(t.replace(/^(.*\/\/)([\w\.]*)(\/.*)$/,function(e,t,n,i){return t+m[a]+i})),h+=1):(s=p=!0,d(t)))},c=function(e,t){p||setTimeout(function(){t&&!l.core&&o()},500),s=!1},i=function(e){var n,i=x.createElement(E),e=(cfg.pl?cfg.ttp&&cfg.ttp.createScript?i.src=cfg.ttp.createScriptURL(e):i.src=(null==(n=window.trustedTypes)?void 0:n.createPolicy(O,{createScriptURL:function(e){try{var t=new URL(e);if(t.host&&"js.monitor.azure.com"===t.host)return e;a(e)}catch(n){a(e)}}})).createScriptURL(e):i.src=e,cfg.nt&&i.setAttribute("nonce",cfg.nt),r&&(i.integrity=r),i.setAttribute("data-ai-name",A),cfg[b]);function a(e){d("AI policy blocked URL: "+e)}return!e&&""!==e||"undefined"==i[b]||(i[b]=e),i.onload=c,i.onerror=o,i.onreadystatechange=function(e,t){"loaded"!==i.readyState&&"complete"!==i.readyState||c(0,t)},cfg.ld&&cfg.ld<0?x.getElementsByTagName("head")[0].appendChild(i):setTimeout(function(){x.getElementsByTagName(E)[0].parentNode.appendChild(i)},cfg.ld||0),i};i(t)}cfg.sri&&(i=n.match(/^((http[s]?:\/\/.*\/)\w+(\.\d+){1,5})\.(([\w]+\.){0,2}js)$/))&&6===i.length?(T="".concat(i[1],".integrity.json"),a="@".concat(i[4]),S=window.fetch,t=function(e){if(!e.ext||!e.ext[a]||!e.ext[a].file)throw Error("Error Loading JSON response");var t=e.ext[a].integrity||null;c(n=i[2]+e.ext[a].file,t)},S&&!cfg.useXhr?S(T,{method:"GET",mode:"cors"}).then(function(e){return e.json()["catch"](function(){return{}})}).then(t)["catch"](o):XMLHttpRequest&&((r=new XMLHttpRequest).open("GET",T),r.onreadystatechange=function(){if(r.readyState===XMLHttpRequest.DONE)if(200===r.status)try{t(JSON.parse(r.responseText))}catch(e){o()}else o()},r.send())):n&&o();try{l.cookie=x.cookie}catch(w){}function e(e){for(;e.length;)!function(t){l[t]=function(){var e=arguments;s||l.queue.push(function(){l[t].apply(l,e)})}}(e.pop())}e(I);var v,y,S=!(l.SeverityLevel={Verbose:0,Information:1,Warning:2,Error:3,Critical:4}),T=(u.extensionConfig||{}).ApplicationInsightsAnalytics||{};return(S=!0!==u[C]&&!0!==T[C]||S)&&(e(["_"+(v="onerror")]),y=k[v],k[v]=function(e,t,n,i,a){var r=y&&y(e,t,n,i,a);return!0!==r&&l["_"+v]({message:e,url:t,lineNumber:n,columnNumber:i,error:a,evt:k.event}),r},u.autoExceptionInstrumented=!0),l}(cfg.cfg),(k[A]=e).queue&&0===e.queue.length?(e.queue.push(a)):a();})(snippetConfig);

			// Both enqueued right here, atomically with the creation of the gate: no call made by the application
			// can be ordered before them (the E2E tests rely on that, see InitialPageViewTrackingTests).
			if (telemetryInitializerTags) {
				api.addTelemetryInitializer((typeof telemetryInitializerTags === "string") ? JSON.parse(telemetryInitializerTags) : telemetryInitializerTags);
			}
			if (trackInitialPageView) {
				api.trackPageView({});
			}
		}
	};

	["trackEvent", "trackPageView", "trackException", "trackTrace", "trackMetric", "trackPageViewPerformance", "trackDependencyData", "startTrackPage", "stopTrackPage", "startTrackEvent", "stopTrackEvent", "setAuthenticatedUserContext", "clearAuthenticatedUserContext", "flush"].forEach(function (name) {
		api[name] = function () { return invoke(name, arguments); };
	});

	return api;
})();
