// TODO Add MutationObserver to cleanup the toast when it is removed from the DOM (especially when using SSR) - waiting for https://github.com/dotnet/AspNetCore.Docs/issues/33842

// !! When updating this file, update also import in Havit.Blazor.Components.Web.Bootstrap.lib.module.js
export function init(element, hxToastDotnetObjectReference) {
	if (!element) {
		return;
	}

	if (hxToastDotnetObjectReference) {
		// interactive render mode only
		element.hxToastDotnetObjectReference = hxToastDotnetObjectReference;
		element.addEventListener('hidden.bs.toast', handleToastHidden);
	}

	// the instance can be already created and shown by module activation (lib.module.js > onEnhancedLoad > activateToasts)
	// which helps to initialize toasts on static SSR (incl. prerendering!)
	// we do not expect the single toast element to be shown multiple times
	let toast = bootstrap.Toast.getInstance(element);
	if (!toast) {
		toast = new bootstrap.Toast(element);
		toast.show();
		console.log('HxToast.init: Toast instance created and shown.', element);
	}
	else if (toast._element.classList.contains('hx-toast-init')) {
		// for SSR enahanced forms, when merging DOM changes, Blazor sometimes reuses the original element
		// (currently not being present in DOM but returned to DOM within patching process)
		// in this case, the Bootstrap Toast instance might already exist, but the element is not shown
		// The .hx-toast-init class indicates that the element is not shown yet.
		toast.show();
		console.log('HxToast.init: Toast shown (existing instance).', element);
	}

	element.classList.remove('hx-toast-init');
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