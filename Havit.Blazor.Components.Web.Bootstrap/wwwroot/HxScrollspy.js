export function initialize(element, targetId) {
	const scrollspy = new bootstrap.ScrollSpy(element, {
		target: '#' + targetId
	});
}

export function refresh(element) {
	const scrollspy = bootstrap.ScrollSpy.getInstance(element);

	if (element.scrollTop > 0) {
		// scrollspy calculates the offsets properly only if the container is scrolled to
		element.scrollTo(0, 0);
	}
	scrollspy.refresh();
}

export function dispose(element) {
	const scrollspy = bootstrap.ScrollSpy.getInstance(element);
	scrollspy.dispose();
}