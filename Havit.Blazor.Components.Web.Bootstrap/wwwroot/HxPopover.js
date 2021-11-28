// has to be aligned with HxTooltip.js!
export function createOrUpdate(element, hxDotnetObjectReference, options) {
	destroy(element);
	element.hxDotnetObjectReference = hxDotnetObjectReference;
	element.addEventListener('shown.bs.popover', handleShown);
	element.addEventListener('hidden.bs.popover', handleHidden);
	new bootstrap.Popover(element, options);
}

export function show(element) {
	var i = bootstrap.Popover.getInstance(element);
	i.show();
}

export function hide(element) {
	var i = bootstrap.Popover.getInstance(element);
	i.hide();
}

function handleShown(event) {
	event.target.hxDotnetObjectReference.invokeMethodAsync('HxHandleJsShown');
};

function handleHidden(event) {
	event.target.hxDotnetObjectReference.invokeMethodAsync('HxHandleJsHidden');
};

export function destroy(element) {
	element.removeEventListener('shown.bs.popover', handleShown);
	element.removeEventListener('hidden.bs.popover', handleHidden);
	element.hxDotnetObjectReference = null;
	var popover = bootstrap.Popover.getInstance(element);
	if (popover) {
		popover.dispose();
	}
}