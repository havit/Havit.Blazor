export function initialize(element, hxCollapseDotnetObjectReference) {
	element.hxCollapseDotnetObjectReference = hxCollapseDotnetObjectReference;
	element.addEventListener('shown.bs.collapse', handleCollapseShown);
	element.addEventListener('hidden.bs.collapse', handleCollapseHidden);
	var c = new bootstrap.Collapse(element, {
		toggle: false,
	});
}

function handleCollapseShown(event) {
	event.target.hxCollapseDotnetObjectReference.invokeMethodAsync('HxCollapse_HandleJsShown');
};

export function show(element) {
	var c = bootstrap.Collapse.getInstance(element);
	c.show();
};

export function hide(element) {
	var c = bootstrap.Collapse.getInstance(element);
	c.hide();
};

function handleCollapseHidden(event) {
	event.target.hxCollapseDotnetObjectReference.invokeMethodAsync('HxCollapse_HandleJsHidden');
};

export function dispose(element) {
	element.removeEventListener('shown.bs.collapse', handleCollapseShown);
	element.removeEventListener('hidden.bs.collapse', handleCollapseHidden);
	element.hxCollapseDotnetObjectReference = null;
	var c = bootstrap.Collapse.getInstance(element);
	c.dispose();
}
