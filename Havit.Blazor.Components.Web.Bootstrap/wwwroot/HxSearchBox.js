export function initialize(inputId, hxSearchBoxDotnetObjectReference, keysToPreventDefault) {
	let inputElement = document.getElementById(inputId);
	if (!inputElement) {
		return;
	}

	inputElement.hxSearchBoxDotnetObjectReference = hxSearchBoxDotnetObjectReference;
	inputElement.hxSearchBoxKeysToPreventDefault = keysToPreventDefault;

	inputElement.addEventListener('keydown', handleKeyDown);

	inputElement.addEventListener('mousedown', handleMouseDown);
	inputElement.addEventListener('mouseup', handleMouseUp);
	inputElement.addEventListener('mouseleave', handleMouseLeave);
}

function handleKeyDown(event) {
	let key = event.key;

	event.target.hxSearchBoxDotnetObjectReference.invokeMethodAsync("HxSearchBox_HandleInputKeyDown", key);

	if (event.target.hxSearchBoxKeysToPreventDefault.includes(key)) {
		event.preventDefault();
	}
}

function handleMouseDown(event) {
	event.target.hxSearchBoxDotnetObjectReference.invokeMethodAsync("HxSearchBox_HandleInputMouseDown");
	event.target.clickIsComing = true;
}
function handleMouseUp(event) {
	event.target.hxSearchBoxDotnetObjectReference.invokeMethodAsync("HxSearchBox_HandleInputMouseUp");
	event.target.clickIsComing = false;
}

function handleMouseLeave(event) {
	// fixes #702 where the dropdown is not shown after selecting input content with mouse
	// (the input does not receive mouseup when the mouse is released outside of the input)
	event.target.hxSearchBoxDotnetObjectReference.invokeMethodAsync("HxSearchBox_HandleInputMouseLeave");
	event.target.clickIsComing = false;
}

export function scrollToFocusedItem() {
	const focusedElements = document.getElementsByClassName("hx-dropdown-item-focused");
	if (focusedElements && focusedElements[0]) {
		focusedElements[0].scrollIntoView({ behavior: 'instant', block: 'nearest', inline: 'start' });
	}
}

export function dispose(inputId) {
	let inputElement = document.getElementById(inputId);
	if (!inputElement) {
		return;
	}

	inputElement.removeEventListener('keydown', handleKeyDown);
	inputElement.removeEventListener('mousedown', handleMouseDown);
	inputElement.removeEventListener('mouseup', handleMouseUp);
	inputElement.removeEventListener('mouseleave', handleMouseLeave);
	inputElement.hxSearchBoxDotnetObjectReference = null;
	inputElement.hxSearchBoxKeysToPreventDefault = null;
}
