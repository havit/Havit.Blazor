export function addOpenAndCloseEventListeners(triggerElement, iconWrapperElement) {
	console.warn("addOpenAndCloseEventListeners");
	if (!triggerElement) {
		return;
	}

	triggerElement.addEventListener('shown.bs.dropdown', handleDropdownShown);
	triggerElement.addEventListener('hidden.bs.dropdown', handleDropdownHidden);

	if (!iconWrapperElement) {
		return;
	}

	iconWrapperElement.addEventListener('click', handleIconClick);
	iconWrapperElement.triggerElement = triggerElement;
}

export function open(triggerElement) {
	console.warn("open");
	if (!triggerElement) {
		return;
	}

	new bootstrap.Dropdown(triggerElement).show();
}

function handleIconClick(event) {
	console.warn("icon-click");
	var triggerElement = event.currentTarget.triggerElement;
	triggerElement.click();
	event.stopPropagation();
}

export function toggle(triggerElement) {
	console.warn("toggle");
	if (!triggerElement) {
		return;
	}

	if (triggerElement.dropdownMenuShown) {
		destroy(triggerElement);
	}
	else {
		open(triggerElement);
	}
}

export function destroy(triggerElement) {
	console.warn("destroy");
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
	console.warn("shown");
	event.target.dropdownMenuShown = true;
}

function handleDropdownHidden(event) {
	console.warn("hidden");
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
