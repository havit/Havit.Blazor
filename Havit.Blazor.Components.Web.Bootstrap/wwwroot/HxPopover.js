// has to be aligned with HxTooltip.js!
export function initialize(element, hxDotnetObjectReference, options) {
	if (!bootstrap || !element) {
		return;
	}
	element.hxDotnetObjectReference = hxDotnetObjectReference;
	element.addEventListener('shown.bs.popover', handleShown);
	element.addEventListener('hidden.bs.popover', handleHidden);
	new bootstrap.Popover(element, options);
}

export function show(element) {
	if (!bootstrap) {
		return;
	}
	var i = bootstrap.Popover.getInstance(element);
	if (i) {
		i.show();
	}
}

export function hide(element) {
	if (!bootstrap) {
		return;
	}
	var i = bootstrap.Popover.getInstance(element);
	if (i) {
		i.hide();
	}
}

export function enable(element) {
	if (!bootstrap) {
		return;
	}
	var i = bootstrap.Popover.getInstance(element);
	if (i) {
		i.enable();
		console.warn("enabled");
	}
}

export function disable(element) {
	if (!bootstrap) {
		return;
	}
	var i = bootstrap.Popover.getInstance(element);
	if (i) {
		i.disable();
		console.warn("disabled");
	}
}

export function setContent(element, newContent) {
	if (!bootstrap) {
		return;
	}
	var i = bootstrap.Popover.getInstance(element);
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
	if (!bootstrap || !element) {
		return;
	}
	element.removeEventListener('shown.bs.popover', handleShown);
	element.removeEventListener('hidden.bs.popover', handleHidden);
	element.hxDotnetObjectReference = null;
	var popover = bootstrap.Popover.getInstance(element);
	if (popover) {
		popover.dispose();
	}
}