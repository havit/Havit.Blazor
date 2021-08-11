export function initialize(element, targetId) {
	var scrollspy = new bootstrap.ScrollSpy(element, {
		target: '#' + targetId
	});
}

export function refresh(element) {
	var scrollspy = bootstrap.ScrollSpy.getInstance(element);

	if (element.scrollTop > 0) {
		// scrollspy calculates the offsets properly only if the container is scrolled to
		element.scrollTo(0, 0);
	}
	scrollspy.refresh();
}

export function dispose(element) {
	var scrollspy = bootstrap.ScrollSpy.getInstance(element);
	scrollspy.dispose();
}