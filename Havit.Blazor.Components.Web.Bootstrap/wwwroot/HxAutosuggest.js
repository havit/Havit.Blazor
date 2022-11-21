export function initialize(inputId, hxAutosuggestDotnetObjectReference, keysToPreventDefault) {
	let inputElement = document.getElementById(inputId);
	if (!inputElement) {
		return;
	}

    inputElement.hxAutosuggestDotnetObjectReference = hxAutosuggestDotnetObjectReference;
	inputElement.hxAutosuggestKeysToPreventDefault = keysToPreventDefault;

    inputElement.addEventListener('keydown', handleKeyDown);
}

function handleKeyDown(event) {
    let key = event.key;

    event.target.hxAutosuggestDotnetObjectReference.invokeMethodAsync("HxAutosuggestInternal_HandleInputKeyDown", key);

	if (event.target.hxAutosuggestKeysToPreventDefault.includes(key)) {
        event.preventDefault();
    }
}

export function open(inputElement, hxAutosuggestDotnetObjectReference) {
	if (!inputElement) {
		return;
	}
    inputElement.setAttribute("data-bs-toggle", "dropdown");
    inputElement.hxAutosuggestDotnetObjectReference = hxAutosuggestDotnetObjectReference;
    inputElement.addEventListener('hidden.bs.dropdown', handleDropdownHidden);

	var d = new bootstrap.Dropdown(inputElement);
	if (d) {
		d.show();
	}
}

export function destroy(inputElement) {
	if (!inputElement) {
		return;
	}

	inputElement.removeAttribute("data-bs-toggle", "dropdown");

    var d = bootstrap.Dropdown.getInstance(inputElement);
    if (d) {
        d.hide();
        d.dispose();
    }
}

function handleDropdownHidden(event) {
    event.target.removeEventListener('hidden.bs.dropdown', handleDropdownHidden);

    // In Blazor, jsinterop is "faster" then events.
    // As a result, this method (handleDropdownHidden) is first, dropdown item click event (Blazor OnClick Event) second.
    // But we need the item click event to fire first.
    // Therefore we delay jsinterop for a while.
    window.setTimeout(function (element) {
        element.hxAutosuggestDotnetObjectReference.invokeMethodAsync('HxAutosuggestInternal_HandleDropdownHidden');       
    }, 1, event.target);

	var d = bootstrap.Dropdown.getInstance(event.target);
	if (d) {
		d.dispose();
	}
};

export function dispose(inputId) {
	let inputElement = document.getElementById(inputId);

	if (inputElement) {
		inputElement.removeEventListener('keydown', handleKeyDown);
		inputElement.hxAutosuggestDotnetObjectReference = null;
		inputElement.hxAutosuggestKeysToPreventDefault = null;
	}
}
