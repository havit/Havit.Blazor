import HxToast from './HxToast.js?v=4.6.16';

export function afterWebStarted(blazor) {
	console.debug('Havit.Blazor.Components.Web.Bootstrap.lib.module.js: afterWebStarted');
	blazor.addEventListener('enhancedload', onEnhancedLoad);
}

function onEnhancedLoad() {
	console.debug('Havit.Blazor.Components.Web.Bootstrap.lib.module.js: onEnhancedLoad');
	// on client-side navigation, we assume all toasts in DOM can be shown
	// if this turns out to be incorrect, we can show only toasts with specific class or toasts without an instance
	// (the explicit HxToast.js:init() will not show these toasts again, if the instance already exists)
	activateToasts();
}

function activateToasts() {
	for (const element of document.querySelectorAll('.hx-toast-init')) {
		HxToast.init(element);
	}
}