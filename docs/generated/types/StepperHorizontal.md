# StepperHorizontal

Responsive horizontal setting (breakpoint) for `HxStepper`. The responsive breakpoints are container query based (they react to the width of the containing element, not the viewport). Default is `StepperHorizontal.Never`.

## Enum values

| Name | Value | Description |
|------|-------|-------------|
| Never | 0 | Never horizontal, always vertical. |
| SmallUp | 1 | Horizontal for containers above the "small" breakpoint (`576px`). |
| MediumUp | 2 | Horizontal for containers above the "medium" breakpoint (`768px`). |
| LargeUp | 3 | Horizontal for containers above the "large" breakpoint (`1024px`). |
| ExtraLargeUp | 4 | Horizontal for containers above the "extra-large" breakpoint (`1280px`). |
| XxlUp | 5 | Horizontal for containers above the "2xl" breakpoint (`1536px`). |
| Always | 6 | Always horizontal, never vertical. |

