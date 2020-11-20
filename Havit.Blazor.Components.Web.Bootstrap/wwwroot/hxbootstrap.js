window.hxBootstrapAutosuggest_openDropdown = (selector) => {
    var el = document.querySelector(selector);
    el.setAttribute("data-toggle", "dropdown");
    new bootstrap.Dropdown(el).show();
};

window.hxBootstrapAutosuggest_destroyDropdown = (selector) => {
    var el = document.querySelector(selector);
    el.removeAttribute("data-toggle", "dropdown");
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

window.hxToast_show = (element, hxtoast) => {
    element.addEventListener('hidden.bs.toast', function () {
        hxtoast.invokeMethodAsync('HxToast_HandleToastHidden');
    });
    new bootstrap.Toast(element).show();
}