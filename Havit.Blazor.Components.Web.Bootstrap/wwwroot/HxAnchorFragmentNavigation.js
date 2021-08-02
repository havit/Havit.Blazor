export function scrollToAnchor(anchor) {
	var selector = anchor || document.location.hash;
	console.log("selector:" + selector);
	if (selector && (selector.length > 1)) {
		var element = document.querySelector(selector);
		if (element) {

			console.log("scrollIntoView");

			element.scrollIntoView({
				behavior: 'smooth'
			});
		}
	}
	else {
		console.log("scroll(0,0)");

		window.scroll(0, 0);
	}
}