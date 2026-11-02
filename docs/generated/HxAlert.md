# HxAlert

Contextual feedback message (success, warning, danger, info, and more) with an optional icon, links, and a dismiss button. Renders the Bootstrap alert markup.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying HTML element. |
| ChildContent | `RenderFragment` | Content of the alert. The alert is a flex container - wrap mixed inline content (text with links etc.) in a `p` element. |
| Color | `ThemeColor` | Alert color. Default is `ThemeColor.None`, which renders the neutral (default) Bootstrap alert. |
| CssClass | `string` | Any additional CSS class to apply. |
| Dismissible | `bool` | Shows the Close button and allows dismissing the alert. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnClosed | `EventCallback` | Fired when the alert has been dismissed by the Close button (`Dismissible`) and removed from the page (Bootstrap `closed.bs.alert` event). Use the callback to stop rendering the alert and to move focus to an appropriate element (the focused Close button is gone). |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| GetColorCss() | `string` |  |

## Available demo samples

- HxAlert_Demo_AdditionalContent.razor
- HxAlert_Demo_Basic.razor
- HxAlert_Demo_Default.razor
- HxAlert_Demo_Dismissible.razor
- HxAlert_Demo_Icons.razor
- HxAlert_Demo_LinkColor.razor
- HxAlert_Demo_OnClosed.razor

