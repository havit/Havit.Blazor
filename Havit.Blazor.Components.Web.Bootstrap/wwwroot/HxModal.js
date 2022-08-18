export function show(element, hxModalDotnetObjectReference, useStaticBackdrop, closeOnEscape) {
	if (!element) {
		return;
	}

	element.hxModalDotnetObjectReference = hxModalDotnetObjectReference;
	element.addEventListener('hidden.bs.modal', handleModalHidden);
	element.addEventListener('shown.bs.modal', handleModalShown);

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

function handleModalShown(event) {
	event.target.hxModalDotnetObjectReference.invokeMethodAsync('HxModal_HandleModalShown');
};

function handleModalHidden(event) {
	event.target.hxModalDotnetObjectReference.invokeMethodAsync('HxModal_HandleModalHidden');
	dispose(event.target);
};

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('hidden.bs.modal', handleModalHidden);
	element.removeEventListener('shown.bs.modal', handleModalHidden);
	element.hxModalDotnetObjectReference = null;

	var modal = bootstrap.Modal.getInstance(element);
	if (modal) {
		modal.dispose();
	}
}