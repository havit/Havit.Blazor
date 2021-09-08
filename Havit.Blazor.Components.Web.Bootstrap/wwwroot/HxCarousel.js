export function InitializeCarousel(id) {
    GetCarousel(id).carousel();
}

export function ClickNextButton(id) {
    document.getElementById(id + "next").click();
}

export function SlideTo(id, index) {
    GetCarousel(id).to(index);
}

export function Previous(id) {
    GetCarousel(id).prev();
}

export function Next(id) {
    GetCarousel(id).next();
}

export function Cycle(id) {
    GetCarousel(id).cycle();
}

export function Pause(id) {
    GetCarousel(id).pause();
}

export function Dispose(id) {
    GetCarousel(id).dispose();
}

export function GetCarousel(id) {
    var carouselElement = document.getElementById(id);
    return bootstrap.Carousel.getInstance(carouselElement)
}

export function SetInterval(id, interval) {
    GetCarousel(id).interval = interval;
}

export function AddEventListeners(id, hxCarouselDotnetObjectReference) {
    let carouselElement = document.getElementById(id);

    carouselElement.addEventListener('slide.bs.carousel', function () { OnSlide(hxCarouselDotnetObjectReference) });
    carouselElement.addEventListener('slid.bs.carousel', function () { OnSlid(hxCarouselDotnetObjectReference) });
}

function OnSlide(HxCarouselDotnetObjectReference) {
    HxCarouselDotnetObjectReference.invokeMethodAsync('HxCarousel_HandleSlide');
}

function OnSlid(HxCarouselDotnetObjectReference) {
    HxCarouselDotnetObjectReference.invokeMethodAsync('HxCarousel_HandleSlid');
}