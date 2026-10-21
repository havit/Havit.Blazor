export function initialize(element, hxCarouselDotnetObjectReference, options) {
	if (!element) {
		return;
	}
	const carousel = new bootstrap.Carousel(element, options);

	element.hxCarouselDotnetObjectReference = hxCarouselDotnetObjectReference;
	element.addEventListener('slide.bs.carousel', handleSlide);
	element.addEventListener('slid.bs.carousel', handleSlid);
}

// Re-creates the Bootstrap carousel after items were added or removed
// (the instance observes and manages only the items present at construction).
export function reinitialize(element, options) {
	if (!element) {
		return;
	}

	let paused = false;
	const c = bootstrap.Carousel.getInstance(element);
	if (c) {
		// keep the runtime autoplay intent (autoplay stops for good once the user takes control)
		options = { ...options, autoplay: c._playing ?? options.autoplay };
		// ...and whether it is cycling right now (pause() via PauseAsync() or hover keeps the intent, only stops the timer)
		paused = options.autoplay && !element.classList.contains('carousel-playing');
		c.dispose();
	}
	// the active item is preserved, the new instance reads it from the .carousel-item.active element
	const carousel = new bootstrap.Carousel(element, options);
	if (paused) {
		// mouseleave or CycleAsync() resumes cycling as before
		carousel.pause();
	}
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