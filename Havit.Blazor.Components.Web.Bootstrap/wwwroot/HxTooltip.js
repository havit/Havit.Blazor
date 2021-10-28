// has to be aligned with HxPopover.js!
export function createOrUpdate(element) {
	destroy(element);
	new bootstrap.Tooltip(element)
}

export function destroy(element) {
	var tooltip = bootstrap.Tooltip.getInstance(element);
	if (tooltip) {
		tooltip.dispose();
	}
}