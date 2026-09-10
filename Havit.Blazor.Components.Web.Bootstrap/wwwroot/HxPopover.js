// has to be aligned with HxTooltip.js!
export function initialize(element, hxDotnetObjectReference, options) {
	if (!element) {
		return;
	}
	element.hxDotnetObjectReference = hxDotnetObjectReference;
	element.addEventListener('shown.bs.popover', handleShown);
	element.addEventListener('hidden.bs.popover', handleHidden);
	new bootstrap.Popover(element, options);
}

export function show(element) {
	const i = bootstrap.Popover.getInstance(element);
	if (i) {
		i.show();
	}
}

export function hide(element) {
	const i = bootstrap.Popover.getInstance(element);
	if (i) {
		i.hide();
	}
}

export function enable(element) {
	const i = bootstrap.Popover.getInstance(element);
	if (i) {
		i.enable();
	}
}

export function disable(element) {
	const i = bootstrap.Popover.getInstance(element);
	if (i) {
		i.disable();
	}
}

export function setContent(element, newContent) {
	const i = bootstrap.Popover.getInstance(element);
	if (i) {
		// #1541 Bootstrap's setContent() does not update the title/content config which drives _isWithContent(),
		// a popover initialized with an empty title+content would never show (and vice versa).
		i._config.title = newContent['.popover-header'] ?? '';
		i._config.content = newContent['.popover-body'] ?? '';
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
	element.removeEventListener('shown.bs.popover', handleShown);
	element.removeEventListener('hidden.bs.popover', handleHidden);
	element.hxDotnetObjectReference = null;
	const popover = bootstrap.Popover.getInstance(element);
	if (popover) {
		popover.dispose();
	}
}