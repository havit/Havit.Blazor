export function initialize(element, hxMultiSelectDotnetObjectReference) {
	if (!element) {
		return;
	}

	element.hxMultiSelectDotnetObjectReference = hxMultiSelectDotnetObjectReference;
	element.addEventListener('shown.bs.menu', handleMenuShown);
	element.addEventListener('hidden.bs.menu', handleMenuHidden);

	const d = new bootstrap.Menu(element);
}

export function show(element) {
	const d = bootstrap.Menu.getInstance(element);
	if (d) {
		d.show();
	}
};

export function hide(element) {
	const d = bootstrap.Menu.getInstance(element);
	if (d) {
		d.hide();
	}
};

function handleMenuShown(event) {
	event.currentTarget.hxMultiSelectDotnetObjectReference.invokeMethodAsync('HxMultiSelect_HandleJsShown');
};

function handleMenuHidden(event) {
	event.currentTarget.hxMultiSelectDotnetObjectReference.invokeMethodAsync('HxMultiSelect_HandleJsHidden');
};

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('shown.bs.menu', handleMenuShown);
	element.removeEventListener('hidden.bs.menu', handleMenuHidden);
	element.hxMultiSelectDotnetObjectReference = null;

	const d = bootstrap.Menu.getInstance(element);
	if (d) {
		d.dispose();
	}
}
