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
	}
	else if (toast._element.classList.contains('hx-toast-init')) {
		// for SSR enahanced forms, when merging DOM changes, Blazor sometimes reuses the original element
		// (currently not being present in DOM but returned to DOM within patching process)
		// in this case, the Bootstrap Toast instance might already exist, but the element is not shown
		// The .hx-toast-init class indicates that the element is not shown yet.
		toast.show();
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

// Modal dialogs and drawers (native <dialog> opened with showModal()) are rendered in the browser top layer, above any z-index.
// To keep the toasts visible, the positioned toast containers are (re)moved to the top layer (popover="manual") whenever a dialog
// or a drawer opens - the element shown last is the topmost one.
// The container stays in the normal document (position: fixed) until then, so the toasts work without JavaScript as well (static SSR).
function raiseToastContainers() {
	for (const container of document.querySelectorAll('.hx-toast-container-fixed')) {
		if (typeof container.showPopover !== 'function') {
			return; // popover API not supported
		}
		if (!container.hasAttribute('popover')) {
			container.setAttribute('popover', 'manual');
		}
		if (container.matches(':popover-open')) {
			container.hidePopover();
		}
		container.showPopover();
	}
}

function handleDialogShow() {
	// show.bs.* is triggered just before the dialog is opened (showModal() runs synchronously right after the event)
	setTimeout(raiseToastContainers);
}

// the module can be loaded more than once (lib.module.js + component import), register the listeners only once
if (!window.hxToastTopLayerListenersRegistered) {
	window.hxToastTopLayerListenersRegistered = true;
	document.addEventListener('show.bs.dialog', handleDialogShow);
	document.addEventListener('show.bs.drawer', handleDialogShow);
}

const HxToast = {
	init,
	dispose
};

export default HxToast;