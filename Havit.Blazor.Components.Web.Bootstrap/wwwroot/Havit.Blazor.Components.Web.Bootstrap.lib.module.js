import HxToast from './HxToast.js?v=4.6.17';

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