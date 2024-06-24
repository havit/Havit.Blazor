export function create(element, hxDropdownDotnetObjectReference, reference, subscribeToHideEvent) {
	if (!element) {
		return;
	}
	element.hxDropdownDotnetObjectReference = hxDropdownDotnetObjectReference;
	if (subscribeToHideEvent) {
		element.addEventListener('hide.bs.dropdown', handleDropdownHide);
	}
	element.addEventListener('shown.bs.dropdown', handleDropdownShown);
	element.addEventListener('hidden.bs.dropdown', handleDropdownHidden);

	if (reference) {
		var referenceOption = document.querySelector(reference);
		var d = new bootstrap.Dropdown(element, {
			reference: referenceOption
		});
	}
	else {
		var d = new bootstrap.Dropdown(element);
	}
}

export function update(element, newReference) {
	if (!element) {
		return;
	}

	var d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.dispose();
	}

	if (newReference) {
		var referenceOption = document.querySelector(newReference);
		var d = new bootstrap.Dropdown(element, {
			reference: referenceOption
		});
	}
	else {
		var d = new bootstrap.Dropdown(element);
	}
}

export function show(element) {
	var d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.show();
	}
}

export function hide(element) {
	var d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.hide();
	}
}

function handleDropdownShown(event) {
	event.target.hxDropdownDotnetObjectReference.invokeMethodAsync('HxDropdown_HandleJsShown');
};

async function handleDropdownHide(event) {
	let d = bootstrap.Dropdown.getInstance(event.target);

	if (d.hidePreventionDisabled) {
		d.hidePreventionDisabled = false;
		return;
	}

	event.preventDefault();

	let cancel = await event.target.hxDropdownDotnetObjectReference.invokeMethodAsync('HxDropdown_HandleJsHide');
	if (!cancel) {
		d.hidePreventionDisabled = true;
		d.hide();
	}
};

function handleDropdownHidden(event) {
	event.target.hxDropdownDotnetObjectReference.invokeMethodAsync('HxDropdown_HandleJsHidden');
};

export function dispose(element) {
	if (!element) {
		return;
	}
	element.removeEventListener('shown.bs.dropdown', handleDropdownShown);
	element.removeEventListener('hidden.bs.dropdown', handleDropdownHidden);
	element.hxDropdownDotnetObjectReference = null;
	var d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.dispose();
	}
}
