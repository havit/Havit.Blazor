export function addOpenAndCloseEventListeners(triggerElement) {
	if (!triggerElement) {
		return;
    }

	triggerElement.addEventListener('shown.bs.dropdown', handleDropdownShown);
	triggerElement.addEventListener('hidden.bs.dropdown', handleDropdownHidden);
}

export function open(triggerElement) {
	if (!triggerElement) {
		return;
	}

    new bootstrap.Dropdown(triggerElement).show();
}

export function toggle(triggerElement) {
	if (!triggerElement || triggerElement.dropdownMenuShown) {
		return;
	}

	open(triggerElement);
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

function handleDropdownShown(event) {
	event.target.dropdownMenuShown = true;
}

function handleDropdownHidden(event) {
	event.target.dropdownMenuShown = false;
};

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
