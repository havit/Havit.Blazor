export function create(element, hxDropdownDotnetObjectReference) {
	element.hxDropdownDotnetObjectReference = hxDropdownDotnetObjectReference;
	element.addEventListener('shown.bs.dropdown', handleDropdownShown);
	element.addEventListener('hidden.bs.dropdown', handleDropdownHidden);
	var d = new bootstrap.Dropdown(element);
	console.debug("Dropdown_create");
}

function handleDropdownShown(event) {
	event.target.parentElement.hxDropdownDotnetObjectReference.invokeMethodAsync('HxDropdown_HandleJsShown');
	console.debug("Dropdown_shown");
};

function handleDropdownHidden(event) {
	event.target.parentElement.hxDropdownDotnetObjectReference.invokeMethodAsync('HxDropdown_HandleJsHidden');
	console.debug("Dropdown_hidden");
};

export function dispose(element) {
	element.removeEventListener('shown.bs.dropdown', handleDropdownShown);
	element.removeEventListener('hidden.bs.dropdown', handleDropdownHidden);
	element.hxDropdownDotnetObjectReference = null;
	console.debug("Dropdown_dispose");
}
