export function initialize(rangeElement, bubble) {
	if (!rangeElement) {
		return;
	}

	// idempotent (re)initialization - e.g. when the Bubble/Ticks/Min/Max parameters change, the plugin is reinitialized
	dispose(rangeElement);

	bootstrap.Range.getOrCreateInstance(rangeElement, { bubble: bubble });
}

export function update(rangeElement) {
	if (!rangeElement) {
		return;
	}

	// recompute the fill (and the bubble text) after a programmatic value change - the plugin only listens to input/change DOM events
	bootstrap.Range.getInstance(rangeElement)?.update();
}

export function dispose(rangeElement) {
	if (!rangeElement) {
		return;
	}

	bootstrap.Range.getInstance(rangeElement)?.dispose();
}
