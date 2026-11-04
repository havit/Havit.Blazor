// has to be aligned with HxPopover.js!
export function initialize(element, hxDotnetObjectReference, options) {
	if (!element) {
		return;
	}
	element.hxDotnetObjectReference = hxDotnetObjectReference;
	element.addEventListener('shown.bs.tooltip', handleShown);
	element.addEventListener('hidden.bs.tooltip', handleHidden);
	if (element.getAttribute('data-bs-placement') === 'auto') {
		options = { ...options, placement: getAutoPlacement };
	}
	new bootstrap.Tooltip(element, options);
}

// Bootstrap 6 (Floating UI) has no "auto" placement (it falls back to "top" + flip).
// Emulate it with a placement function choosing the side with the most available space.
function getAutoPlacement(tip, trigger) {
	const rect = trigger.getBoundingClientRect();
	const space = {
		top: rect.top,
		bottom: window.innerHeight - rect.bottom,
		left: rect.left,
		right: window.innerWidth - rect.right
	};
	const side = Object.keys(space).reduce((best, current) => (space[current] > space[best]) ? current : best);
	if (document.documentElement.dir === 'rtl') {
		// Bootstrap maps left/right to start/end
		return { left: 'right', right: 'left' }[side] ?? side;
	}
	return side;
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
		// #1541 Bootstrap's setContent() does not update the title config which drives _isWithContent(),
		// a tooltip initialized with an empty title would never show (and vice versa).
		i._config.title = newContent['.tooltip-inner'] ?? '';
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