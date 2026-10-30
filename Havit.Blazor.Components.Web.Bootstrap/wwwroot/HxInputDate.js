// Used by HxInputDate and HxInputDateRange.
// Registered from Havit.Blazor.Components.Web.Bootstrap.lib.module.js.
export function register() {
	// Capture phase to stop the propagation before any other handler (as the original element-level listener did).
	document.addEventListener('click', handleClick, { capture: true });
}

function handleClick(event) {
	const iconWrapperElement = event.target.closest?.('[data-hx-input-date-icon]');
	if (!iconWrapperElement) {
		return;
	}

	// the icon wrapper is a sibling of the dropdown toggle (input) within HxDropdown
	const triggerElement = iconWrapperElement.parentElement.querySelector(':scope > [data-bs-toggle="dropdown"]');
	if (triggerElement) {
		event.stopPropagation();
		triggerElement.click();
	}
}
