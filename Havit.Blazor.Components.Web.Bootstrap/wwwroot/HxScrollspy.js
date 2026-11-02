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

function fixSmoothScrollNavigationUrl(scrollspy) {
	// With smoothScroll enabled, Bootstrap updates the URL by history.replaceState(null, '', '#fragment') once the scroll settles.
	// The relative '#fragment' URL is resolved against <base href> (required by Blazor), which would replace the current page path
	// (e.g. /components/HxScrollspy#section becomes /#section). Resolve the fragment against the current URL instead.
	const settleNavigation = scrollspy._settleNavigation;
	if (typeof settleNavigation !== 'function') {
		return;
	}

	scrollspy._settleNavigation = function (hash, section) {
		const url = new URL(window.location.href);
		url.hash = hash;
		return settleNavigation.call(this, url.href, section);
	};
}
