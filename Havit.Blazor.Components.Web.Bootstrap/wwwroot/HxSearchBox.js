export function initialize(inputId, hxSearchBoxDotnetObjectReference, keys) {
    let inputElement = document.getElementById(inputId);

    inputElement.onkeydown = function (e) {
        let key = e.key;

        hxSearchBoxDotnetObjectReference.invokeMethodAsync("HxSearchBox_HandleInputKeyDown", key);

        if (keys.includes(key)) {
            e.preventDefault();
        }
    }
}