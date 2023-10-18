export function initialize(element, hxMultiSelectDotnetObjectReference) {
	if (!element) {
		return;
	}

	element.hxMultiSelectDotnetObjectReference = hxMultiSelectDotnetObjectReference;
	element.addEventListener('shown.bs.dropdown', handleDropdownShown);
	element.addEventListener('hidden.bs.dropdown', handleDropdownHidden);

	var d = new bootstrap.Dropdown(element);
}

export function show(element) {
	var d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.show();
	}
};

export function hide(element) {
	var d = bootstrap.Dropdown.getInstance(element);
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

	var d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.dispose();
	}
}
