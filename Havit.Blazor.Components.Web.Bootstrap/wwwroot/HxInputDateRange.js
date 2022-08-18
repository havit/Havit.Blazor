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

export function open(triggerElement) {
	if (!triggerElement) {
		return;
	}

	new bootstrap.Dropdown(triggerElement).show();
}

function handleIconClick(event) {
	var triggerElement = event.currentTarget.triggerElement;
	triggerElement.click();
	event.stopPropagation();
}

export function destroy(triggerElement) {
	if (!triggerElement) {
		return;
	}

	var dropdown = bootstrap.Dropdown.getInstance(triggerElement);
	if (dropdown) {
		dropdown.hide();
		dropdown.dispose();
	}
}

export function setInputValid(inputElement) {
	if (!inputElement) {
		return;
	}
	inputElement.classList.remove("is-invalid");
}

export function setInputInvalid(inputElement) {
	if (!inputElement) {
		return;
	}
	inputElement.classList.add("is-invalid");
}
