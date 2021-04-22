export function createOrUpdate(element) {
	dispose(element);
	new bootstrap.Tooltip(element)
}

export function dispose(element) {
	var tooltip = bootstrap.Tooltip.getInstance(element);
	if (tooltip) {
		tooltip.dispose();
	}
}