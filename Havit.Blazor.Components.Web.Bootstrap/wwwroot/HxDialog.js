export function show(element, hxDialogDotnetObjectReference, closeOnEscape, subscribeToHideEvent) {
	if (window.dialogElement) {
		const previousDialog = bootstrap.Dialog.getInstance(window.dialogElement);
		if (previousDialog) {
			previousDialog.hide();
		}
	}

	if (!element) {
		return;
	}

	// Remove old listeners to prevent duplicates when show() is called multiple times
	element.removeEventListener('hide.bs.dialog', handleDialogHide);
	element.removeEventListener('hidden.bs.dialog', handleDialogHidden);
	element.removeEventListener('shown.bs.dialog', handleDialogShown);

	element.hxDialogDotnetObjectReference = hxDialogDotnetObjectReference;
	if (subscribeToHideEvent) {
		element.addEventListener('hide.bs.dialog', handleDialogHide);
	}
	element.addEventListener('hidden.bs.dialog', handleDialogHidden);
	element.addEventListener('shown.bs.dialog', handleDialogShown);
	window.dialogElement = element;

	let dialog = bootstrap.Dialog.getInstance(element);
	if (!dialog) {
		dialog = new bootstrap.Dialog(element, {
			keyboard: closeOnEscape
		});
	}
	dialog.show();
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

function handleDialogShown(event) {
	event.target.hxDialogDotnetObjectReference.invokeMethodAsync('HxDialog_HandleDialogShown');
}

async function handleDialogHide(event) {
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

function handleDialogHidden(event) {
	event.target.hxDialogHiding = false;

	if (event.target === window.dialogElement) {
		window.dialogElement = null;
	}

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
	element.removeEventListener('hidden.bs.dialog', handleDialogHidden);
	element.removeEventListener('shown.bs.dialog', handleDialogShown);
	element.hxDialogDotnetObjectReference = null;

	const dialog = bootstrap.Dialog.getInstance(element);
	if (dialog) {
		dialog.dispose();
	}

	if (element === window.dialogElement) { // another dialog might be already open
		window.dialogElement = null;
	}
}
