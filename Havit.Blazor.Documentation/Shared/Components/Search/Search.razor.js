let _handleKeyDown = null;

export function initializeGlobalSearchShortcut(searchDotnetObjectReference) {
	_handleKeyDown = (event) => {
		if ((event.ctrlKey || event.metaKey) && event.key?.toLowerCase() === 'k') {
			event.preventDefault();
			searchDotnetObjectReference.invokeMethodAsync('FocusSearchInput');
		}
	};

	window.addEventListener('keydown', _handleKeyDown);
}

export function disposeGlobalSearchShortcut() {
	if (_handleKeyDown) {
		window.removeEventListener('keydown', _handleKeyDown);
		_handleKeyDown = null;
	}
}