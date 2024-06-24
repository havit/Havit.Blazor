export function show(element, hxOffcanvasDotnetObjectReference, closeOnEscape, scrollingEnabled, subscribeToHideEvent) {
	if (window.offcanvasElement) {
		let previousOffcanvas = bootstrap.Offcanvas.getInstance(window.offcanvasElement);
		if (previousOffcanvas) {
            // Although opening a new offcanvas should close the previous one,
			// we do not set previousOffcanvas.hidePreventionDisabled = true and force the hide() here (when handling the OnHiding event)
			// We want the developer to handle such specific scenarios on their own.
			// This might be subject to change in the future.
            previousOffcanvas.hide();
		}
	}

	if (!element) {
		return;
	}

	element.hxOffcanvasDotnetObjectReference = hxOffcanvasDotnetObjectReference;
	if (subscribeToHideEvent) {
		element.addEventListener('hide.bs.offcanvas', handleOffcanvasHide);
	}
	element.addEventListener('hidden.bs.offcanvas', handleOffcanvasHidden);
	element.addEventListener('shown.bs.offcanvas', handleOffcanvasShown);
	window.offcanvasElement = element;

	let offcanvas = new bootstrap.Offcanvas(element, {
		keyboard: closeOnEscape,
		scroll: scrollingEnabled
	});
	if (offcanvas) {
		offcanvas.show();
	}
}

export function hide(element) {
	if (!element) {
		return;
	}
	element.hxOffcanvasHiding = true;
	let o = bootstrap.Offcanvas.getInstance(element);
	if (o) {
		o.hide();
	}
}

function handleOffcanvasShown(event) {
	event.target.hxOffcanvasDotnetObjectReference.invokeMethodAsync('HxOffcanvas_HandleOffcanvasShown');
}

async function handleOffcanvasHide(event) {
	let o = bootstrap.Offcanvas.getInstance(event.target);

	if (o.hidePreventionDisabled || event.target.hxOffcanvasDisposing) {
		o.hidePreventionDisabled = false;
		return;
	}

	event.preventDefault();

	let cancel = await event.target.hxOffcanvasDotnetObjectReference.invokeMethodAsync('HxOffcanvas_HandleOffcanvasHide');
	if (!cancel) {
		o.hidePreventionDisabled = true;
		event.target.hxOffcanvasHiding = true;
		o.hide();
	}
};

function handleOffcanvasHidden(event) {
	event.target.hxOffcanvasHiding = false;

	if (event.target === window.offcanvasElement) {
		window.offcanvasElement = null;
	}

	if (event.target.hxOffcanvasDisposing) {
		// fix for #110 where the dispose() gets called while the offcanvas is still in hiding-transition
		dispose(event.target, false);
		return;
	}

	event.target.hxOffcanvasDotnetObjectReference.invokeMethodAsync('HxOffcanvas_HandleOffcanvasHidden');
}

export function dispose(element, opened) {
	if (!element) {
		return;
	}

	element.hxOffcanvasDisposing = true;

	if (element.hxOffcanvasHiding) {
		// fix for #110 where the dispose() gets called while the offcanvas is still in hiding-transition
		return;
	}

	if (opened) {
		// #110 Scrolling not working when offcanvas is removed (even if disposed is called)
		// Compensates https://github.com/twbs/bootstrap/issues/36397,
		// where the o.dispose() does not reset the ScrollBarHelper() and the scrolling remains deactivated.
		// The dispose() is re-called from hidden.bs.offcanvas event handler.
		// Remove when the issue is fixed.
		hide(element);
		return;
	}

	element.removeEventListener('hidden.bs.offcanvas', handleOffcanvasHidden);
	element.removeEventListener('shown.bs.offcanvas', handleOffcanvasShown);
	element.hxOffcanvasDotnetObjectReference = null;

	let o = bootstrap.Offcanvas.getInstance(element);
	if (o) {
		o.dispose();
	}

	if (element === window.offcanvasElement) { // another offcanvas might be already open
		window.offcanvasElement = null;
	}
}