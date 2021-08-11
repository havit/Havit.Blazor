export function scrollToAnchor(anchor) {
	var selector = anchor || document.location.hash;
	if (selector && (selector.length > 1)) {
		var element = document.querySelector(selector);
		if (element) {
			element.scrollIntoView({
				behavior: 'smooth'
			});
		}
		console.log("scrollIntoView");
	}
	else {
		window.scroll(0, 0);
	}
}