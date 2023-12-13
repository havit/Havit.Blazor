var dropdowns = new Map();

export function initializePopper(navId, dropdownId) {
	const nav = document.querySelector(`#${navId}`);
	const dropdown = document.querySelector(`#${dropdownId}`);

	// Create popper instance
	const popperInstance = Popper.createPopper(nav, dropdown, {
		placement: 'right-start',
		modifiers: [
			{ name: 'eventListeners', enabled: false }
		]
	});

	dropdowns.set(dropdownId, {
		popperInstance:popperInstance,
		navElement: nav,
		show: show,
		hide: hide
	});

	nav.addEventListener('mouseenter', show);
	nav.addEventListener('focus', show);

	nav.addEventListener('mouseleave', hide);
	nav.addEventListener('blur', hide);

	function show() {
		// Make the dropdown visible
		dropdown.setAttribute('popper-show', '');

		// Enable the event listeners
		popperInstance.setOptions((options) => ({
			...options,
			modifiers: [
				...options.modifiers,
				{ name: 'eventListeners', enabled: true }
			]
		}));

		// Update its position
		popperInstance.update();
	}

	function hide() {
		// Hide the dropdown
		dropdown.removeAttribute('popper-show');

		// Disable the event listeners
		popperInstance.setOptions((options) => ({
			...options,
			modifiers: [
				...options.modifiers,
				{ name: 'eventListeners', enabled: false }
			]
		}));
	}
}

export function destroyPopper(navId, dropdownId) {
	// Destroy the popper instance
	const dropdown = dropdowns.get(dropdownId);
	dropdown.popperInstance.destroy();

	// Remove the event listeners
	dropdown.navElement.removeEventListener('mouseenter', dropdown.show);
	dropdown.navElement.removeEventListener('focus', dropdown.show);

	dropdown.navElement.removeEventListener('mouseleave', dropdown.hide);
	dropdown.navElement.removeEventListener('blur', dropdown.hide);

	dropdowns.delete(dropdownId);
}