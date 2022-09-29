export function initialize(inputId, hxSearchBoxDotnetObjectReference, keys) {
    let inputElement = document.getElementById(inputId);

    inputElement.hxSearchBoxDotnetObjectReference = hxSearchBoxDotnetObjectReference;
    inputElement.keys = keys;

    inputElement.addEventListener('keydown', handleKeyDown);
}

function handleKeyDown(event) {
    let key = event.key;

    event.target.hxSearchBoxDotnetObjectReference.invokeMethodAsync("HxSearchBox_HandleInputKeyDown", key);

    if (event.target.keys.includes(key)) {
        event.preventDefault();
    }
}

export function dispose(inputId) {
    let inputElement = document.getElementById(inputId);

    inputElement.removeEventListener('keydown', handleKeyDown);
    inputElement.hxSearchBoxDotnetObjectReference = null;
    inputElement.keys = null;
}
