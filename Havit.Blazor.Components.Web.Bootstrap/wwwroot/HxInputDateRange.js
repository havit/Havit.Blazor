export function open(triggerElement) {
	if (!triggerElement) {
		return;
	}
    new bootstrap.Dropdown(triggerElement).show();
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
