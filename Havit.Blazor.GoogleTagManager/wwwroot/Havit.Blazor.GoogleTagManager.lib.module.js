// JS initializer - loaded automatically by Blazor, no <script> reference needed.
// Adds Google Tag Manager support for static server-side rendering (static SSR) with enhanced navigation,
// where no interactive runtime exists to observe NavigationManager.LocationChanged.
import { initialize, pushPageViewEventOnce } from './HxGoogleTagManager.js';

// Blazor Web App (blazor.web.js).
export function afterWebStarted(blazor) {
	start(blazor);
}

// Standalone WebAssembly (blazor.webassembly.js) and classic Blazor Server (blazor.server.js)
// do not raise afterWebStarted. Only one of the two callbacks is ever invoked for a given host.
export function afterStarted(blazor) {
	start(blazor);
}

function start(blazor) {
	// enhancedload is not raised for the page the app started on.
	trackCurrentPage();

	blazor.addEventListener('enhancedload', trackCurrentPage);
}

function trackCurrentPage() {
	const config = window.hxGoogleTagManager?.config;
	if (!config) {
		// HxGoogleTagManagerPageViewTracker is not rendered on this page, or it is rendered
		// interactively and tracks page-views through NavigationManager.LocationChanged instead.
		return;
	}

	initialize(config.gtmId);
	pushPageViewEventOnce(config.pageViewEventName, config.pageViewUrlVariableName, location.href, null);
}
