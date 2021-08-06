export function open(inputElement, hxAutosuggestDotnetObjectReference) {
    inputElement.setAttribute("data-bs-toggle", "dropdown");
    inputElement.hxAutosuggestDotnetObjectReference = hxAutosuggestDotnetObjectReference;
    inputElement.addEventListener('hidden.bs.dropdown', handleDropdownHidden)

    new bootstrap.Dropdown(inputElement).show();
}

export function destroy(inputElement) {
    inputElement.removeAttribute("data-bs-toggle", "dropdown");
    var dropdown = bootstrap.Dropdown.getInstance(inputElement);
    if (dropdown) {
        dropdown.hide();
        dropdown.dispose();
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
        element.hxAutosuggestDotnetObjectReference = null;
    }, 1, event.target);

//    var dropdown = bootstrap.Dropdown.getInstance(event.inputElement);
//    dropdown.dispose();
};

