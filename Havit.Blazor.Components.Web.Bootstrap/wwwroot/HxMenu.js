export function create(element, hxMenuDotnetObjectReference, options, subscribeToHideEvent) {
	if (!element) {
		return;
	}
	element.hxMenuDotnetObjectReference = hxMenuDotnetObjectReference;
	if (subscribeToHideEvent) {
		element.addEventListener('hide.bs.menu', handleMenuHide);
	}
	element.addEventListener('shown.bs.menu', handleMenuShown);
	element.addEventListener('hidden.bs.menu', handleMenuHidden);

	if (options) {
		prepareOptions(options);
		const d = new bootstrap.Menu(element, options);
	}
	else {
		const d = new bootstrap.Menu(element);
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

	const dOld = bootstrap.Menu.getInstance(element);
	if (dOld) {
		dOld.dispose();
	}

	if (options) {
		prepareOptions(options);
		const d = new bootstrap.Menu(element, options);
	}
	else {
		const d = new bootstrap.Menu(element);
	}
}

export function show(element) {
	const d = bootstrap.Menu.getInstance(element);
	if (d) {
		d.show();
	}
}

export function hide(element) {
	const d = bootstrap.Menu.getInstance(element);
	if (d) {
		d.hide();
	}
}

function handleMenuShown(event) {
	event.target.hxMenuDotnetObjectReference.invokeMethodAsync('HxMenu_HandleJsShown');
};

async function handleMenuHide(event) {
	const d = bootstrap.Menu.getInstance(event.target);

	if (d.hidePreventionDisabled) {
		d.hidePreventionDisabled = false;
		return;
	}

	event.preventDefault();

	const cancel = await event.target.hxMenuDotnetObjectReference.invokeMethodAsync('HxMenu_HandleJsHide');
	if (!cancel) {
		d.hidePreventionDisabled = true;
		d.hide();
	}
};

function handleMenuHidden(event) {
	event.target.hxMenuDotnetObjectReference.invokeMethodAsync('HxMenu_HandleJsHidden');
};

export function dispose(element) {
	if (!element) {
		return;
	}
	element.removeEventListener('shown.bs.menu', handleMenuShown);
	element.removeEventListener('hidden.bs.menu', handleMenuHidden);
	element.hxMenuDotnetObjectReference = null;
	const d = bootstrap.Menu.getInstance(element);
	if (d) {
		d.dispose();
	}
}
