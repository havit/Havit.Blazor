export function create(element, hxAccordionItemDotnetObjectReference, parentSelector) {
	if (!element) {
		return;
	}

	element.hxAccordionItemDotnetObjectReference = hxAccordionItemDotnetObjectReference;
	element.addEventListener('shown.bs.collapse', handleAccordionItemShown);
	element.addEventListener('hidden.bs.collapse', handleAccordionItemHidden);

	var c = new bootstrap.Collapse(element, {
		toggle: false,
	});
}

function handleAccordionItemShown(event) {
	event.target.hxAccordionItemDotnetObjectReference.invokeMethodAsync('HxAccordionItem_HandleJsShown');
};

export function show(element) {
	var c = bootstrap.Collapse.getInstance(element);
	if (c) {
		c.show();
	}
};

export function hide(element) {
	var c = bootstrap.Collapse.getInstance(element);
	if (c) {
		c.hide();
	}
};

function handleAccordionItemHidden(event) {
	event.target.hxAccordionItemDotnetObjectReference.invokeMethodAsync('HxAccordionItem_HandleJsHidden');
};

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('shown.bs.collapse', handleAccordionItemShown);
	element.removeEventListener('hidden.bs.collapse', handleAccordionItemHidden);
	element.hxAccordionItemDotnetObjectReference = null;

	var c = bootstrap.Collapse.getInstance(element);
	if (c) {
		c.dispose();
	}
}
