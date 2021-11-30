export function show(element, hxToastDotnetObjectReference) {
	if (!element) {
		return;
	}

	element.hxToastDotnetObjectReference = hxToastDotnetObjectReference;
	element.addEventListener('hidden.bs.toast', handleToastHidden);

	var toast = new bootstrap.Toast(element);
	if (toast) {
		toast.show();
	}
}

function handleToastHidden(event) {
	event.target.hxToastDotnetObjectReference.invokeMethodAsync('HxToast_HandleToastHidden');
};

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('hidden.bs.toast', handleToastHidden);
	element.hxToastDotnetObjectReference = null;

	var t = bootstrap.Toast.getInstance(element);
	if (t) {
		t.dispose();
	}
}