export function scrollToAnchor(anchor) {
	var selector = anchor || document.location.hash;
	if (selector && (selector.length > 1)) {
		var element = null;
		try {
			element = document.querySelector(selector);
		}
		catch (error) {
			console.warn(error);
		}
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