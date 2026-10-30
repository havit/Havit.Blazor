import HxToast from './HxToast.js';
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
