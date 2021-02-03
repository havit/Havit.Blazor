export function open(inputElement) {
    inputElement.setAttribute("data-bs-toggle", "dropdown");
    new bootstrap.Dropdown(inputElement).show();
}

export function destroy(inputElement) {
    inputElement.removeAttribute("data-bs-toggle", "dropdown");
    var dd = bootstrap.Dropdown.getInstance(inputElement);
    if (dd) {
        dd.hide();
        dd.dispose();
    }
}
