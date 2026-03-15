# HxCarousel

A slideshow component for cycling through elements—images or slides of text—like a carousel.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Controls | `bool` | Display controls to switch between slides. |
| Crossfade | `bool` | Animate slides with a fade transition instead of sliding. |
| CssClass | `string` | Carousel CSS class. Added to root div `.carousel`. |
| Dark | `bool` | Show controls, captions, etc. with dark colors. |
| ChildContent | `RenderFragment` | Content of the carousel. |
| Indicators | `bool` | Display indicators showing which slide is active. Can also be used to switch between slides. |
| Interval | `int?` | Delay for automatically switching slides. Default is `3000 ms`. |
| Pause | `CarouselPause` | Carousel pause behavior. Default is `CarouselPause.Hover` (carousel will stop sliding on hover). |
| Ride | `CarouselRide` | Carousel ride (autoplay) behavior. Default is `CarouselRide.Carousel` (autoplays the carousel on load). |
| TouchSwiping | `bool` | Enable or disable swiping left/right on touchscreen devices to move between slides. Default is `true` (enabled). |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnSlid | `EventCallbackCarouselSlideEventArgs>` | Is fired when the current slide is changed (once the transition is completed). |
| OnSlide | `EventCallbackCarouselSlideEventArgs>` | Is fired when the current slide is changed (at the very start of the sliding transition). |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| CycleAsync() | `Task` | Start cycling between slides. |
| PauseAsync() | `Task` | Pause cycling. |
| SlideToAsync(int index) | `Task` | Slides to an element with the corresponding index. |
| SlideToNextItemAsync() | `Task` | Slides to the next item (to the right). |
| SlideToPreviousItemAsync() | `Task` | Slides to the previous item (to the left). |

## Available demo samples

- HxCarousel_Demo_Captions.razor
- HxCarousel_Demo_Controls.razor
- HxCarousel_Demo_Crossfade.razor
- HxCarousel_Demo_Dark.razor
- HxCarousel_Demo_Events.razor
- HxCarousel_Demo_Indicators.razor
- HxCarousel_Demo_Intervals.razor
- HxCarousel_Demo_NoAutoSliding.razor
- HxCarousel_Demo_Pause.razor
- HxCarousel_Demo_ProgrammaticControl.razor
- HxCarousel_Demo_SlidesOnly.razor
- HxCarousel_Demo_TouchSwiping.razor

