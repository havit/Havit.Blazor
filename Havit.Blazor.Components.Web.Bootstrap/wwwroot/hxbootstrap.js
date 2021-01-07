window.hxBootstrapAutosuggest_openDropdown = (selector) => {
    var el = document.querySelector(selector);
    el.setAttribute("data-bs-toggle", "dropdown");
    new bootstrap.Dropdown(el).show();
};
window.hxBootstrapAutosuggest_destroyDropdown = (selector) => {
    var el = document.querySelector(selector);
    el.removeAttribute("data-bs-toggle", "dropdown");
    var dd = bootstrap.Dropdown.getInstance(el);
    if (dd) {
        dd.hide();
        dd.dispose();
    }
};

window.hxGrid_cellClick = (event) => {
    if ((event.target.nodeName === 'A')
        || (event.target.nodeName === 'INPUT')
        || (event.target.nodeName === 'BUTTON')
        || (event.target.nodeName === 'SUBMIT')
        || event.target.hasAttribute('href')) {
        event.stopPropagation();
    }
}

window.hxModal_show = (element, hxModalDotnetObjectReference, useStaticBackdrop, closeOnEscape) => {
    element.hxModalDotnetObjectReference = hxModalDotnetObjectReference;
    element.addEventListener('hidden.bs.modal', window.hxModal_handleModalHiddenInDotnet)

    var modal = new bootstrap.Modal(element, {
        backdrop: useStaticBackdrop ? "static" : false,
        keyboard: closeOnEscape
    });
    modal.show();
}

window.hxModal_handleModalHiddenInDotnet = (event) => {
    event.target.removeEventListener('hidden.bs.modal', window.hxModal_handleModalHiddenInDotnet);
    event.target.hxModalDotnetObjectReference.invokeMethodAsync('HxModal_HandleModalHidden');
    event.target.hxModalDotnetObjectReference = null;
    hxModal_hide(event.target);
};

window.hxModal_hide = (element) =>
{
    var modal = bootstrap.Modal.getInstance(element);    
    modal.hide();
    modal.dispose();
}

var handleToastHiddenInDotnet = (event) => {
	event.target.hxToastDotnetObjectReference.invokeMethodAsync('HxToast_HandleToastHidden');
}; 
window.hxToast_show = (element, hxToastDotnetObjectReference) => {
	element.hxToastDotnetObjectReference = hxToastDotnetObjectReference;
	element.addEventListener('hidden.bs.toast', handleToastHiddenInDotnet);
    new bootstrap.Toast(element).show();
}
window.hxToast_dispose = (element) => {
	element.removeEventListener('hidden.bs.toast', handleToastHiddenInDotnet);
}