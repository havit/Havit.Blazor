window.hxBootstrapAutosuggest_openDropdown = (selector) => {
	$(selector).attr("data-toggle", "dropdown");
	$(selector).dropdown("show");
};

window.hxBootstrapAutosuggest_destroyDropdown = (selector) => {
	$(selector).removeAttr("data-toggle", "dropdown");
	$(selector).dropdown("hide");
	$(selector).dropdown("dispose");
};

window.hxGrid_cellClick = (event) => {
	if ((event.target.nodeName === 'A')
		|| (event.target.nodeName === 'INPUT')
		|| (event.target.nodeName === 'BUTTON')
		|| (event.target.nodeName === 'SUBMIT')
		|| event.target.hasAttribute('href'))
	{
		event.stopPropagation();
	}
}

window.hxToast_show = (element) => {
	new bootstrap.Toast(element).show();
}