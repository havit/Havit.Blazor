export function isElementActive(id) {
    let element = document.getElementById(id);

    if (element) {
        return element.classList.contains('active');
    }
    
    return false;
}

export function expand(id, navLinkId) {
    let navLink = document.getElementById(navLinkId);

    if (navLink) {
        navLink.ariaExpanded = true;
    }

    let element = document.getElementById(id);

    if (element) {
        element.classList.add('show');
    }
}