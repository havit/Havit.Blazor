# HxToast

Bootstrap Toast component. Not intended to be used in user code, use `HxMessenger`. After the first render, the component never updates.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AutohideDelay | `int?` | Delay in milliseconds to automatically hide the toast. |
| Color | `ThemeColor?` | Color scheme. |
| ContentTemplate | `RenderFragment` | Content (body) template. |
| ContentText | `string` | Content (body) text. |
| CssClass | `string` | CSS class to render with the toast. |
| HeaderIcon | `IconBase` | Header icon. |
| HeaderTemplate | `RenderFragment` | Header template. |
| HeaderText | `string` | Header text. |
| ShowCloseButton | `bool` | Indicates whether to show the close button. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnToastHidden | `EventCallback` | Fires when the toast is hidden (button or autohide). |

## Available demo samples

- HxToast_Demo.razor

