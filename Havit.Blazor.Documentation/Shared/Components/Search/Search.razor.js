export function initializeGlobalSearchShortcut(searchDotnetObjectReference) {
	const handleKeyDown = (event) => {
		if ((event.ctrlKey || event.metaKey) && event.key?.toLowerCase() === 'k') {
			event.preventDefault();
			searchDotnetObjectReference.invokeMethodAsync('FocusSearchInput');
		}
	};

	window.addEventListener('keydown', handleKeyDown);

	return () => {
		window.removeEventListener('keydown', handleKeyDown);
	};
}

export function disposeGlobalSearchShortcut(removeEventListener) {
	if (removeEventListener) {
		removeEventListener();
	}
}