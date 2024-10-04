// !! When updating this file, update also import in Havit.Blazor.Components.Web.Bootstrap.lib.module.js
export function init(element, hxToastDotnetObjectReference) {
	if (!element) {
		return;
	}

	element.classList.remove('hx-toast-init');

	if (hxToastDotnetObjectReference) {
		// interactive render mode only
		element.hxToastDotnetObjectReference = hxToastDotnetObjectReference;
		element.addEventListener('hidden.bs.toast', handleToastHidden);
	}

	// we create and show the toast only if it is not already shown
	// the instance can be already created and shown by module activation (lib.module.js > onEnhancedLoad > activateToasts)
	// which helps to show toasts on static SSR (incl. prerendering!)
	// we do not expect the toast to be in DOM without being shown immediately
	// we do not expect the single toast element to be shown multiple times
	let toast = bootstrap.Toast.getInstance(element);
	if (!toast) {
		toast = new bootstrap.Toast(element);
		toast.show();
		console.log('HxToast.init: Toast created and shown.', element);
	}
}

function handleToastHidden(event) {
	event.target.hxToastDotnetObjectReference.invokeMethodAsync('HxToast_HandleToastHidden');
};

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('hidden.bs.toast', handleToastHidden);
	element.hxToastDotnetObjectReference = null;

	const t = bootstrap.Toast.getInstance(element);
	if (t) {
		t.dispose();
	}
}

const HxToast = {
	init,
	dispose
};

export default HxToast;