function initialize() {
	window.HavitBlazorStorage = window.HavitBlazorStorage || {}

	window.HavitBlazorStorage.getLength = function (storageName) {
		return window[storageName].length
	}
}

// Blazor Server and standalone WebAssembly apps.
export function afterStarted() {
	initialize()
}

// Blazor Web App.
export function afterWebStarted() {
	initialize()
}