export function activate(element, targetId) {
	var scrollspy = new bootstrap.ScrollSpy(element, {
		target: '#' + targetId
	});
	scrollspy.refresh();
}