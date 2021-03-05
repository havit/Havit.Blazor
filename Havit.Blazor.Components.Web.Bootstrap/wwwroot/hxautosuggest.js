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
    event.target.hxAutosuggestDotnetObjectReference.invokeMethodAsync('HxAutosuggestInternal_HandleDropdownHidden');
    event.target.hxAutosuggestDotnetObjectReference = null;

//    var dropdown = bootstrap.Dropdown.getInstance(event.inputElement);
//    dropdown.dispose();
};

