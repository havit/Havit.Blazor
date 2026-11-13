export function scrollToAnchor(anchor) {
	const selector = anchor || document.location.hash;
	if (selector && (selector.length > 1)) {
		const element = findElement(selector);
		if (element) {
			element.scrollIntoView({
				behavior: 'smooth'
			});
			console.debug("scrollIntoView: " + selector);
		}
	}
	else {
		window.scroll(0, 0);
	}
}

function findElement(selector) {
	// The URI fragment is escaped (e.g. diacritics) and an id is not always a valid CSS selector
	// (starts with a digit, contains dots or colons) - look the element up by its id.
	if (selector.startsWith('#')) {
		const element = document.getElementById(decodeFragment(selector.slice(1)));
		if (element) {
			return element;
		}
	}

	// ScrollToAnchorAsync has always accepted any CSS selector.
	try {
		return document.querySelector(selector);
	}
	catch {
		return null;
	}
}

function decodeFragment(fragment) {
	try {
		return decodeURIComponent(fragment);
	}
	catch {
		return fragment; // malformed escape sequence, use the fragment as it is
	}
}
