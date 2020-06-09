window.hxBootstrapAutosuggest_openDropdown = (selector) => {
	$(selector).attr("data-toggle", "dropdown");
	$(selector).dropdown("show");
};

window.hxBootstrapAutosuggest_destroyDropdown = (selector) => {
	$(selector).removeAttr("data-toggle", "dropdown");
	$(selector).dropdown("hide");
	$(selector).dropdown("dispose");
};