import * as HxToast from './HxToast.js';
import * as HxInputDate from './HxInputDate.js';
import * as HxInputNumber from './HxInputNumber.js';
import * as HxSidebar from './HxSidebar.js';

// Delegated (document-level) event listeners replace inline event-handler attributes (onfocus="...", etc.),
// which are blocked by a strict Content Security Policy (#1730).
// Components render data-hx-* marker attributes only, the behavior is driven by the presence of the attribute at the time of the event.
// Registered when the module is loaded (works for Blazor Web App, standalone WebAssembly and legacy Server alike).
registerSelectOnFocus();
HxInputDate.register();
HxInputNumber.register();
HxSidebar.register();

// Bootstrap 6 is ESM-only; the Hx* JS modules consume it through the window.bootstrap bridge.
// Awaiting the import here (Blazor awaits the before*Start initializers before starting the runtime)
// guarantees the bridge exists before any component JS interop can run - the script emitted by
// HxSetup.RenderBootstrapJavaScriptReference() is a deferred module and can lose that race on first load.
function ensureBootstrapBridgeAsync() {
	// The promise slot is claimed synchronously (??=) so this initializer and the script emitted by
	// HxSetup.RenderBootstrapJavaScriptReference() can never both import the bundle (two module
	// instances would duplicate the Bootstrap data-API listeners). Whichever runs first wins;
	// the other awaits the same promise.
	// window.bootstrap check: best-effort guard against a legacy HxSetup script (pre-promise-slot) that
	// sets the global without claiming the slot; a deferred legacy module can still lose this race -
	// consumers should update to the current RenderBootstrapJavaScriptReference() output.
	window.havitBlazorBootstrapReady ??= window.bootstrap
		? Promise.resolve()
		: import('./bootstrap.bundle.min.js').then(m => { window.bootstrap = m; });
	return window.havitBlazorBootstrapReady;
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

function registerSelectOnFocus() {
	// focusin (not focus) to keep the original timing - after Blazor's @onfocus handlers (HxAutosuggest)
	document.addEventListener('focusin', (event) => {
		if (event.target.hasAttribute?.('data-hx-select-on-focus')) {
			event.target.select();
		}
	});
}
