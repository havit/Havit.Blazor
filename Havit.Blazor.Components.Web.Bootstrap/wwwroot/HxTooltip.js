// has to be aligned with HxPopover.js!
export function initialize(element, hxDotnetObjectReference, options) {
	if (!element) {
		return;
	}
	element.hxDotnetObjectReference = hxDotnetObjectReference;
	element.addEventListener('shown.bs.tooltip', handleShown);
	element.addEventListener('hidden.bs.tooltip', handleHidden);
	new bootstrap.Tooltip(element, options);
}

export function show(element) {
	const i = bootstrap.Tooltip.getInstance(element);
	if (i) {
		i.show();
	}
}

export function hide(element) {
	const i = bootstrap.Tooltip.getInstance(element);
	if (i) {
		i.hide();
	}
}

export function enable(element) {
	const i = bootstrap.Tooltip.getInstance(element);
	if (i) {
		i.enable();
	}
}

export function disable(element) {
	const i = bootstrap.Tooltip.getInstance(element);
	if (i) {
		i.disable();
	}
}

export function setContent(element, newContent) {
	const i = bootstrap.Tooltip.getInstance(element);
	if (i) {
		i.setContent(newContent);
	}
}

function handleShown(event) {
	event.target.hxDotnetObjectReference.invokeMethodAsync('HxHandleJsShown');
};

function handleHidden(event) {
	event.target.hxDotnetObjectReference.invokeMethodAsync('HxHandleJsHidden');
};

export function dispose(element) {
	if (!element) {
		return;
	}
	element.removeEventListener('shown.bs.tooltip', handleShown);
	element.removeEventListener('hidden.bs.tooltip', handleHidden);
	element.hxDotnetObjectReference = null;
	const tooltip = bootstrap.Tooltip.getInstance(element);
	if (tooltip) {
		tooltip.dispose();
	}
}