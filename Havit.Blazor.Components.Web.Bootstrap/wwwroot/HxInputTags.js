export function open(inputElement, hxAutosuggestDotnetObjectReference, delayShow) {
	console.warn("open:", delayShow);
	inputElement.setAttribute("data-bs-toggle", "dropdown");
	inputElement.hxAutosuggestDotnetObjectReference = hxAutosuggestDotnetObjectReference;
	inputElement.addEventListener('hidden.bs.dropdown', handleDropdownHidden)

	var dd = new bootstrap.Dropdown(inputElement);
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

export function destroy(inputElement) {
	console.warn("destroy");
	inputElement.removeAttribute("data-bs-toggle", "dropdown");
	var dropdown = bootstrap.Dropdown.getInstance(inputElement);
	if (dropdown) {
		dropdown.hide();
		dropdown.dispose();
	}
}

function handleDropdownHidden(event) {
	console.warn("handleDropdownHidden");
	event.target.removeEventListener('hidden.bs.dropdown', handleDropdownHidden);

	destroy(event.target);

	// In Blazor, jsinterop is "faster" then events.
	// As a result, this method (handleDropdownHidden) is first, dropdown item click event (Blazor OnClick Event) second.
	// But we need the item click event to fire first.
	// Therefore we delay jsinterop for a while.
	window.setTimeout(function (element) {
		element.hxAutosuggestDotnetObjectReference.invokeMethodAsync('HxInputTagsInternal_HandleDropdownHidden');
		element.hxAutosuggestDotnetObjectReference = null;
	}, 1, event.target);

	//    var dropdown = bootstrap.Dropdown.getInstance(event.inputElement);
	//    dropdown.dispose();
};