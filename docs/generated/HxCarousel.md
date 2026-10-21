# HxCarousel

A slideshow component for cycling through elements—images or slides of text—like a carousel.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `IReadOnlyDictionary<string, object>` | Additional attributes to be splatted onto the root `.carousel` element (e.g. `data-bs-theme="dark"` for a dark carousel). |
| Autoplay | `bool` | Automatically cycle the carousel on load. Default is `false`. Autoplay pauses on hover (see `Pause`) and stops for good once the visitor takes control. |
| Center | `bool` | Snap the active slide to the center of the viewport instead of its start (`.carousel-center`). Pairs nicely with `ItemsPeek`. |
| ChildContent | `RenderFragment` | Content of the carousel. |
| Controls | `bool` | Display previous/next controls to switch between slides. |
| Crossfade | `bool` | Animate slides with a fade transition instead of sliding. |
| CssClass | `string` | Carousel CSS class. Added to root div `.carousel`. |
| Ends | `CarouselEnds` | What happens at the first and last slide. Default is `CarouselEnds.Loop`. |
| Indicators | `bool` | Display indicators showing which slide is active. Can also be used to switch between slides. |
| Interval | `int?` | Delay (ms) between automatically cycling to the next item while `Autoplay` is enabled. When `null` (default), Bootstrap's default of `5000 ms` is used. Can be overridden per item via `HxCarouselItem.Interval`. |
| ItemsGap | `string` | Space between slides as a CSS length (`--bs-carousel-items-gap`), e.g. `"1rem"`. |
| ItemsPeek | `string` | How much of the neighboring slides to reveal, as a CSS length (`--bs-carousel-items-peek`), e.g. `"3rem"`. |
| Keyboard | `bool` | Whether the carousel reacts to keyboard events. Default is `true`. |
| Overlay | `bool` | Overlay the controls and indicators on top of the slides (`.carousel-overlay`) instead of rendering them below. Default is `false`. |
| Pause | `CarouselPause` | Carousel pause behavior. Default is `CarouselPause.Hover` (autoplay stops sliding on hover). |
| PlayPauseControl | `bool` | Display a play/pause button that toggles autoplay. Most useful together with `Autoplay`. |
| VariableWidth | `bool` | Let each `HxCarouselItem` size itself (`.carousel-auto`) for variable-width slides. Set the width of every item via its `HxCarouselItem.CssClass` or inline styles. |
| VisibleItems | `int?` | Number of whole slides visible per view (`--bs-carousel-items`). Default (`null`) shows one. |

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

- HxCarousel_Demo_Autoplay.razor
- HxCarousel_Demo_Controls.razor
- HxCarousel_Demo_Crossfade.razor
- HxCarousel_Demo_CustomContent.razor
- HxCarousel_Demo_Dark.razor
- HxCarousel_Demo_EndsStop.razor
- HxCarousel_Demo_EndsWrap.razor
- HxCarousel_Demo_Events.razor
- HxCarousel_Demo_Indicators.razor
- HxCarousel_Demo_Intervals.razor
- HxCarousel_Demo_MultipleItems.razor
- HxCarousel_Demo_Overlay.razor
- HxCarousel_Demo_Pause.razor
- HxCarousel_Demo_Peeking.razor
- HxCarousel_Demo_ProgrammaticControl.razor
- HxCarousel_Demo_SlidesOnly.razor
- HxCarousel_Demo_VariableWidth.razor

