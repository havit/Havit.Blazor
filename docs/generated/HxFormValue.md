# HxFormValue

Displays a read-only value in the form control visual (as `.form-control`, with label, border, etc.).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| CssClass | `string` | Custom CSS class to render with the wrapping div. |
| Hint | `string` | Hint to render after the input as form-text. |
| HintTemplate | `RenderFragment` | Hint to render after the input as form-text. |
| InputGroupEndTemplate | `RenderFragment` | Input-group at the end of the input. |
| InputGroupEndText | `string` | Input-group-text at the end of the input. In comparison to `IFormValueComponentWithInputGroups.InputGroupEndTemplate`, this property is rendered as `.input-group-text`. |
| InputGroupStartTemplate | `RenderFragment` | Input-group at the beginning of the input. |
| InputGroupStartText | `string` | Input-group-text at the beginning of the input. In comparison to `IFormValueComponentWithInputGroups.InputGroupStartTemplate`, this property is rendered as `.input-group-text`. |
| InputSize | `InputSize?` | Size of the input. |
| Label | `string` | Label to render before the input (or after the input for Checkbox). |
| LabelCssClass | `string` | Custom CSS class to render with the label. |
| LabelTemplate | `RenderFragment` | Label to render before the input (or after the input for Checkbox). |
| Settings | `FormValueSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Value | `string` | Value to be presented. |
| ValueCssClass | `string` | Custom CSS class to render with the value. |
| ValueTemplate | `RenderFragment` | Template to render the value. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| RenderValue(RenderTreeBuilder builder) | `void` | Renders the content of the component (value, input). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `FormValueSettings` | Application-wide defaults for `HxFormValue` and derived components. |

## Available demo samples

- HxFormValue_Demo.razor
- HxFormValue_Demo_CustomContent.razor
- HxFormValue_Demo_InputGroups.razor
- HxFormValue_Demo_Sizing.razor

