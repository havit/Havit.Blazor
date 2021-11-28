export function create(element, hxAccordionItemDotnetObjectReference, parentSelector) {
	element.hxAccordionItemDotnetObjectReference = hxAccordionItemDotnetObjectReference;
	element.addEventListener('shown.bs.collapse', handleAccordionItemShown);
	element.addEventListener('hidden.bs.collapse', handleAccordionItemHidden);
	var c = new bootstrap.Collapse(element, {
		toggle: false,
	});
	console.debug("AccordionItem_create[" + element.id + "]");
}

function handleAccordionItemShown(event) {
	event.target.hxAccordionItemDotnetObjectReference.invokeMethodAsync('HxAccordionItem_HandleJsShown');
	console.debug("AccordionItem_shown[" + event.target.id + "]");
};

export function show(element) {
	console.debug("AccordionItem_show[" + element.id + "]");
	var c = bootstrap.Collapse.getInstance(element);
	c.show();
};

export function hide(element) {
	console.debug("AccordionItem_hide[" + element.id + "]");
	var c = bootstrap.Collapse.getInstance(element);
	c.hide();
};

function handleAccordionItemHidden(event) {
	event.target.hxAccordionItemDotnetObjectReference.invokeMethodAsync('HxAccordionItem_HandleJsHidden');
	console.debug("AccordionItem_hidden[" + event.target.id + "]");
};

export function dispose(element) {
	element.removeEventListener('shown.bs.collapse', handleAccordionItemShown);
	element.removeEventListener('hidden.bs.collapse', handleAccordionItemHidden);
	element.hxAccordionItemDotnetObjectReference = null;
	console.debug("AccordionItem_dispose[" + element.id + "]");
}
