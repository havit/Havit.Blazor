export function create(element, hxDropdownDotnetObjectReference, options, subscribeToHideEvent) {
	if (!element) {
		return;
	}
	element.hxDropdownDotnetObjectReference = hxDropdownDotnetObjectReference;
	if (subscribeToHideEvent) {
		element.addEventListener('hide.bs.dropdown', handleDropdownHide);
	}
	element.addEventListener('shown.bs.dropdown', handleDropdownShown);
	element.addEventListener('hidden.bs.dropdown', handleDropdownHidden);

	if (options) {
		prepareOptions(options);
		const d = new bootstrap.Dropdown(element, options);
	}
	else {
		const d = new bootstrap.Dropdown(element);
	}
}

function prepareOptions(options) {
	if (options.reference !== undefined) {
		if (options.reference === null) {
			options.reference = "toggle"; // default Bootstrap value
		}
		else if (options.reference.startsWith("#")) {
			options.reference = document.querySelector(options.reference);
		}
	}
}

export function update(element, options) {
	if (!element) {
		return;
	}

	const dOld = bootstrap.Dropdown.getInstance(element);
	if (dOld) {
		dOld.dispose();
	}

	if (options) {
		prepareOptions(options);
		const d = new bootstrap.Dropdown(element, options);
	}
	else {
		const d = new bootstrap.Dropdown(element);
	}
}

export function show(element) {
	const d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.show();
	}
}

export function hide(element) {
	const d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.hide();
	}
}

function handleDropdownShown(event) {
	event.target.hxDropdownDotnetObjectReference.invokeMethodAsync('HxDropdown_HandleJsShown');
};

async function handleDropdownHide(event) {
	const d = bootstrap.Dropdown.getInstance(event.target);

	if (d.hidePreventionDisabled) {
		d.hidePreventionDisabled = false;
		return;
	}

	event.preventDefault();

	const cancel = await event.target.hxDropdownDotnetObjectReference.invokeMethodAsync('HxDropdown_HandleJsHide');
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
	const d = bootstrap.Dropdown.getInstance(element);
	if (d) {
		d.dispose();
	}
}
