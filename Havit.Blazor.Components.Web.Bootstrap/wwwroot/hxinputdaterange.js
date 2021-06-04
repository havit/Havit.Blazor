export function open(triggerElement) {
    new bootstrap.Dropdown(triggerElement).show();
}
    
export function destroy(triggerElement) {
    var dropdown = bootstrap.Dropdown.getInstance(triggerElement);
    if (dropdown) {
        dropdown.hide();
        dropdown.dispose();
    }
}
