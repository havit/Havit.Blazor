# HxAlert

Bootstrap alert component.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Color **[REQUIRED]** | `ThemeColor` | Alert color (background). Required. |
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying HTML element. |
| CssClass | `string` | Any additional CSS class to apply. |
| Dismissible | `bool` | Shows the Close button and allows dismissing the alert. |
| ChildContent | `RenderFragment` | Content of the component. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| GetColorCss() | `string` |  |

## Available demo samples

- HxAlert_Demo_AdditionalContent.razor
- HxAlert_Demo_Basic.razor
- HxAlert_Demo_Dismissible.razor
- HxAlert_Demo_Icons.razor

