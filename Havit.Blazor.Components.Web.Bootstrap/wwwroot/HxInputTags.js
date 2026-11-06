export function initialize(inputId, hxInputTagsDotnetObjectReference, keysToPrevendDefault) {
	const inputElement = document.getElementById(inputId);
	if (!inputElement) {
		return;
	}

	inputElement.hxInputTagsDotnetObjectReference = hxInputTagsDotnetObjectReference;
	inputElement.hxInputTagsKeysToPreventDefault = keysToPrevendDefault;

	inputElement.addEventListener('keydown', handleKeyDown);
	inputElement.addEventListener('blur', handleInputBlur);
}

function handleKeyDown(event) {
	const key = event.key;

	event.target.hxInputTagsDotnetObjectReference.invokeMethodAsync("HxInputTagsInternal_HandleInputKeyDown", key);

	if (event.target.hxInputTagsKeysToPreventDefault.includes(key)) {
		event.preventDefault();
	}
}

function handleInputBlur(event) {
	// We need the blur event to confirm custom tag creation.
	// When the menu is open, the blur event is fired before the click event on the menu item.
	// We need to recognize, whether the blur event is fired because of the menu item click or because of the user clicked somewhere else.
	// We will use relatedTarget property of the event to recognize the click on the menu item.
	// If relatedTarget is within the menu, we will ignore the blur event.
	let isWithinMenu = false;
	if (event.relatedTarget) {
		const menu = event.target.parentElement.querySelector('.menu');
		isWithinMenu = (menu !== null) && menu.contains(event.relatedTarget);
	}

	event.target.hxInputTagsDotnetObjectReference.invokeMethodAsync("HxInputTagsInternal_HandleInputBlur", isWithinMenu);
}

export function open(inputElement, hxInputTagsDotnetObjectReference, delayShow) {
	if (!inputElement) {
		return;
	}

	inputElement.setAttribute("data-bs-toggle", "menu");
	inputElement.hxInputTagsDotnetObjectReference = hxInputTagsDotnetObjectReference;
	inputElement.addEventListener('hidden.bs.menu', handleMenuHidden)

	const dd = new bootstrap.Menu(inputElement);
	if (!dd) {
		return;
	}
	if (!delayShow) {
		dd.show();
	}
	else {
		// Bootstrap menu registers onClick event handler which toggles the menu
		// For focus-triggered menus we need to delay the show() as the upcomming click event will toggle (= hide) the menu
		window.setTimeout(function (menu) {
			menu.show();
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

	inputElement.removeAttribute("data-bs-toggle", "menu");

	const menu = bootstrap.Menu.getInstance(inputElement);
	if (menu) {
		menu.hide();

		inputElement.addEventListener('hidden.bs.menu', event => {
			menu.dispose()
		})
	}
}

function handleMenuHidden(event) {
	event.target.removeEventListener('hidden.bs.menu', handleMenuHidden);

	destroy(event.target);

	// In Blazor, jsinterop is "faster" then events.
	// As a result, this method (handleMenuHidden) is first, menu item click event (Blazor OnClick Event) second.
	// But we need the item click event to fire first.
	// Therefore we delay jsinterop for a while.
	window.setTimeout(function (element) {
		element.hxInputTagsDotnetObjectReference?.invokeMethodAsync('HxInputTagsInternal_HandleMenuHidden');
	}, 1, event.target);
}

export function dispose(inputId) {
	const inputElement = document.getElementById(inputId);

	inputElement.removeEventListener('keydown', handleKeyDown);
	inputElement.removeEventListener('blur', handleInputBlur);
	inputElement.hxInputTagsDotnetObjectReference = null;
	inputElement.hxInputTagsKeysToPreventDefault = null;
}