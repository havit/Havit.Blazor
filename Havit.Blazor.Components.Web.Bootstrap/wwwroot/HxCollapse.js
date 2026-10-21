export function initialize(element, hxCollapseDotnetObjectReference, shouldShow) {
	if (!element) {
		return;
	}

	element.hxCollapseDotnetObjectReference = hxCollapseDotnetObjectReference;
	element.addEventListener('show.bs.collapse', handleCollapseShow);
	element.addEventListener('shown.bs.collapse', handleCollapseShown);
	element.addEventListener('hide.bs.collapse', handleCollapseHide);
	element.addEventListener('hidden.bs.collapse', handleCollapseHidden);

	const c = new bootstrap.Collapse(element);

	// Bootstrap 6 removed the "toggle" option (which showed the collapse on initialization), so show it explicitly.
	if (shouldShow) {
		c.show();
	}
}

export function show(element) {
	const c = bootstrap.Collapse.getInstance(element);
	if (c) {
		c.show();
	}
};

export function hide(element) {
	const c = bootstrap.Collapse.getInstance(element);
	if (c) {
		c.hide();
	}
};

function handleCollapseShow(event) {
	event.target.hxCollapseDotnetObjectReference.invokeMethodAsync('HxCollapse_HandleJsShow');
};
function handleCollapseShown(event) {
	event.target.hxCollapseDotnetObjectReference.invokeMethodAsync('HxCollapse_HandleJsShown');
};
function handleCollapseHide(event) {
	event.target.hxCollapseDotnetObjectReference.invokeMethodAsync('HxCollapse_HandleJsHide');
};
function handleCollapseHidden(event) {
	event.target.hxCollapseDotnetObjectReference.invokeMethodAsync('HxCollapse_HandleJsHidden');
};

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('show.bs.collapse', handleCollapseShow);
	element.removeEventListener('shown.bs.collapse', handleCollapseShown);
	element.removeEventListener('hide.bs.collapse', handleCollapseHide);
	element.removeEventListener('hidden.bs.collapse', handleCollapseHidden);
	element.hxCollapseDotnetObjectReference = null;

	const c = bootstrap.Collapse.getInstance(element);
	if (c) {
		c.dispose();
	}
}
