function handleKeyDown(event) {
	if ((event.ctrlKey || event.metaKey) && event.key?.toLowerCase() === 'k') {
		event.preventDefault();
		handleKeyDown._dotnetObjectReference.invokeMethodAsync('FocusSearchInputAsync');
	}
}

export function initializeGlobalSearchShortcut(searchDotnetObjectReference) {
	handleKeyDown._dotnetObjectReference = searchDotnetObjectReference;
	window.addEventListener('keydown', handleKeyDown);
}

export function disposeGlobalSearchShortcut() {
	window.removeEventListener('keydown', handleKeyDown);
	handleKeyDown._dotnetObjectReference = null;
}