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

	// Bootstrap Modal.show() refuses to proceed when _isTransitioning is true.
	// If a hide transition is in progress, defer the show to after the transition completes
	// (handleModalHidden will pick up the deferred show).
	const existingModal = bootstrap.Modal.getInstance(element);
	if (existingModal && existingModal._isTransitioning && !existingModal._isShown) {
		element.hxDeferredShow = { hxModalDotnetObjectReference, closeOnEscape, subscribeToHideEvent };
		return;
	}

	// Remove old listeners to prevent duplicates when show() is called multiple times
	element.removeEventListener('hide.bs.modal', handleModalHide);
	element.removeEventListener('hidden.bs.modal', handleModalHidden);
	element.removeEventListener('shown.bs.modal', handleModalShown);

	element.hxModalDotnetObjectReference = hxModalDotnetObjectReference;
	if (subscribeToHideEvent) {
		element.addEventListener('hide.bs.modal', handleModalHide);
	}
	element.addEventListener('hidden.bs.modal', handleModalHidden);
	element.addEventListener('shown.bs.modal', handleModalShown);
	window.modalElement = element;

	let modal = bootstrap.Modal.getInstance(element);
	if (!modal) {
		modal = new bootstrap.Modal(element, {
			keyboard: closeOnEscape
		});
	}
	modal.show();
}

export function hide(element) {
	if (!element) {
		return;
	}
	element.hxDeferredShow = null; // Cancel any pending deferred show
	element.hxModalHiding = true;
	const modal = bootstrap.Modal.getInstance(element);
	if (modal) {
		// Bootstrap Modal.hide() refuses to run while _isTransitioning is true.
		// Clear it to allow hide during a show transition (rapid show→hide).
		if (modal._isTransitioning) {
			modal._isTransitioning = false;
		}
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

	if (event.target === window.modalElement) {
		window.modalElement = null;
	}

	// If show() was called during a hide transition, execute the deferred show now
	if (event.target.hxDeferredShow) {
		const { hxModalDotnetObjectReference, closeOnEscape, subscribeToHideEvent } = event.target.hxDeferredShow;
		event.target.hxDeferredShow = null;
		show(event.target, hxModalDotnetObjectReference, closeOnEscape, subscribeToHideEvent);
		return;
	}

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

	element.hxDeferredShow = null;
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

	if (element === window.modalElement) { // another modal might be already open
		window.modalElement = null;
	}
}
