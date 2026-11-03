# HxToast

Bootstrap Toast component. Not intended to be used in user code, use `HxMessenger`. After the first render, the component never updates.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto the underlying `div.toast` element (e.g. `data-bs-theme="dark"` for a dark toast). |
| AutohideDelay | `int?` | Delay in milliseconds to automatically hide the toast. |
| Color | `ThemeColor?` | Color scheme (Bootstrap 6 `theme-*` variant: colored border and subtle header). `ThemeColor.Danger` and `ThemeColor.Warning` toasts are announced to assistive technologies immediately (`role="alert"`, `aria-live="assertive"`), other toasts politely (`role="status"`, `aria-live="polite"`). |
| ContentTemplate | `RenderFragment` | Content (body) template. |
| ContentText | `string` | Content (body) text. |
| CssClass | `string` | CSS class to render with the toast. |
| HeaderIcon | `IconBase` | Header icon. |
| HeaderTemplate | `RenderFragment` | Header template. |
| HeaderText | `string` | Header text. |
| Instant | `bool` | When `true`, the toast shows and hides without the fade animation (`toast-instant`). Default is `false`. |
| ShowCloseButton | `bool` | Indicates whether to show the close button. |
| Translucent | `bool` | When `true`, enhances the toast translucency with a backdrop blur and saturation filter (`toast-translucent`, new in Bootstrap 6). Default is `false`. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnToastHidden | `EventCallback` | Fires when the toast is hidden (button or autohide). |

## Available demo samples

- HxToast_Demo.razor
- HxToast_Demo_Color.razor
- HxToast_Demo_Dark.razor
- HxToast_Demo_Placement.razor

