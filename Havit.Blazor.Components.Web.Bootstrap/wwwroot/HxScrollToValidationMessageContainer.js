const findGroupFromMessage = (messageElement) => {
	const inputGroup =
		messageElement.closest(".hx-form-group")  // Hx component
		|| messageElement.closest(".hx-autosuggest") // Hx component
		|| messageElement.closest(".hx-validation-scrollable-group") // Vanilla component wrapper

	return inputGroup;
}

export function tryScrollToFirstError(
	scrollWrapperElement = new Element(),
	validationMessageClass = ""
) {
	var firstValidationMessage = scrollWrapperElement.getElementsByClassName(validationMessageClass)[0];
	if (firstValidationMessage == null) {
		return;
	}

	var inputGroup = findGroupFromMessage(firstValidationMessage);
	if (inputGroup == null) {
		return;
	}

	if (isVisibleOnScreen(inputGroup)) {
		return;
	}

	inputGroup.scrollIntoView();
}

/**
 * Checks that an element is visible in the viewport
 * and not covered by another element (like a header)
 * @param {Element} element
 * @returns
 */
function isVisibleOnScreen(element) {
	const rect = element.getBoundingClientRect();

	// Check if element is within the viewport
	const inViewport =
		rect.bottom >= 0 &&
		rect.right >= 0 &&
		rect.top <= (window.innerHeight || document.documentElement.clientHeight) &&
		rect.left <= (window.innerWidth || document.documentElement.clientWidth);

	if (!inViewport) return false;

	// Get the center point of the element
	const centerX = rect.left + rect.width / 2;
	const centerY = rect.top + rect.height / 2;

	// Find the topmost element at that point
	const topElement = document.elementFromPoint(centerX, centerY);

	// Check if the target element contains that point or is on top
	return element.contains(topElement) || topElement === element;
}
