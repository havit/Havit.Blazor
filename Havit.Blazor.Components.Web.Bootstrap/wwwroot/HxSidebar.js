// Used by HxSidebarItem - shows the subitems dropdown on hover (relevant for collapsed sidebar).
// Registered from Havit.Blazor.Components.Web.Bootstrap.lib.module.js.
export function register() {
	// mouseenter/mouseleave do not bubble, capture phase delivers them for every element entered/left.
	document.addEventListener('mouseenter', handleMouseEnter, { capture: true });
	document.addEventListener('mouseleave', handleMouseLeave, { capture: true });
}

function handleMouseEnter(event) {
	if (event.target.hasAttribute?.('data-hx-sidebar-item-hover-dropdown')) {
		bootstrap.Dropdown.getOrCreateInstance(event.target).show();
	}
}

function handleMouseLeave(event) {
	if (event.target.hasAttribute?.('data-hx-sidebar-item-hover-dropdown')) {
		bootstrap.Dropdown.getInstance(event.target)?.hide();
	}
}
