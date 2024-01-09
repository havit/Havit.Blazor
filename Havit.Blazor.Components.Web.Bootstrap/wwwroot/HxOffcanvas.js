export function show(element, hxOffcanvasDotnetObjectReference, closeOnEscape, scrollingEnabled) {
	if (window.offcanvasElement) {
		let previousOffcanvas = bootstrap.Offcanvas.getInstance(window.offcanvasElement);
		if (previousOffcanvas) {
			previousOffcanvas.hide();
		}
	}

	if (!element) {
		return;
	}

	element.hxOffcanvasDotnetObjectReference = hxOffcanvasDotnetObjectReference;
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

function handleOffcanvasHidden(event) {
	event.target.hxOffcanvasHiding = false;

	if (event.target.hxOffcanvasDisposing) {
		// fix for #110 where the dispose() gets called while the offcanvas is still in hiding-transition
		dispose(event.target);
		return;
	}

	event.target.hxOffcanvasDotnetObjectReference.invokeMethodAsync('HxOffcanvas_HandleOffcanvasHidden');
}

export function dispose(element) {
	if (!element) {
		return;
	}

	element.hxOffcanvasDisposing = true;

	if (element.hxOffcanvasHiding) {
		// fix for #110 where the dispose() gets called while the offcanvas is still in hiding-transition
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