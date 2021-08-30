export function show(element, hxModalDotnetObjectReference, useStaticBackdrop, closeOnEscape) {
    element.hxModalDotnetObjectReference = hxModalDotnetObjectReference;
    element.addEventListener('hidden.bs.modal', handleModalHidden)

    var modal = new bootstrap.Modal(element, {
        backdrop: useStaticBackdrop ? "static" : true,
        keyboard: closeOnEscape
    });

    modal.show();
}

export function hide(element) {
    var modal = bootstrap.Modal.getInstance(element);
    modal.hide();
}

export function dispose(element) {
    var modal = bootstrap.Modal.getInstance(element);
    element.removeEventListener('hidden.bs.modal', handleModalHidden);
    element.hxModalDotnetObjectReference = null;
    modal.hide();
    //modal.dispose(); // modal.js:318 Uncaught TypeError: Cannot read property 'style' of null
}

function handleModalHidden(event) {
    event.target.removeEventListener('hidden.bs.modal', handleModalHidden);
    event.target.hxModalDotnetObjectReference.invokeMethodAsync('HxModal_HandleModalHidden');
    event.target.hxModalDotnetObjectReference = null;

    var modal = bootstrap.Modal.getInstance(event.target);    
    modal.dispose();
};
