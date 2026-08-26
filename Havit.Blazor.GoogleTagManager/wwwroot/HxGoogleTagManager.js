// Lives on window because the inline snippet rendered by HxGoogleTagManagerPageViewTracker during
// static SSR is a plain <script> and cannot reach this module's scope. Both have to agree on whether
// GTM is already running and which page-view was tracked last.
const state = window.hxGoogleTagManager = window.hxGoogleTagManager || { initialized: false, config: null, lastPageViewUrl: null, initialPageViewHandled: false };

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

// One navigation reaches automatic tracking more than once: enhancedload is raised for every enhanced
// page update including streaming ones, and an interactive HxGoogleTagManagerPageViewTracker reacts to
// the same navigation. Explicit IHxGoogleTagManager.PushPageViewAsync() calls do not come through here.
export function pushPageViewEventOnce(eventName, urlVariableName, url, eventData, trackInitialPageView) {
	if (url === state.lastPageViewUrl) {
		return;
	}

	const isInitialPageView = !state.initialPageViewHandled;
	state.initialPageViewHandled = true;

	if (isInitialPageView && (trackInitialPageView === false)) {
		// Suppressed but still remembered, otherwise the next enhancedload for the same URL would look
		// like a navigation and push it after all.
		state.lastPageViewUrl = url;
		return;
	}

	pushPageViewEvent(eventName, urlVariableName, url, eventData);
}
