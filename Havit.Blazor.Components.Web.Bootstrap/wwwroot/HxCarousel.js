export function initialize(element, hxCarouselDotnetObjectReference, options) {
	if (!element) {
		return;
	}
	const carousel = new bootstrap.Carousel(element, options);

	element.hxCarouselDotnetObjectReference = hxCarouselDotnetObjectReference;
	element.addEventListener('slide.bs.carousel', handleSlide);
	element.addEventListener('slid.bs.carousel', handleSlid);
}
export function slideTo(element, index) {
	const c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.to(index);
	}
}

export function previous(element) {
	const c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.prev();
	}
}

export function next(element) {
	const c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.next();
	}
}

export function cycle(element) {
	const c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.cycle();
	}
}

export function pause(element) {
	const c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.pause();
	}
}

function handleSlide(event) {
	event.target.hxCarouselDotnetObjectReference.invokeMethodAsync('HxCarousel_HandleSlide', event.from, event.to, event.direction);
}

function handleSlid(event) {
	event.target.hxCarouselDotnetObjectReference.invokeMethodAsync('HxCarousel_HandleSlid', event.from, event.to, event.direction);
}

export function dispose(element) {
	if (!element) {
		return;
	}

	element.removeEventListener('slide.bs.carousel', handleSlide);
	element.removeEventListener('slid.bs.carousel', handleSlid);
	element.hxCarouselDotnetObjectReference = null;

	const c = bootstrap.Carousel.getInstance(element);
	if (c) {
		c.dispose();
	}
}