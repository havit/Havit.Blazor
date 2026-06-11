const overflowAttribute = 'data-bs-nav-overflow'; // styled by Bootstrap (.nav-overflow [data-bs-nav-overflow="true"] { display: none; })
const keepCssClass = 'nav-overflow-keep';
const widthBuffer = 10; // safety buffer in px (mirrors the Bootstrap NavOverflow plugin)

export function initialize(element, hxNavOverflowDotnetObjectReference, options) {
	if (!element) {
		return;
	}

	let state = element.hxNavOverflow;
	if (!state) {
		state = {
			dotnetObjectReference: hxNavOverflowDotnetObjectReference,
			options: options,
			lastOverflowCount: 0
		};
		element.hxNavOverflow = state;

		if (typeof ResizeObserver !== 'undefined') {
			state.resizeObserver = new ResizeObserver(() => recalculate(element));
			state.resizeObserver.observe(element);
		}
		else {
			// fallback for older browsers
			state.windowResizeHandler = () => recalculate(element);
			window.addEventListener('resize', state.windowResizeHandler);
		}

		// Items can be re-rendered by Blazor at any time (visibility attributes of recreated elements get lost,
		// added/removed/resized items change the layout) - recalculate on any mutation except our own.
		state.mutationObserver = new MutationObserver(mutations => {
			if (mutations.every(m => (m.type === 'attributes') && (m.attributeName === overflowAttribute))) {
				return; // our own visibility changes
			}
			recalculate(element);
		});
		state.mutationObserver.observe(element, { subtree: true, childList: true, attributes: true, characterData: true });
	}
	else {
		state.dotnetObjectReference = hxNavOverflowDotnetObjectReference;
		state.options = options;
	}

	recalculate(element);
}

export function update(element) {
	if (!element?.hxNavOverflow) {
		return;
	}
	recalculate(element);
}

export function dispose(element) {
	if (!element) {
		return;
	}

	const state = element.hxNavOverflow;
	if (!state) {
		return;
	}

	state.resizeObserver?.disconnect();
	state.mutationObserver?.disconnect();
	if (state.windowResizeHandler) {
		window.removeEventListener('resize', state.windowResizeHandler);
	}
	element.hxNavOverflow = null;

	// restore item visibility
	const { moreItem, items, menuItems } = getParts(element);
	for (const item of [...items, ...menuItems]) {
		item.removeAttribute(overflowAttribute);
	}
	moreItem?.setAttribute(overflowAttribute, 'true');
}

function getParts(element) {
	const moreItem = element.querySelector(':scope > .nav-overflow-item');
	const menu = moreItem?.querySelector('.nav-overflow-menu');
	return {
		moreItem: moreItem,
		items: [...element.children].filter(child => child !== moreItem),
		menuItems: menu ? [...menu.children] : []
	};
}

function resolveCollapseBelow(value) {
	if (!value) {
		return 0;
	}

	const numericValue = Number.parseFloat(value);
	if (!Number.isNaN(numericValue)) {
		return numericValue; // direct pixel value
	}

	// breakpoint name, e.g. "md"
	const cssValue = getComputedStyle(document.documentElement).getPropertyValue(`--bs-breakpoint-${value}`);
	return Number.parseFloat(cssValue) || 0;
}

function recalculate(element) {
	const state = element.hxNavOverflow;
	if (!state) {
		return;
	}

	const { moreItem, items, menuItems } = getParts(element);
	if (!moreItem) {
		return;
	}

	// restore all items (and the "More" item) to measure the natural widths
	// (synchronous remeasure and reapply, no repaint in between)
	for (const item of items) {
		item.removeAttribute(overflowAttribute);
	}
	moreItem.removeAttribute(overflowAttribute);

	const navWidth = element.offsetWidth;
	const collapseBelow = resolveCollapseBelow(state.options?.collapseBelow);
	const minimumVisibleItems = state.options?.minimumVisibleItems ?? 0;

	let overflowingItems;
	if ((collapseBelow > 0) && (navWidth < collapseBelow)) {
		// below the collapse threshold all items overflow (except keep items)
		overflowingItems = items.filter(item => !item.classList.contains(keepCssClass));
	}
	else {
		// keep items are always visible; subtract their widths so the threshold reflects the actual space available for the remaining items
		const keepWidth = items
			.filter(item => item.classList.contains(keepCssClass))
			.reduce((sum, item) => sum + item.offsetWidth, 0);
		const availableWidth = navWidth - moreItem.offsetWidth - keepWidth - widthBuffer;

		let usedWidth = 0;
		overflowingItems = [];
		for (const item of items) {
			if (item.classList.contains(keepCssClass)) {
				continue;
			}

			usedWidth += item.offsetWidth;
			if (usedWidth > availableWidth) {
				overflowingItems.push(item);
			}
		}

		// ensure the minimum number of visible items
		const visibleCount = items.length - overflowingItems.length;
		if ((visibleCount < minimumVisibleItems) && (items.length > minimumVisibleItems)) {
			overflowingItems = items.slice(minimumVisibleItems).filter(item => !item.classList.contains(keepCssClass));
		}
	}

	// apply the visibility (nav items hidden when overflowing, menu items visible when overflowing)
	const overflowingItemsSet = new Set(overflowingItems);
	for (const [index, item] of items.entries()) {
		const overflowing = overflowingItemsSet.has(item);
		if (overflowing) {
			item.setAttribute(overflowAttribute, 'true');
		}

		const menuItem = menuItems[index];
		if (menuItem) {
			if (overflowing) {
				menuItem.removeAttribute(overflowAttribute);
			}
			else {
				menuItem.setAttribute(overflowAttribute, 'true');
			}
		}
	}

	if (overflowingItems.length === 0) {
		moreItem.setAttribute(overflowAttribute, 'true');
	}

	if (state.lastOverflowCount !== overflowingItems.length) {
		state.lastOverflowCount = overflowingItems.length;
		state.dotnetObjectReference.invokeMethodAsync('HxNavOverflow_HandleOverflowChanged', overflowingItems.length, items.length - overflowingItems.length);
	}
}
