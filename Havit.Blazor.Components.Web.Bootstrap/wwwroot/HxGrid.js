export function resetScrollPosition(element) {
	if (!element) {
		return;
	}
	element.scrollTo(0, 0);
}
