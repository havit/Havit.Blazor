export function show(element, hxToastDotnetObjectReference) {
	if (element == null || hxToastDotnetObjectReference == null) {
		return;
	}

	element.hxToastDotnetObjectReference = hxToastDotnetObjectReference;
	element.addEventListener('hidden.bs.toast', handleToastHidden);

	var toast = new bootstrap.Toast(element);
	toast.show();
}

export function dispose(element) {
	if (element == null) {
		return;
    }

	element.removeEventListener('hidden.bs.toast', handleToastHidden);
	element.hxToastDotnetObjectReference = null;
}

function handleToastHidden(event) {
	event.target.hxToastDotnetObjectReference.invokeMethodAsync('HxToast_HandleToastHidden');
};
