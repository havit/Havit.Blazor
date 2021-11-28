export function show(element, hxOffcanvasDotnetObjectReference, backdropEnabled, closeOnEscape, scrollingEnabled) {
	if (window.offcanvasElement) {
		let previousOffcanvas = bootstrap.Offcanvas.getInstance(window.offcanvasElement);
		if (previousOffcanvas) {
			previousOffcanvas.hide();
		}
	}
	element.hxOffcanvasDotnetObjectReference = hxOffcanvasDotnetObjectReference;
	element.addEventListener('hidden.bs.offcanvas', handleOffcanvasHidden)

	let offcanvas = new bootstrap.Offcanvas(element, {
		backdrop: backdropEnabled,
		keyboard: closeOnEscape,
		scroll: scrollingEnabled
	});
	window.offcanvasElement = element;
	offcanvas.show();
}

export function hide(element) {
	let offcanvas = bootstrap.Offcanvas.getInstance(element);
	offcanvas.hide();
}

export function dispose(element) {
	let offcanvas = bootstrap.Offcanvas.getInstance(element);
	element.removeEventListener('hidden.bs.offcanvas', handleOffcanvasHidden);
	element.hxOffcanvasDotnetObjectReference = null;
	offcanvas.hide();
	// offcanvas.dispose(); // BS bug: offcanvas.js: 145: Cannot read property 'setAttribute' of null

	if (element === window.offcanvasElement) { // another offcanvas might be already open
		window.offcanvasElement = null;
		console.warn("dispose-null");
	}
}

function handleOffcanvasHidden(event) {
	event.target.removeEventListener('hidden.bs.offcanvas', handleOffcanvasHidden);
	event.target.hxOffcanvasDotnetObjectReference.invokeMethodAsync('HxOffcanvas_HandleOffcanvasHidden');
	event.target.hxOffcanvasDotnetObjectReference = null;

	let offcanvas = bootstrap.Offcanvas.getInstance(event.target);

	if (event.target === window.offcanvasElement) { // another offcanvas might be already open
		window.offcanvasElement = null;
	}
	offcanvas.dispose();
};
