export function show(element, hxOffcanvasDotnetObjectReference, backdropEnabled, closeOnEscape, scrollingEnabled) {
	element.hxOffcanvasDotnetObjectReference = hxOffcanvasDotnetObjectReference;
	element.addEventListener('hidden.bs.offcanvas', handleOffcanvasHidden)

    var offcanvas = new bootstrap.Offcanvas(element, {
		backdrop: backdropEnabled,
		keyboard: closeOnEscape,
		scroll: scrollingEnabled
    });
    offcanvas.show();
}

export function hide(element) {
    var offcanvas = bootstrap.Offcanvas.getInstance(element);
    offcanvas.hide();
}

export function dispose(element) {
    var offcanvas = bootstrap.Offcanvas.getInstance(element);
    element.removeEventListener('hidden.bs.offcanvas', handleOffcanvasHidden);
    element.hxOffcanvasDotnetObjectReference = null;
    offcanvas.hide();
    //offcanvas.dispose(); // modal.js:318 Uncaught TypeError: Cannot read property 'style' of null
}

function handleOffcanvasHidden(event) {
	event.target.removeEventListener('hidden.bs.offcanvas', handleOffcanvasHidden);
	event.target.hxOffcanvasDotnetObjectReference.invokeMethodAsync('HxOffcanvas_HandleOffcanvasHidden');
	event.target.hxOffcanvasDotnetObjectReference = null;

    var offcanvas = bootstrap.Offcanvas.getInstance(event.target);    
    offcanvas.dispose();
};
