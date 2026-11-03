export function initialize(element, targetId, options) {
	const scrollspy = new bootstrap.ScrollSpy(element, {
		...options,
		target: '#' + targetId
	});

	if (options?.smoothScroll) {
		fixSmoothScrollNavigationUrl(scrollspy);
	}
}

export function reinitialize(element, targetId, options) {
	dispose(element);
	initialize(element, targetId, options);
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

export function navigateToFragment(fragment) {
	// Used by HxScrollspyNavLink instead of NavigationManager.NavigateTo(), which (for the same page)
	// calls element.scrollIntoView() and scrolls all the scrollable ancestors including the page itself.
	const element = findElementByFragment(fragment);
	if (element) {
		const container = getScrollContainer(element);
		if (container) {
			// scroll only the container (scroll-behavior of the container applies, see .hx-scrollspy-smooth-scroll)
			const scrollMarginTop = Number.parseFloat(getComputedStyle(element).scrollMarginTop) || 0;
			const top = element.getBoundingClientRect().top - container.getBoundingClientRect().top + container.scrollTop - scrollMarginTop;
			container.scrollTo({ top: top });
		}
		else {
			// the page itself is the scroll container
			element.scrollIntoView();
		}
	}

	// Resolve the fragment against the current URL (a relative '#fragment' would be resolved against <base href>).
	const url = new URL(window.location.href);
	url.hash = fragment;
	window.history.replaceState(window.history.state, '', url.href);
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
	for (let parent = element.parentElement; parent && (parent !== document.body) && (parent !== document.documentElement); parent = parent.parentElement) {
		if (isScrollableByOverflow(parent)
			&& (parent.classList.contains('hx-scrollspy') || (parent.scrollHeight > parent.clientHeight))) {
			return parent;
		}
	}
	return null;
}

function isScrollableByOverflow(element) {
	const overflowY = getComputedStyle(element).overflowY;
	return (overflowY === 'auto') || (overflowY === 'scroll');
}

function fixSmoothScrollNavigationUrl(scrollspy) {
	// With smoothScroll enabled, Bootstrap updates the URL by history.replaceState(null, '', '#fragment') once the scroll settles.
	// It also replaces the history state (null), so the original state is restored afterwards.
	// The relative '#fragment' URL is resolved against <base href> (required by Blazor), which would replace the current page path
	// (e.g. /components/HxScrollspy#section becomes /#section). Resolve the fragment against the current URL instead.
	const settleNavigation = scrollspy._settleNavigation;
	if (typeof settleNavigation !== 'function') {
		return;
	}

	scrollspy._settleNavigation = function (hash, section) {
		const url = new URL(window.location.href);
		url.hash = hash;
		// Bootstrap replaces the history state with null, which would drop the state Blazor keeps there.
		const state = window.history.state;
		const result = settleNavigation.call(this, url.href, section);
		window.history.replaceState(state, '', url.href);
		return result;
	};
}
