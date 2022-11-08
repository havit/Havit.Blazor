export function initialize(inputId, hxSearchBoxDotnetObjectReference, keysToPreventDefault) {
	let inputElement = document.getElementById(inputId);
	if (!inputElement) {
		return;
	}

    inputElement.hxSearchBoxDotnetObjectReference = hxSearchBoxDotnetObjectReference;
	inputElement.hxSearchBoxKeysToPreventDefault = keysToPreventDefault;

    inputElement.addEventListener('keydown', handleKeyDown);
}

function handleKeyDown(event) {
    let key = event.key;

    event.target.hxSearchBoxDotnetObjectReference.invokeMethodAsync("HxSearchBox_HandleInputKeyDown", key);

	if (event.target.hxSearchBoxKeysToPreventDefault.includes(key)) {
        event.preventDefault();
    }
}

export function dispose(inputId) {
    let inputElement = document.getElementById(inputId);

    inputElement.removeEventListener('keydown', handleKeyDown);
    inputElement.hxSearchBoxDotnetObjectReference = null;
	inputElement.hxSearchBoxKeysToPreventDefault = null;
}
