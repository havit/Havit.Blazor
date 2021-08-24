export function initialize(GTMID) {
	(function (w, d, s, l, i) {
		w[l] = w[l] || [];
		w[l].push({
			"gtm.start": new Date().getTime(),
			event: "gtm.js",
		});
		var f = d.getElementsByTagName("head")[0],
			j = d.createElement(s),
			dl = l !== "dataLayer" ? "&l=" + l : "";
		j.async = true;
		j.src = "https://www.googletagmanager.com/gtm.js?id=" + i + dl;
		f.appendChild(j, f);
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
	push(eventData);
}