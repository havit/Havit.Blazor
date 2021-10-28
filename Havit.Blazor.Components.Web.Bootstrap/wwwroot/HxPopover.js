// has to be aligned with HxTooltip.js!
export function createOrUpdate(element) {
	destroy(element);
	new bootstrap.Popover(element)
}

export function destroy(element) {
	var popover = bootstrap.Popover.getInstance(element);
	if (popover) {
		popover.dispose();
	}
}