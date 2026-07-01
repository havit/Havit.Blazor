export function initialize(element, hxMultiSelectDotnetObjectReference) {
	if (!element) {
		return;
	}

	element.hxMultiSelectDotnetObjectReference = hxMultiSelectDotnetObjectReference;
	element.addEventListener('shown.bs.dropdown', handleDropdownShown);
	element.addEventListener('hidden.bs.dropdown', handleDropdownHidden);

	const d = new bootstrap.Dropdown(element);
}

export function show(element) {
	const d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.show();
	}
};

export function hide(element) {
	const d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.hide();
	}
};

function handleDropdownShown(event) {
	event.currentTarget.hxMultiSelectDotnetObjectReference.invokeMethodAsync('HxMultiSelect_HandleJsShown');
};

function handleDropdownHidden(event) {
	event.currentTarget.hxMultiSelectDotnetObjectReference.invokeMethodAsync('HxMultiSelect_HandleJsHidden');
};

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('shown.bs.dropdown', handleDropdownShown);
	element.removeEventListener('hidden.bs.dropdown', handleDropdownHidden);
	element.hxMultiSelectDotnetObjectReference = null;

	const d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.dispose();
	}
}
