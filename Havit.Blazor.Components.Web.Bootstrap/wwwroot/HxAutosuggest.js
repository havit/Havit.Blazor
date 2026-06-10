export function initialize(inputId, hxAutosuggestDotnetObjectReference, keysToPreventDefault) {
	const inputElement = document.getElementById(inputId);
	if (!inputElement) {
		return;
	}

	inputElement.hxAutosuggestDotnetObjectReference = hxAutosuggestDotnetObjectReference;
	inputElement.hxAutosuggestKeysToPreventDefault = keysToPreventDefault;
	inputElement.clickIsComing = false;

	inputElement.addEventListener('keydown', handleKeyDown);

	inputElement.addEventListener('mousedown', handleMouseDown);
	inputElement.addEventListener('mouseup', handleMouseUp);
	inputElement.addEventListener('mouseleave', handleMouseLeave);
}

function handleKeyDown(event) {
	const key = event.key;

	event.target.hxAutosuggestDotnetObjectReference.invokeMethodAsync("HxAutosuggestInternal_HandleInputKeyDown", key);

	if (event.target.hxAutosuggestKeysToPreventDefault.includes(key)) {
		event.preventDefault();
	}
}

export function scrollToSelectedItem(menuId) {
	const menuElement = document.getElementById(menuId);
	if (!menuElement) {
		return;
	}

	const selectedItem = menuElement.getElementsByClassName("hx-autosuggest-item-focused");
	if (!selectedItem) {
		return;
	}

	selectedItem[0].scrollIntoView({ block: "nearest", inline: "nearest" });
}

export function open(inputElement, hxAutosuggestDotnetObjectReference) {
	if (!inputElement) {
		return;
	}
	inputElement.setAttribute("data-bs-toggle", "menu");
	inputElement.hxAutosuggestDotnetObjectReference = hxAutosuggestDotnetObjectReference;
	inputElement.addEventListener('hidden.bs.menu', handleMenuHidden);

	const d = new bootstrap.Menu(inputElement);
	if (d && (inputElement.clickIsComing === false)) {
		// clickIsComing logic fixes #572 - Initial suggestions disappear when the DataProvider response is quick
		// If click is coming, we do not want to show the menu as it will be toggled by the later click event (if we open it here, onfocus, click will hide it)
		d.show();
	}
}

export function destroy(inputElement) {
	if (!inputElement) {
		return;
	}

	inputElement.removeAttribute("data-bs-toggle", "menu");

	const d = bootstrap.Menu.getInstance(inputElement);
	if (d) {
		inputElement.addEventListener('hidden.bs.menu', event => {
			d.dispose()
		});
		try {
			// Blazor may have already removed the elements from the DOM (Cannot read properties of null (reading 'focus')).
			d.hide();
		}
		catch (e)
		{
			// NOOP
			console.debug(e);
		}
	}
}

function handleMouseDown(event) {
	event.target.clickIsComing = true;
}
function handleMouseUp(event) {
	event.target.clickIsComing = false;
}

function handleMouseLeave(event) {
	// Fixes #702: The menu is not shown after selecting input content with the mouse
	// (the input does not receive mouseup when the mouse is released outside of the input)
	event.target.clickIsComing = false;
}

function handleMenuHidden(event) {
	event.target.removeEventListener('hidden.bs.menu', handleMenuHidden);

	// In Blazor, JS interop is faster than DOM events.
	// As a result, this method (handleMenuHidden) is called first, and the menu item click event (Blazor OnClick Event) is called second.
	// However, we need the item click event to fire first.
	// Therefore, we delay the JS interop call slightly.
	window.setTimeout(function (element) {
		element.hxAutosuggestDotnetObjectReference?.invokeMethodAsync('HxAutosuggestInternal_HandleMenuHidden');
	}, 1, event.target);

	const d = bootstrap.Menu.getInstance(event.target);
	if (d) {
		d.dispose();
	}
};

export function dispose(inputId) {
	const inputElement = document.getElementById(inputId);

	if (inputElement) {
		inputElement.removeEventListener('keydown', handleKeyDown);
		inputElement.removeEventListener('mouseup', handleMouseUp);
		inputElement.removeEventListener('mousedown', handleMouseDown);
		inputElement.removeEventListener('mouseleave', handleMouseLeave);
		inputElement.hxAutosuggestDotnetObjectReference = null;
		inputElement.hxAutosuggestKeysToPreventDefault = null;
	}
}
