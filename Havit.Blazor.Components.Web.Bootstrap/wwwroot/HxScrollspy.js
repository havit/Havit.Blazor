export function initialize(element, targetId, options) {
	// HxScrollspy.SmoothScroll is not passed to Bootstrap (smoothScroll option): it is the CSS scroll-behavior of the container.
	// The Bootstrap click handler would resolve the URL fragment against <base href> (required by Blazor).
	const { smoothScroll, ...bootstrapOptions } = options ?? {}; // never passed to Bootstrap (see above)
	element.dataset.hxScrollspyTarget = targetId;
	element.addEventListener('activate.bs.scrollspy', handleActivate);
	new bootstrap.ScrollSpy(element, {
		...bootstrapOptions,
		target: '#' + targetId
	});
}

export function reinitialize(element, targetId, options) {
	const scrollspy = bootstrap.ScrollSpy.getInstance(element);
	if (!scrollspy || (typeof scrollspy._getConfig !== 'function')) {
		dispose(element);
		initialize(element, targetId, options);
		return;
	}

	// Update the configuration of the existing instance instead of creating a new one:
	// a new instance does not know the currently active section and highlights the first one until another section crosses the activation line.
	const { smoothScroll, ...bootstrapOptions } = options ?? {}; // never passed to Bootstrap (see initialize)
	element.dataset.hxScrollspyTarget = targetId;
	scrollspy._config = scrollspy._getConfig({
		...bootstrapOptions,
		target: '#' + targetId
	});
	scrollspy.refresh(); // rebuilds the observer with the new configuration
}

export function refresh(element) {
	const scrollspy = bootstrap.ScrollSpy.getInstance(element);
	if (!scrollspy) {
		return;
	}

	scrollspy.refresh();
}

export function dispose(element) {
	if (!element) {
		// the element has already been removed from the DOM (e.g. enhanced navigation replaced the page)
		return;
	}

	element.removeEventListener('activate.bs.scrollspy', handleActivate);

	const scrollspy = bootstrap.ScrollSpy.getInstance(element);
	if (scrollspy) {
		scrollspy.dispose();
	}
}

export function navigateToFragment(fragment) {
	// Used by HxScrollspyNavLink instead of NavigationManager.NavigateTo(), which (for the same page)
	// calls element.scrollIntoView() and scrolls all the scrollable ancestors including the page itself.
	const element = findElementByFragment(fragment);
	if (element) {
		scrollToElement(element);
	}

	// Resolve the fragment against the current URL (a relative '#fragment' would be resolved against <base href>).
	const url = new URL(window.location.href);
	url.hash = fragment;
	window.history.replaceState(window.history.state, '', url.href);

	if (element) {
		focusSection(element);
	}
}

function focusSection(element) {
	// Move the focus to the section (keyboard and assistive technology users).
	element.focus({ preventScroll: true });
	if ((document.activeElement !== element) && !element.hasAttribute('tabindex')) {
		// Not focusable on its own (e.g. a heading): make it focusable temporarily,
		// an element focusable by itself (e.g. a button) keeps its place in the Tab sequence.
		element.setAttribute('tabindex', '-1');
		element.addEventListener('blur', () => element.removeAttribute('tabindex'), { once: true });
		element.focus({ preventScroll: true });
	}
}

function scrollToElement(element) {
	let container = getScrollContainer(element);
	if (!container) {
		// the page itself is the scroll container
		element.scrollIntoView();
		return;
	}

	// Scroll the containers from the nearest one up to the HxScrollspy container (not the page):
	// the section in a nested scrollable panel is scrolled to the top of the panel, the panel to the top of the outer container, etc.
	// The destination does not depend on the current scroll position of the container (works even during a smooth scroll).
	// As element.scrollIntoView() does, the point to show (the section top minus its scroll-margin-top) is aligned
	// with the top of the scrollport of each container reduced by its scroll-padding-top (e.g. the space of a sticky header).
	// offset: the position of the point to show (the section top minus its scroll-margin-top) below the top of the current target,
	// including the distance a nested container could not scroll (clamped at the end of its content).
	let target = element;
	let offset = -(Number.parseFloat(getComputedStyle(element).scrollMarginTop) || 0);
	while (container) {
		const scrollPaddingTop = getScrollPaddingTop(container);
		// the position in the scroll coordinates of the container (clientTop: the container border, scrollTop is measured inside the border)
		const desiredTop = target.getBoundingClientRect().top - container.getBoundingClientRect().top - container.clientTop + container.scrollTop + offset - scrollPaddingTop;
		const maxTop = Math.max(container.scrollHeight - container.clientHeight, 0);
		const top = Math.min(Math.max(desiredTop, 0), maxTop);
		container.scrollTo({ top: top }); // scroll-behavior of the container applies, see .hx-scrollspy-smooth-scroll

		if (container.classList.contains('hx-scrollspy') || !container.parentElement?.closest('.hx-scrollspy')) {
			break;
		}
		// after the scroll, the point is (scrollPaddingTop + desiredTop - top) below the top of the container content (inside its border)
		offset = container.clientTop + scrollPaddingTop + (desiredTop - top);
		target = container;
		container = getScrollContainer(container);
	}
}

function getScrollPaddingTop(container) {
	// scroll-padding-top: auto (treated as 0), a length (resolved to px) or a percentage of the scrollport height (kept as a percentage by getComputedStyle)
	const scrollPaddingTop = getComputedStyle(container).scrollPaddingTop;
	const value = Number.parseFloat(scrollPaddingTop);
	if (!Number.isFinite(value)) {
		return 0;
	}
	return scrollPaddingTop.endsWith('%') ? (value * container.clientHeight / 100) : value;
}

function findElementByFragment(fragment) {
	if (!fragment || (fragment.length < 2)) {
		return null;
	}
	let id = fragment.slice(1);
	try {
		id = decodeURIComponent(id);
	}
	catch {
		// malformed escape sequence, use the fragment as it is
	}
	return document.getElementById(id);
}

function getScrollContainer(element) {
	// The nearest scrollable ancestor of the target.
	// The HxScrollspy container is used even when its content currently fits (there is nothing to scroll, the page must not scroll either),
	// other ancestors only when they really overflow (e.g. .table-responsive has overflow-y: auto computed from overflow-x: auto).
	// overflow: hidden containers are scrollable programmatically as well (overflow: clip is not).
	for (let parent = element.parentElement; parent && (parent !== document.body) && (parent !== document.documentElement); parent = parent.parentElement) {
		const overflowY = getComputedStyle(parent).overflowY;
		const isScrollable = (overflowY === 'auto') || (overflowY === 'scroll') || (overflowY === 'hidden');
		if (isScrollable
			&& (parent.classList.contains('hx-scrollspy') || (parent.scrollHeight > parent.clientHeight))) {
			return parent;
		}
	}
	return null;
}

function handleActivate(event) {
	// Bootstrap activates the menu toggle of an active menu item, but clears only [href].active,
	// so a <button> menu toggle would remain active after another section gets active.
	if (event.target !== event.currentTarget) {
		// activate.bs.scrollspy bubbles - ignore the events of a nested scrollspy
		return;
	}
	const link = event.relatedTarget;
	const navigation = document.getElementById(event.currentTarget.dataset.hxScrollspyTarget);
	if (!link || !navigation) {
		return;
	}
	for (const toggle of navigation.querySelectorAll('[data-bs-toggle="menu"].active:not([href])')) {
		const menu = toggle.nextElementSibling;
		if (!menu || !menu.contains(link)) {
			toggle.classList.remove('active');
		}
	}
}
