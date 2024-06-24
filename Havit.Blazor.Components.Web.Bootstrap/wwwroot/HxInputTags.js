export function initialize(inputId, hxInputTagsDotnetObjectReference, keysToPrevendDefault) {
	let inputElement = document.getElementById(inputId);
	if (!inputElement) {
		return;
	}

	inputElement.hxInputTagsDotnetObjectReference = hxInputTagsDotnetObjectReference;
	inputElement.hxInputTagsKeysToPreventDefault = keysToPrevendDefault;

	inputElement.addEventListener('keydown', handleKeyDown);
	inputElement.addEventListener('blur', handleInputBlur);
}

function handleKeyDown(event) {
	let key = event.key;

	event.target.hxInputTagsDotnetObjectReference.invokeMethodAsync("HxInputTagsInternal_HandleInputKeyDown", key);

	if (event.target.hxInputTagsKeysToPreventDefault.includes(key)) {
		event.preventDefault();
	}
}

function handleInputBlur(event) {
	// We need the blur event to confirm custom tag creation.
	// When the dropdown is open, the blur event is fired before the click event on the dropdown item.
	// We need to recognize, whether the blur event is fired because of the dropdown item click or because of the user clicked somewhere else.
	// We will use relatedTarget property of the event to recognize the click on the dropdown item.
	// If relatedTarget is within the dropdown, we will ignore the blur event.
	var isWithinDropdown = false;
	if (event.relatedTarget) {
		isWithinDropdown = event.target.parentElement.contains(event.relatedTarget);
	}

	event.target.hxInputTagsDotnetObjectReference.invokeMethodAsync("HxInputTagsInternal_HandleInputBlur", isWithinDropdown);
}

export function open(inputElement, hxInputTagsDotnetObjectReference, delayShow) {
	if (!inputElement) {
		return;
	}

	inputElement.setAttribute("data-bs-toggle", "dropdown");
	inputElement.hxInputTagsDotnetObjectReference = hxInputTagsDotnetObjectReference;
	inputElement.addEventListener('hidden.bs.dropdown', handleDropdownHidden)

	var dd = new bootstrap.Dropdown(inputElement);
	if (!dd) {
		return;
	}
	if (!delayShow) {
		dd.show();
	}
	else {
		// Bootstrap dropdown registers onClick event handler which toggles the dropdown
		// For focus-triggered dropdowns we need to delay the show() as the upcomming click event will toggle (= hide) the dropdown
		window.setTimeout(function (dropdown) {
			dropdown.show();
		}, 200, dd);
	}
}

export function tryFocus(inputElement) {
	if (!inputElement) {
		return false;
	}

	inputElement.focus({ preventScroll: true });
	return true;
}

export function destroy(inputElement) {
	if (!inputElement) {
		return;
	}

	inputElement.removeAttribute("data-bs-toggle", "dropdown");

	var dropdown = bootstrap.Dropdown.getInstance(inputElement);
	if (dropdown) {
		dropdown.hide();

		inputElement.addEventListener('hidden.bs.dropdown', event => {
			dropdown.dispose()
		})
	}
}

function handleDropdownHidden(event) {
	event.target.removeEventListener('hidden.bs.dropdown', handleDropdownHidden);

	destroy(event.target);

	// In Blazor, jsinterop is "faster" then events.
	// As a result, this method (handleDropdownHidden) is first, dropdown item click event (Blazor OnClick Event) second.
	// But we need the item click event to fire first.
	// Therefore we delay jsinterop for a while.
	window.setTimeout(function (element) {
		element.hxInputTagsDotnetObjectReference.invokeMethodAsync('HxInputTagsInternal_HandleDropdownHidden');
	}, 1, event.target);
}

export function dispose(inputId) {
	let inputElement = document.getElementById(inputId);

	inputElement.removeEventListener('keydown', handleKeyDown);
	inputElement.removeEventListener('blur', handleInputBlur);
	inputElement.hxInputTagsDotnetObjectReference = null;
	inputElement.hxInputTagsKeysToPreventDefault = null;
}