// Keyboard navigation in the HxTabPanel tab list (WAI-ARIA tabs pattern, mirrors the Bootstrap Tab plugin):
// ArrowLeft/ArrowUp = previous tab, ArrowRight/ArrowDown = next tab (wrapping around), Home/End = first/last tab, disabled tabs are skipped.
// The target tab gets the focus and is clicked (the click activates the tab in Blazor and moves the roving tabindex).
// A single delegated listener (registered once, when the module is loaded) serves all the tab panels on the page.
const previousKeys = ['ArrowLeft', 'ArrowUp'];
const nextKeys = ['ArrowRight', 'ArrowDown'];

function isEnabled(tab) {
	return tab.getAttribute('aria-disabled') !== 'true';
}

function getTargetTab(tabs, currentTab, key) {
	if (key === 'Home') {
		return tabs.find(isEnabled);
	}
	if (key === 'End') {
		return tabs.findLast(isEnabled);
	}

	const step = nextKeys.includes(key) ? 1 : -1;
	const currentIndex = tabs.indexOf(currentTab);
	for (let i = 1; i <= tabs.length; i++) {
		const tab = tabs[(currentIndex + (step * i) + tabs.length) % tabs.length];
		if (isEnabled(tab)) {
			return tab;
		}
	}
	return null;
}

function handleKeyDown(event) {
	const key = event.key;
	if (event.defaultPrevented || event.altKey || event.ctrlKey || event.metaKey || event.shiftKey
		|| !(previousKeys.includes(key) || nextKeys.includes(key) || (key === 'Home') || (key === 'End'))) {
		return;
	}

	const currentTab = event.target.closest?.('[role="tab"]');
	const tabList = currentTab?.parentElement;
	if (!tabList?.matches('.hx-tab-panel[role="tablist"]')) {
		return;
	}

	const tabs = Array.from(tabList.children).filter(element => element.getAttribute('role') === 'tab');
	const targetTab = getTargetTab(tabs, currentTab, key);

	event.preventDefault(); // do not scroll the page
	if (targetTab && (targetTab !== currentTab)) {
		targetTab.focus();
		targetTab.click();
	}
}

document.addEventListener('keydown', handleKeyDown);
