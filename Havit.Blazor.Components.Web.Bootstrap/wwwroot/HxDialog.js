export function show(element, hxDialogDotnetObjectReference, closeOnEscape, subscribeToHideEvent, subscribeToHidePreventedEvent) {
	if (!element) {
		return;
	}

	// Remove old listeners to prevent duplicates when show() is called multiple times
	element.removeEventListener('hide.bs.dialog', handleDialogHide);
	element.removeEventListener('hidePrevented.bs.dialog', handleDialogHidePrevented);
	element.removeEventListener('hidden.bs.dialog', handleDialogHidden);
	element.removeEventListener('shown.bs.dialog', handleDialogShown);

	element.hxDialogDotnetObjectReference = hxDialogDotnetObjectReference;
	if (subscribeToHideEvent) {
		element.addEventListener('hide.bs.dialog', handleDialogHide);
	}
	if (subscribeToHidePreventedEvent) {
		element.addEventListener('hidePrevented.bs.dialog', handleDialogHidePrevented);
	}
	element.addEventListener('hidden.bs.dialog', handleDialogHidden);
	element.addEventListener('shown.bs.dialog', handleDialogShown);

	// Bootstrap reads the configuration (incl. data-bs-backdrop and data-bs-modal) only in the constructor.
	// The dialog is closed here, so we always create a fresh instance to apply the current settings.
	bootstrap.Dialog.getInstance(element)?.dispose();
	const dialog = new bootstrap.Dialog(element, {
		keyboard: closeOnEscape
	});

	// Dialogs stack (native top layer). When the current dialog is just hiding (HideAsync() followed by ShowAsync() of another dialog),
	// swap them the Bootstrap way: the new dialog's backdrop starts fully visible and the hiding dialog drops its own backdrop,
	// so the backdrop does not flicker.
	const hidingDialog = document.querySelector('dialog.hx-dialog.hiding:modal');
	if (hidingDialog && (element.dataset.bsModal !== 'false')) {
		hidingDialog.classList.add('hx-dialog-swap-out');
		element.classList.add('dialog-swap-in');
	}

	dialog.show();

	observeOverflow(element);
}

export function hide(element) {
	if (!element) {
		return;
	}
	element.hxDialogHiding = true;
	const dialog = bootstrap.Dialog.getInstance(element);
	if (dialog) {
		dialog.hide();
	}
}

// Bootstrap 6 dialogs are overflow: visible (so dropdowns, tooltips etc. are not clipped) and the page cannot scroll behind a modal dialog.
// For non-scrollable dialogs whose content does not fit the viewport, we let the whole dialog scroll (.hx-dialog-overflow).
function observeOverflow(element) {
	element.hxDialogResizeObserver?.disconnect();
	element.hxDialogResizeObserver = null;

	if (element.classList.contains('dialog-scrollable') || (typeof ResizeObserver === 'undefined')) {
		element.classList.remove('hx-dialog-overflow');
		return;
	}

	const update = () => {
		// Sum of the sections (header, body, footer) - unlike scrollHeight, it is not affected by absolutely positioned content (dropdowns).
		let contentHeight = 0;
		for (const child of element.children) {
			contentHeight += child.offsetHeight;
		}
		element.classList.toggle('hx-dialog-overflow', contentHeight > element.clientHeight + 1);
	};

	const resizeObserver = new ResizeObserver(update);
	resizeObserver.observe(element);
	for (const child of element.children) {
		resizeObserver.observe(child);
	}
	element.hxDialogResizeObserver = resizeObserver;
}

function handleDialogShown(event) {
	if (event.target !== event.currentTarget) {
		return; // bubbled from a nested dialog (rendered in the content of this one)
	}

	event.target.classList.remove('dialog-swap-in');
	event.target.hxDialogDotnetObjectReference.invokeMethodAsync('HxDialog_HandleDialogShown');
}

async function handleDialogHide(event) {
	if (event.target !== event.currentTarget) {
		return; // bubbled from a nested dialog (rendered in the content of this one)
	}

	const dialogInstance = bootstrap.Dialog.getInstance(event.target);

	if (dialogInstance.hidePreventionDisabled || event.target.hxDialogDisposing) {
		dialogInstance.hidePreventionDisabled = false;
		return;
	}

	event.preventDefault();

	const cancel = await event.target.hxDialogDotnetObjectReference.invokeMethodAsync('HxDialog_HandleDialogHide');
	if (!cancel) {
		dialogInstance.hidePreventionDisabled = true;
		event.target.hxDialogHiding = true;
		dialogInstance.hide();
	}
}

function handleDialogHidePrevented(event) {
	if (event.target !== event.currentTarget) {
		return; // bubbled from a nested dialog (rendered in the content of this one)
	}

	event.target.hxDialogDotnetObjectReference.invokeMethodAsync('HxDialog_HandleDialogHidePrevented');
}

function handleDialogHidden(event) {
	if (event.target !== event.currentTarget) {
		return; // bubbled from a nested dialog (rendered in the content of this one)
	}

	event.target.hxDialogHiding = false;
	event.target.classList.remove('hx-dialog-swap-out');
	event.target.hxDialogResizeObserver?.disconnect();
	event.target.hxDialogResizeObserver = null;

	if (event.target.hxDialogDisposing) {
		// fix for #110 where the dispose() gets called while the dialog is still in hiding-transition
		dispose(event.target, false);
		return;
	}

	event.target.hxDialogDotnetObjectReference.invokeMethodAsync('HxDialog_HandleDialogHidden');
}

export function dispose(element, opened) {
	if (!element) {
		return;
	}

	element.hxDialogDisposing = true;

	if (element.hxDialogHiding) {
		// fix for #110 where the dispose() gets called while the dialog is still in hiding-transition
		return;
	}

	if (opened) {
		// #110 Scrolling not working when dialog is removed (even if disposed is called)
		// Compensates https://github.com/twbs/bootstrap/issues/36397,
		// where the o.dispose() does not reset the ScrollBarHelper() and the scrolling remains deactivated.
		// The dispose() is re-called from hidden.bs.dialog event handler.
		// Remove when the issue is fixed.
		hide(element);
		return;
	}

	element.removeEventListener('hide.bs.dialog', handleDialogHide);
	element.removeEventListener('hidePrevented.bs.dialog', handleDialogHidePrevented);
	element.removeEventListener('hidden.bs.dialog', handleDialogHidden);
	element.removeEventListener('shown.bs.dialog', handleDialogShown);
	element.hxDialogDotnetObjectReference = null;
	element.hxDialogResizeObserver?.disconnect();
	element.hxDialogResizeObserver = null;

	const dialog = bootstrap.Dialog.getInstance(element);
	if (dialog) {
		dialog.dispose();
	}
}
