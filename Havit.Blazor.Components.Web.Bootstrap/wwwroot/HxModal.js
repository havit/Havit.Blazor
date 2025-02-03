export function show(element, hxModalDotnetObjectReference, closeOnEscape, subscribeToHideEvent) {
	if (window.modalElement) {
		const previousModal = bootstrap.Modal.getInstance(window.modalElement);
		if (previousModal) {
			previousModal.hide();
		}
	}

	if (!element) {
		return;
	}

	element.hxModalDotnetObjectReference = hxModalDotnetObjectReference;
	if (subscribeToHideEvent) {
		element.addEventListener('hide.bs.modal', handleModalHide);
	}
	element.addEventListener('hidden.bs.modal', handleModalHidden);
	element.addEventListener('shown.bs.modal', handleModalShown);
	window.modalElement = element;

	const modal = new bootstrap.Modal(element, {
		keyboard: closeOnEscape
	});
	if (modal) {
		modal.show();
	}
}

export function hide(element) {
	if (!element) {
		return;
	}
	element.hxModalHiding = true;
	const modal = bootstrap.Modal.getInstance(element);
	if (modal) {
		modal.hide();
	}
}

function handleModalShown(event) {
	event.target.hxModalDotnetObjectReference.invokeMethodAsync('HxModal_HandleModalShown');
}

async function handleModalHide(event) {
	const modalInstance = bootstrap.Modal.getInstance(event.target);

	if (modalInstance.hidePreventionDisabled || event.target.hxModalDisposing) {
		modalInstance.hidePreventionDisabled = false;
		return;
	}

    event.preventDefault();

	const cancel = await event.target.hxModalDotnetObjectReference.invokeMethodAsync('HxModal_HandleModalHide');
    if (!cancel) {
		modalInstance.hidePreventionDisabled = true;
		event.target.hxModalHiding = true;
		modalInstance.hide();
    }
}

function handleModalHidden(event) {
	event.target.hxModalHiding = false;

	if (event.target.hxModalDisposing) {
		// fix for #110 where the dispose() gets called while the modal is still in hiding-transition
		dispose(event.target, false);
		return;
	}

	event.target.hxModalDotnetObjectReference.invokeMethodAsync('HxModal_HandleModalHidden');
}

export function dispose(element, opened) {
	if (!element) {
		return;
	}

	element.hxModalDisposing = true;

	if (element.hxModalHiding) {
		// fix for #110 where the dispose() gets called while the modal is still in hiding-transition
		return;
	}

	if (opened) {
		// #110 Scrolling not working when modal is removed (even if disposed is called)
		// Compensates https://github.com/twbs/bootstrap/issues/36397,
		// where the o.dispose() does not reset the ScrollBarHelper() and the scrolling remains deactivated.
		// The dispose() is re-called from hidden.bs.modal event handler.
		// Remove when the issue is fixed.
		hide(element);
		return;
	}

	element.removeEventListener('hide.bs.modal', handleModalHide);
	element.removeEventListener('hidden.bs.modal', handleModalHidden);
	element.removeEventListener('shown.bs.modal', handleModalShown);
	element.hxModalDotnetObjectReference = null;

	const modal = bootstrap.Modal.getInstance(element);
	if (modal) {
		modal.dispose();
	}
}