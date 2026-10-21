export function initialize(element, targetId) {
	const scrollspy = new bootstrap.ScrollSpy(element, {
		target: '#' + targetId
	});
}

export function refresh(element) {
	const scrollspy = bootstrap.ScrollSpy.getInstance(element);
	if (!scrollspy) {
		return;
	}

	scrollspy.refresh();
}

export function dispose(element) {
	const scrollspy = bootstrap.ScrollSpy.getInstance(element);
	if (scrollspy) {
		scrollspy.dispose();
	}
}