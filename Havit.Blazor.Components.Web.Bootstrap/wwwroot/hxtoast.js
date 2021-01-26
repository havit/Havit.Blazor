export function show(element, hxToastDotnetObjectReference) {
	element.hxToastDotnetObjectReference = hxToastDotnetObjectReference;
	element.addEventListener('hidden.bs.toast', handleToastHidden);

	var toast = new bootstrap.Toast(element);
	toast.show();
}

export function dispose(element) {
	element.removeEventListener('hidden.bs.toast', handleToastHidden);
	element.hxToastDotnetObjectReference = null;
}

function handleToastHidden(event) {
	event.target.hxToastDotnetObjectReference.invokeMethodAsync('HxToast_HandleToastHidden');
};
