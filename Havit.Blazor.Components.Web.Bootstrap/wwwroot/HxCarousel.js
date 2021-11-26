export function Initialize(id, hxCarouselDotnetObjectReference, ride) {
    if (hxCarouselDotnetObjectReference == null) {
        return;
    }

    var carouselElement = document.getElementById(id);

    if (carouselElement == null) {
        return;
    }

	var carousel = new bootstrap.Carousel(carouselElement,
		{
			"ride": ride
		});

	carouselElement.addEventListener('slide.bs.carousel', function () { OnSlide(hxCarouselDotnetObjectReference) });
	carouselElement.addEventListener('slid.bs.carousel', function () { OnSlid(hxCarouselDotnetObjectReference) });

	if (ride === "carousel") {
		carousel.cycle();
	}
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
    if (id == null) {
        return;
    }

    var carousel = GetCarousel(id);
    if (carousel != null) {
        carousel.dispose();
    }
}

export function GetCarousel(id) {
    var carouselElement = document.getElementById(id);
    return bootstrap.Carousel.getInstance(carouselElement)
}

function OnSlide(HxCarouselDotnetObjectReference) {
    HxCarouselDotnetObjectReference.invokeMethodAsync('HxCarousel_HandleSlide');
}

function OnSlid(HxCarouselDotnetObjectReference) {
    HxCarouselDotnetObjectReference.invokeMethodAsync('HxCarousel_HandleSlid');
}