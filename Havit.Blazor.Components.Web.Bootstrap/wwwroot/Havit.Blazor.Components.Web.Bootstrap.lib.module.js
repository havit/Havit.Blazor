import HxToast from './HxToast.js';

// Bootstrap 6 is ESM-only; the Hx* JS modules consume it through the window.bootstrap bridge.
// Awaiting the import here (Blazor awaits the before*Start initializers before starting the runtime)
// guarantees the bridge exists before any component JS interop can run - the script emitted by
// HxSetup.RenderBootstrapJavaScriptReference() is a deferred module and can lose that race on first load.
async function ensureBootstrapBridgeAsync() {
	if (!window.bootstrap) {
		window.bootstrap = await import('./bootstrap.bundle.min.js');
	}
}

// Blazor Web App (blazor.web.js)
export function beforeWebStart() {
	return ensureBootstrapBridgeAsync();
}

// Blazor Server / WebAssembly (blazor.server.js / blazor.webassembly.js)
export function beforeStart() {
	return ensureBootstrapBridgeAsync();
}

export function afterWebStarted(blazor) {
	console.debug('Havit.Blazor.Components.Web.Bootstrap.lib.module.js: afterWebStarted');

	blazor.addEventListener('enhancedload', onEnhancedLoad);

	activateToasts();  // onEnhancedLoad is not called when enhanced navigation is not used
}

function onEnhancedLoad() {
	console.debug('Havit.Blazor.Components.Web.Bootstrap.lib.module.js: onEnhancedLoad');

	activateToasts(); // idempotent 
}

function activateToasts() {
	// idempotent implementation required
	for (const element of document.querySelectorAll('.hx-toast-init')) {
		HxToast.init(element);
	}
}