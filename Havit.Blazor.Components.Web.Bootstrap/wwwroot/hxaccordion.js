export function create(element, hxAccordionItemDotnetObjectReference) {
	element.hxAccordionItemDotnetObjectReference = hxAccordionItemDotnetObjectReference;
	element.addEventListener('shown.bs.collapse', handleAccordionItemShown);
	element.addEventListener('hidden.bs.collapse', handleAccordionItemHidden);
	console.log("AccordionItem_create[" + element.id + "]");
}

function handleAccordionItemShown(event) {
	event.target.hxAccordionItemDotnetObjectReference.invokeMethodAsync('HxAccordionItem_HandleJsShown');
	console.log("AccordionItem_shown[" + event.target.id + "]");
};

function handleAccordionItemHidden(event) {
	event.target.hxAccordionItemDotnetObjectReference.invokeMethodAsync('HxAccordionItem_HandleJsHidden');
	console.log("AccordionItem_hidden[" + event.target.id + "]");
};

export function dispose(element) {
	element.removeEventListener('shown.bs.collapse', handleAccordionItemShown);
	element.removeEventListener('hidden.bs.collapse', handleAccordionItemHidden);
	element.hxAccordionItemDotnetObjectReference = null;
	console.log("AccordionItem_dispose[" + element.id + "]");
}
