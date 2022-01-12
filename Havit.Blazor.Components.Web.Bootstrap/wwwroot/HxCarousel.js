export function initialize(element, hxCarouselDotnetObjectReference, ride, pause) {
	if (!element) {
		return;
	}
	var carousel = new bootstrap.Carousel(element,
		{
			"ride": ride,
			"pause": pause
		});

	element.hxCarouselDotnetObjectReference = hxCarouselDotnetObjectReference;
	element.addEventListener('slide.bs.carousel', handleSlide);
	element.addEventListener('slid.bs.carousel', handleSlid);

	if (ride === "carousel") {
		carousel.cycle();
	}
}
export function slideTo(element, index) {
	var c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.to(index);
	}
}

export function previous(element) {
	var c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.prev();
	}
}

export function next(element) {
	var c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.next();
	}
}

export function cycle(element) {
	var c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.cycle();
	}
}

export function pause(element) {
	var c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.pause();
	}
}

function handleSlide(event) {
	event.target.hxCarouselDotnetObjectReference.invokeMethodAsync('HxCarousel_HandleSlide');
}

function handleSlid(event) {
	event.target.hxCarouselDotnetObjectReference.invokeMethodAsync('HxCarousel_HandleSlid');
}

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('slide.bs.carousel', handleSlide);
	element.removeEventListener('slid.bs.carousel', handleSlid);
	element.hxCarouselDotnetObjectReference = null;

	var c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.dispose();
	}
}