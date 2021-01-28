export function create(element, hxAccordionItemDotnetObjectReference) {
	element.hxAccordionItemDotnetObjectReference = hxAccordionItemDotnetObjectReference;
	element.addEventListener('shown.bs.collapse', handleAccordionItemShown);
	element.addEventListener('hidden.bs.collapse', handleAccordionItemHidden);
	console.log("accordition_create");
}

function show(element) {
	var ai = bootstrap.Collapse.getInstance();
	ai.show();
	console.log("accordition_show");
};

function handleAccordionItemShown(event) {
	event.target.hxAccordionItemDotnetObjectReference.invokeMethodAsync('HxAccordionItem_HandleJsShown');
	console.log("accordition_shown");
};

function handleAccordionItemHidden(event) {
	event.target.hxAccordionItemDotnetObjectReference.invokeMethodAsync('HxAccordionItem_HandleJsHidden');
	console.log("accordition_hidden");
};

export function dispose(element) {
	element.removeEventListener('shown.bs.collapse', handleAccordionItemShown);
	element.removeEventListener('hidden.bs.collapse', handleAccordionItemHidden);
	element.hxAccordionItemDotnetObjectReference = null;
	console.log("accordition_dispose");
}
