export function show(element, hxDrawerDotnetObjectReference, closeOnEscape, scrollingEnabled, subscribeToHideEvent) {
	if (window.drawerElement) {
		const previousDrawer = bootstrap.Drawer.getInstance(window.drawerElement);
		if (previousDrawer) {
            // Although opening a new drawer should close the previous one,
			// we do not set previousDrawer.hidePreventionDisabled = true and force the hide() here (when handling the OnHiding event)
			// We want the developer to handle such specific scenarios on their own.
			// This might be subject to change in the future.
            previousDrawer.hide();
		}
	}

	if (!element) {
		return;
	}

	// Remove old listeners to prevent duplicates when show() is called multiple times
	element.removeEventListener('hide.bs.drawer', handleDrawerHide);
	element.removeEventListener('hidden.bs.drawer', handleDrawerHidden);
	element.removeEventListener('shown.bs.drawer', handleDrawerShown);

	element.hxDrawerDotnetObjectReference = hxDrawerDotnetObjectReference;
	if (subscribeToHideEvent) {
		element.addEventListener('hide.bs.drawer', handleDrawerHide);
	}
	// Backdrop-click close fallback: clicks on the native ::backdrop dispatch on the <dialog> element
	// itself. The Drawer plugin attaches an equivalent listener, but it has proven unreliable across
	// browsers in E2E runs; this explicit listener is idempotent with it (hide() no-ops when already
	// hidden/hiding). Static backdrops are respected (the plugin plays its own "static" bounce).
	element.removeEventListener('click', handleDrawerElementClick);
	element.addEventListener('click', handleDrawerElementClick);
	element.addEventListener('hidden.bs.drawer', handleDrawerHidden);
	element.addEventListener('shown.bs.drawer', handleDrawerShown);
	window.drawerElement = element;

	const drawer = bootstrap.Drawer.getOrCreateInstance(element, {
		keyboard: closeOnEscape,
		scroll: scrollingEnabled
	});
	if (drawer) {
		drawer.show();
	}
}

export function hide(element) {
	if (!element) {
		return;
	}
	element.hxDrawerHiding = true;
	const o = bootstrap.Drawer.getInstance(element);
	if (o) {
		o.hide();
	}
}

function handleDrawerElementClick(event) {
	if (event.target !== event.currentTarget) {
		return; // click inside the drawer panel content, not on the backdrop
	}
	const drawer = bootstrap.Drawer.getInstance(event.currentTarget);
	if (!drawer || drawer._config?.backdrop === 'static') {
		return;
	}
	drawer.hide();
}

function handleDrawerShown(event) {
	event.target.hxDrawerDotnetObjectReference.invokeMethodAsync('HxDrawer_HandleDrawerShown');
}

async function handleDrawerHide(event) {
	const o = bootstrap.Drawer.getInstance(event.target);

	if (o.hidePreventionDisabled || event.target.hxDrawerDisposing) {
		o.hidePreventionDisabled = false;
		return;
	}

	event.preventDefault();

	const cancel = await event.target.hxDrawerDotnetObjectReference.invokeMethodAsync('HxDrawer_HandleDrawerHide');
	if (!cancel) {
		o.hidePreventionDisabled = true;
		event.target.hxDrawerHiding = true;
		o.hide();
	}
};

function handleDrawerHidden(event) {
	event.target.hxDrawerHiding = false;

	if (event.target === window.drawerElement) {
		window.drawerElement = null;
	}

	if (event.target.hxDrawerDisposing) {
		// fix for #110 where the dispose() gets called while the drawer is still in hiding-transition
		dispose(event.target, false);
		return;
	}

	event.target.hxDrawerDotnetObjectReference.invokeMethodAsync('HxDrawer_HandleDrawerHidden');
}

export function dispose(element, opened) {
	if (!element) {
		return;
	}

	element.hxDrawerDisposing = true;

	if (element.hxDrawerHiding) {
		// fix for #110 where the dispose() gets called while the drawer is still in hiding-transition
		return;
	}

	if (opened) {
		// #110 Scrolling not working when drawer is removed (even if disposed is called)
		// Compensates https://github.com/twbs/bootstrap/issues/36397,
		// where the o.dispose() does not reset the ScrollBarHelper() and the scrolling remains deactivated.
		// The dispose() is re-called from hidden.bs.drawer event handler.
		// Remove when the issue is fixed.
		hide(element);
		return;
	}

	element.removeEventListener('hidden.bs.drawer', handleDrawerHidden);
	element.removeEventListener('shown.bs.drawer', handleDrawerShown);
	element.hxDrawerDotnetObjectReference = null;

	const o = bootstrap.Drawer.getInstance(element);
	if (o) {
		o.dispose();
	}

	if (element === window.drawerElement) { // another drawer might be already open
		window.drawerElement = null;
	}
}