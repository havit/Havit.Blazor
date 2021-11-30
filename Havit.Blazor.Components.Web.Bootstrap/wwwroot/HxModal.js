export function show(element, hxModalDotnetObjectReference, useStaticBackdrop, closeOnEscape) {
	if (!element) {
		return;
	}

	element.hxModalDotnetObjectReference = hxModalDotnetObjectReference;
	element.addEventListener('hidden.bs.modal', handleModalHidden)

	var modal = new bootstrap.Modal(element, {
		backdrop: useStaticBackdrop ? "static" : true,
		keyboard: closeOnEscape
	});
	if (modal) {
		modal.show();
	}
}

export function hide(element) {
	var modal = bootstrap.Modal.getInstance(element);
	if (modal) {
		modal.hide();
	}
}

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('hidden.bs.modal', handleModalHidden);
	element.hxModalDotnetObjectReference = null;

	var modal = bootstrap.Modal.getInstance(element);
	if (modal) {
		modal.hide();
		modal.dispose();
	}
}

function handleModalHidden(event) {
	event.target.hxModalDotnetObjectReference.invokeMethodAsync('HxModal_HandleModalHidden');
	dispose(event.target);
};
