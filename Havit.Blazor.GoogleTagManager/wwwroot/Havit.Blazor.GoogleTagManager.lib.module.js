// Tracks page-views for static SSR, where there is no interactive runtime to observe
// NavigationManager.LocationChanged.
import { initialize, pushPageViewEventOnce } from './HxGoogleTagManager.js';

// Blazor Web App.
export function afterWebStarted(blazor) {
	start(blazor);
}

// Blazor Server and standalone WebAssembly apps.
export function afterStarted(blazor) {
	start(blazor);
}

function start(blazor) {
	trackCurrentPage(); // enhancedload is not raised for the page the app started on

	blazor.addEventListener('enhancedload', trackCurrentPage);
}

function trackCurrentPage() {
	const config = window.hxGoogleTagManager?.config;
	if (!config) {
		return; // no HxGoogleTagManagerPageViewTracker was rendered on the server, so there is nothing to track from here
	}

	initialize(config.gtmId);
	pushPageViewEventOnce(config.pageViewEventName, config.pageViewUrlVariableName, location.href, null, config.enableInitialPageViewTracking);
}
