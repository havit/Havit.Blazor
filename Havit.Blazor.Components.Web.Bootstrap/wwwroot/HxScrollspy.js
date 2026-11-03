export function initialize(element, targetId, options) {
	// Bootstrap smoothScroll is not used: its click handler resolves the URL fragment against <base href> (required by Blazor),
	// replaces the history state with null, runs before HxScrollspyNavLink.OnClick and does not ignore controls (buttons) inside the links.
	const { smoothScroll, ...bootstrapOptions } = options ?? {};
	element.dataset.hxScrollspyTarget = targetId;
	element.addEventListener('activate.bs.scrollspy', handleActivate);
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
	element.dataset.hxScrollspyTarget = targetId;
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
	// offset: the position of the point to show (the section top minus its scroll-margin-top) below the top of the current target,
	// including the distance a nested container could not scroll (clamped at the end of its content).
	let target = element;
	let offset = -(Number.parseFloat(getComputedStyle(element).scrollMarginTop) || 0);
	while (container) {
		// the position in the scroll coordinates of the container (clientTop: the container border, scrollTop is measured inside the border)
		const desiredTop = target.getBoundingClientRect().top - container.getBoundingClientRect().top - container.clientTop + container.scrollTop + offset;
		const maxTop = Math.max(container.scrollHeight - container.clientHeight, 0);
		const top = Math.min(Math.max(desiredTop, 0), maxTop);
		container.scrollTo({ top: top }); // scroll-behavior of the container applies, see .hx-scrollspy-smooth-scroll

		if (container.classList.contains('hx-scrollspy') || !container.parentElement?.closest('.hx-scrollspy')) {
			break;
		}
		// after the scroll, the point is (desiredTop - top) below the top of the container content (inside its border)
		offset = container.clientTop + (desiredTop - top);
		target = container;
		container = getScrollContainer(container);
	}
}

function isDisabledLink(link) {
	// the same rules as Bootstrap ScrollSpy uses to ignore disabled links
	return link.classList.contains('disabled')
		|| (link.hasAttribute('disabled') && (link.getAttribute('disabled') !== 'false'))
		|| (link.getAttribute('aria-disabled') === 'true');
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

const smoothScrollRegistrations = new WeakMap();

function enableSmoothScroll(element, targetId) {
	const navigation = document.getElementById(targetId);
	if (!navigation) {
		return;
	}
	const handler = event => handleSmoothScrollClick(event, navigation, element);
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

function handleSmoothScrollClick(event, navigation, scrollspyElement) {
	if (event.defaultPrevented || (event.button !== 0) || event.ctrlKey || event.shiftKey || event.altKey || event.metaKey) {
		return;
	}

	const link = event.target.closest('a[href]');
	if (!link || !navigation.contains(link)) {
		return;
	}

	if (isDisabledLink(link)) {
		// keyboard activation of a disabled link (pointer-events: none blocks the mouse only); the default would navigate via <base href>
		event.preventDefault();
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
	if (!section || !scrollspyElement.contains(section)) {
		// not a section of this scrollspy (e.g. a link to a page-level heading), leave the link alone
		return;
	}

	event.preventDefault(); // otherwise the link would be resolved against <base href>
	navigateToFragment(fragment);
}
