# HxStepper

Bootstrap Stepper component (new in Bootstrap 6). Displays progress through a multi-step workflow (wizards, timelines, step-by-step progress bars) as a sequence of numbered steps (`HxStepperItem`s). The component is CSS-only, there is no step-switching logic (wire up your own state).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying HTML element (e.g. use the `style` attribute to customize the gap by overriding the `--bs-stepper-gap` CSS variable). |
| ChildContent | `RenderFragment` | Content of the stepper (`HxStepperItem`s). |
| Color | `ThemeColor?` | Theme color of the stepper (renders the `theme-*` class), applies to all active items. To color individual steps, use `HxStepperItem.Color`. |
| CssClass | `string` | Additional CSS class(es) for the stepper. |
| Horizontal | `StepperHorizontal?` | Changes the layout of the stepper items from vertical to horizontal, including the responsive breakpoint (Bootstrap 6 responsive `{breakpoint}:stepper-horizontal` classes are container query based, the component renders the required `contains-inline` wrapper automatically). The default is `StepperHorizontal.Never` (vertical stepper). |
| Overflow | `bool?` | When `true`, the stepper is wrapped in a `stepper-overflow` container which enables horizontal scrolling when the (horizontal) stepper overflows its parent. The default is `false`. |
| Settings | `StepperSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `StepperSettings` | Application-wide defaults for `HxStepper` and derived components. |

## Available demo samples

- HxStepper_Demo.razor
- HxStepper_Demo_Alignment.razor
- HxStepper_Demo_Anchors.razor
- HxStepper_Demo_Badges.razor
- HxStepper_Demo_Colors.razor
- HxStepper_Demo_ComplexContent.razor
- HxStepper_Demo_Gap.razor
- HxStepper_Demo_Horizontal.razor
- HxStepper_Demo_Overflow.razor
- HxStepper_Demo_Responsive.razor
- HxStepper_Demo_States.razor

