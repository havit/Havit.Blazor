﻿export function initialize(element, hxCollapseDotnetObjectReference) {
	if (!element) {
		return;
	}

	element.hxCollapseDotnetObjectReference = hxCollapseDotnetObjectReference;
	element.addEventListener('show.bs.collapse', handleCollapseShow);
	element.addEventListener('hide.bs.collapse', handleCollapseHide);
	element.addEventListener('shown.bs.collapse', handleCollapseShown);
	element.addEventListener('hidden.bs.collapse', handleCollapseHidden);

	var c = new bootstrap.Collapse(element, {
		toggle: false,
	});
}

export function show(element) {
	var c = bootstrap.Collapse.getInstance(element);
	if (c) {
		c.show();
	}
};

export function hide(element) {
	var c = bootstrap.Collapse.getInstance(element);
	if (c) {
		c.hide();
	}
};

function handleCollapseShow(event) {
	event.target.hxCollapseDotnetObjectReference.invokeMethodAsync('HxCollapse_HandleJsShow');
}

function handleCollapseHide(event) {
	event.target.hxCollapseDotnetObjectReference.invokeMethodAsync('HxCollapse_HandleJsHide');
}

function handleCollapseShown(event) {
	event.target.hxCollapseDotnetObjectReference.invokeMethodAsync('HxCollapse_HandleJsShown');
};

function handleCollapseHidden(event) {
	event.target.hxCollapseDotnetObjectReference.invokeMethodAsync('HxCollapse_HandleJsHidden');
};

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('show.bs.collapse', handleCollapseShow);
	element.removeEventListener('hide.bs.collapse', handleCollapseHide);
	element.removeEventListener('shown.bs.collapse', handleCollapseShown);
	element.removeEventListener('hidden.bs.collapse', handleCollapseHidden);
	element.hxCollapseDotnetObjectReference = null;

	var c = bootstrap.Collapse.getInstance(element);
	if (c) {
		c.dispose();
	}
}
