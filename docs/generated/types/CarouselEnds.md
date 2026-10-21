# CarouselEnds

Defines what happens at the first and last slide of a `HxCarousel`.

## Enum values

| Name | Value | Description |
|------|-------|-------------|
| Loop | 0 | Scrolls seamlessly past the ends for an endless conveyor effect (default). Applies to single-slide carousels; multi-item, peek, centered, and variable-width layouts (and users who prefer reduced motion) fall back to `CarouselEnds.Wrap`. |
| Wrap | 1 | Jumps from the last slide back to the first (and vice versa), so the controls never reach a dead end. |
| Stop | 2 | Hard-stops at the first and last slide. The previous control is disabled on the first slide and the next control on the last (via the native `disabled` attribute on `<button>` controls). |

