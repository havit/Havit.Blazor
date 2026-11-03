export function initialize(element, targetId, options) {
	// Bootstrap smoothScroll is not used: its click handler resolves the URL fragment against <base href> (required by Blazor),
	// replaces the history state with null, runs before HxScrollspyNavLink.OnClick and does not ignore controls (buttons) inside the links.
	const { smoothScroll, ...bootstrapOptions } = options ?? {};
	new bootstrap.ScrollSpy(element, {
		...bootstrapOptions,
		target: '#' + targetId
	});

	if (smoothScroll) {
		enableSmoothScroll(element, targetId);
	}
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
	disableSmoothScroll(element);
	const { smoothScroll, ...bootstrapOptions } = options ?? {};
	scrollspy._config = scrollspy._getConfig({
		...bootstrapOptions,
		target: '#' + targetId
	});
	scrollspy.refresh(); // rebuilds the observer with the new configuration

	if (smoothScroll) {
		enableSmoothScroll(element, targetId);
	}
}

export function refresh(element) {
	const scrollspy = bootstrap.ScrollSpy.getInstance(element);
	if (!scrollspy) {
		return;
	}

	scrollspy.refresh();
}

export function dispose(element) {
	disableSmoothScroll(element);

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
			// clientTop: the container border (scrollTop is measured inside the border)
			const top = element.getBoundingClientRect().top - container.getBoundingClientRect().top - container.clientTop + container.scrollTop - scrollMarginTop;
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

const smoothScrollRegistrations = new WeakMap();

function enableSmoothScroll(element, targetId) {
	const navigation = document.getElementById(targetId);
	if (!navigation) {
		return;
	}
	const handler = event => handleSmoothScrollClick(event, navigation);
	navigation.addEventListener('click', handler);
	smoothScrollRegistrations.set(element, { navigation, handler });
}

function disableSmoothScroll(element) {
	const registration = smoothScrollRegistrations.get(element);
	if (registration) {
		registration.navigation.removeEventListener('click', registration.handler);
		smoothScrollRegistrations.delete(element);
	}
}

function handleSmoothScrollClick(event, navigation) {
	if (event.defaultPrevented || (event.button !== 0) || event.ctrlKey || event.shiftKey || event.altKey || event.metaKey) {
		return;
	}

	const link = event.target.closest('a[href]');
	if (!link || !navigation.contains(link)) {
		return;
	}

	if (link.hasAttribute('data-hx-scrollspy-nav-link')) {
		// HxScrollspyNavLink navigates on its own (after its OnClick callback)
		return;
	}

	const control = event.target.closest('button, input, select, textarea, label, [role="button"]');
	if (control && (control !== link) && link.contains(control)) {
		// a click on a control placed inside the link is not a navigation
		return;
	}

	const fragment = link.getAttribute('href');
	if (!fragment || !fragment.startsWith('#')) {
		return;
	}
	const section = findElementByFragment(fragment);
	if (!section) {
		return;
	}

	event.preventDefault(); // otherwise the link would be resolved against <base href>
	navigateToFragment(fragment);

	// move the focus to the section (keyboard and assistive technology users), the same way Bootstrap smoothScroll does
	if (!section.hasAttribute('tabindex')) {
		section.setAttribute('tabindex', '-1');
	}
	section.focus({ preventScroll: true });
}
