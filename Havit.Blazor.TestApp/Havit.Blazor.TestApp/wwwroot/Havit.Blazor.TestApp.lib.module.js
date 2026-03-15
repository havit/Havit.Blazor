function appendElement(elementId) {
	// create new div element to signal that Blazor is ready for tests
	const div = document.createElement('div');
	div.id = elementId;
	document.body.appendChild(div);
}

export function afterWebStarted(blazor) {
	appendElement('blazor-afterWebStarted');
	appendElement('blazor-ready-for-tests');
	blazor.addEventListener('enhancedload', () => appendElement('blazor-enhancedload'));
	blazor.addEventListener('enhancedload', () => appendElement('blazor-ready-for-tests'));
}