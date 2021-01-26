export function open(selector) {
    var el = document.querySelector(selector);
    el.setAttribute("data-bs-toggle", "dropdown");
    new bootstrap.Dropdown(el).show();
}

export function destroy(selector) {
    var el = document.querySelector(selector);
    el.removeAttribute("data-bs-toggle", "dropdown");
    var dd = bootstrap.Dropdown.getInstance(el);
    if (dd) {
        dd.hide();
        dd.dispose();
    }
}
