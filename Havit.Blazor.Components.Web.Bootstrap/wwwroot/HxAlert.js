export function initialize(element, hxAlertDotnetObjectReference) {
	if (!element) {
		return;
	}

	element.hxAlertDotnetObjectReference = hxAlertDotnetObjectReference;
	element.addEventListener('close.bs.alert', handleAlertClose);
	element.addEventListener('closed.bs.alert', handleAlertClosed);
}

function handleAlertClose(event) {
	// remember the position, Bootstrap removes the alert element from the DOM before raising closed.bs.alert
	const element = event.target;
	element.hxAlertParentNode = element.parentNode;
	element.hxAlertNextSibling = element.nextSibling;
}

function handleAlertClosed(event) {
	const element = event.target;

	// Blazor still owns the element (and fails when removing/inserting around a detached node),
	// return it to its original position, hidden, so that Blazor can remove it later.
	const parentNode = element.hxAlertParentNode;
	if (parentNode && !element.isConnected) {
		const nextSibling = element.hxAlertNextSibling;
		element.style.display = 'none';
		parentNode.insertBefore(element, (nextSibling?.parentNode === parentNode) ? nextSibling : null);
	}
	element.hxAlertParentNode = null;
	element.hxAlertNextSibling = null;

	element.hxAlertDotnetObjectReference?.invokeMethodAsync('HxAlert_HandleJsClosed');
};

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('close.bs.alert', handleAlertClose);
	element.removeEventListener('closed.bs.alert', handleAlertClosed);
	element.hxAlertDotnetObjectReference = null;
}
