export function initialize(toggleElement, hxSelectDotnetObjectReference) {
	if (!toggleElement) {
		return;
	}

	toggleElement.hxSelectDotnetObjectReference = hxSelectDotnetObjectReference;
	toggleElement.addEventListener('shown.bs.menu', handleMenuShown);
	toggleElement.addEventListener('hidden.bs.menu', handleMenuHidden);

	new bootstrap.Menu(toggleElement);
}

function handleMenuShown(event) {
	event.currentTarget.hxSelectDotnetObjectReference.invokeMethodAsync('HxSelect_HandleJsShown');
};

function handleMenuHidden(event) {
	event.currentTarget.hxSelectDotnetObjectReference.invokeMethodAsync('HxSelect_HandleJsHidden');
};

export function dispose(toggleElement) {
	if (!toggleElement) {
		return;
	}

	toggleElement.removeEventListener('shown.bs.menu', handleMenuShown);
	toggleElement.removeEventListener('hidden.bs.menu', handleMenuHidden);
	toggleElement.hxSelectDotnetObjectReference = null;

	const menu = bootstrap.Menu.getInstance(toggleElement);
	if (menu) {
		menu.dispose();
	}
}
