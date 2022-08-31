export function addOpenAndCloseEventListeners(triggerElement, iconWrapperElement) {
	if (!triggerElement) {
		return;
	}

	if (!iconWrapperElement) {
		return;
	}

	iconWrapperElement.addEventListener('click', handleIconClick);
	iconWrapperElement.triggerElement = triggerElement;
}

function handleIconClick(event) {
	var triggerElement = event.currentTarget.triggerElement;
	triggerElement.click();
	event.stopPropagation();
}
