// Shared state so that every entry point converges on a single GTM initialization
// and a single automatic page-view per URL:
//  - the inline snippet rendered by HxGoogleTagManagerPageViewTracker during static SSR/prerendering,
//  - the JS initializer (Havit.Blazor.GoogleTagManager.lib.module.js) reacting to enhanced navigation,
//  - the IHxGoogleTagManager service used from interactive rendering.
// The inline snippet creates the very same object, so whichever runs first wins and the others join in.
const state = (window.hxGoogleTagManager = window.hxGoogleTagManager || { initialized: false, config: null, lastPageViewUrl: null });

export function initialize(GTMID) {
	if (state.initialized) {
		return;
	}
	state.initialized = true;

	(function (w, d, s, l, i) {
		w[l] = w[l] || [];
		w[l].push({
			"gtm.start": new Date().getTime(),
			event: "gtm.js",
		});
		const f = d.getElementsByTagName("head")[0],
			j = d.createElement(s),
			dl = l !== "dataLayer" ? "&l=" + l : "";
		j.async = true;
		j.src = "https://www.googletagmanager.com/gtm.js?id=" + i + dl;
		f.appendChild(j);
		dataLayer.push({ event: "pageview" });
		window.isGTM = true;
	})(window, document, "script", "dataLayer", GTMID);
}

export function push(data) {
	dataLayer.push(data);
	console.debug("GTM:" + JSON.stringify(data));
}

export function pushEvent(eventName, eventData) {
	if (eventData === null) {
		eventData = new Object();
	}
	eventData['event'] = eventName;
	push(eventData);
}

export function pushPageViewEvent(eventName, urlVariableName, url, eventData) {
	if (eventData === null) {
		eventData = new Object();
	}
	eventData[urlVariableName] = url;
	eventData['event'] = eventName;
	state.lastPageViewUrl = url;
	push(eventData);
}

// Used by the automatic tracking paths (JS initializer + HxGoogleTagManagerPageViewTracker).
// A single navigation can reach us more than once - the enhancedload event is raised for every
// enhanced page update including streaming updates, and in a Blazor Web App an interactive tracker
// can react to the very same navigation - so the URL last tracked is remembered and repeats are ignored.
// Explicit IHxGoogleTagManager.PushPageViewAsync() calls bypass this and always push.
export function pushPageViewEventOnce(eventName, urlVariableName, url, eventData) {
	if (url === state.lastPageViewUrl) {
		return;
	}
	pushPageViewEvent(eventName, urlVariableName, url, eventData);
}
