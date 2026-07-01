# HxDialog

Component for rendering a dialog as a Bootstrap 6 Dialog (built on the native `<dialog>` element). Visit Bootstrap Dialog for more information.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Animation | `DialogAnimation?` | Open/close animation of the dialog. Default is `DialogAnimation.Fade` (Bootstrap default). |
| Backdrop | `DialogBackdrop?` | Indicates whether to apply a backdrop on body while the dialog is open. If set to `DialogBackdrop.Static`, the dialog cannot be closed by clicking on the backdrop. Default value (from `Defaults`) is `DialogBackdrop.True`. |
| BodyCssClass | `string` | Additional body CSS class (`div.dialog-body`). |
| BodyTemplate | `RenderFragment` | Body template. |
| CloseButtonIcon | `IconBase` | Close icon to be used in header. If set to `null`, Bootstrap default close-button will be used. |
| CloseOnEscape | `bool?` | Indicates whether the dialog closes when escape key is pressed. Default value is `true`. |
| CssClass | `string` | Additional CSS class for the main element (`div.dialog`). |
| FooterCssClass | `string` | Additional footer CSS class (`div.dialog-footer`). |
| FooterTemplate | `RenderFragment` | Footer template. |
| Fullscreen | `DialogFullscreen?` | Fullscreen behavior of the dialog. Default is `DialogFullscreen.Disabled`. |
| HeaderCssClass | `string` | Additional header CSS class (`div.dialog-header`). |
| HeaderTemplate | `RenderFragment` | Header template. |
| NonModal | `bool?` | When `true`, the dialog opens as non-modal (browser-native `show()` instead of `showModal()`): no backdrop, no focus trap, no blocking of the rest of the page. Default is `false`. |
| RenderMode | `DialogRenderMode` | Determines whether the content is always rendered or only if the dialog is open. |
| Scrollable | `bool?` | Allows scrolling the dialog body. Default is `false`. |
| Settings | `DialogSettings` | A set of settings applied to this component instance. Overrides `Defaults` and is itself overridden by individual parameters. |
| ShowCloseButton | `bool?` | Indicates whether the dialog shows close button in header. Default value is `true`. |
| Size | `DialogSize?` | Size of the dialog. Default is `DialogSize.Regular`. |
| Title | `string` | Title in dialog header. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnClosed | `EventCallback` | Fired when the dialog has finished hiding from the user, after CSS transitions complete. Triggered by `HxDialog.HideAsync`, the close button, the Esc key, or other interactions. |
| OnHiding | `EventCallbackDialogHidingEventArgs>` | Fired immediately when the 'hide' instance method is called. This can be triggered by `HxDialog.HideAsync`, the close button, the Esc key, or other interactions. To cancel hiding, set `DialogHidingEventArgs.Cancel` to `true`. |
| OnShown | `EventCallback` | Fired when a dialog element becomes visible to the user, after CSS transitions complete. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| HideAsync() | `Task` | Closes the dialog. |
| ShowAsync() | `Task` | Opens the dialog. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `DialogSettings` | Application-wide default settings for the `HxDialog`. |

## Available demo samples

- HxDialog_Demo.razor
- HxDialog_Demo_Animation.razor
- HxDialog_Demo_Backdrop.razor
- HxDialog_Demo_Footer.razor
- HxDialog_Demo_Fullscreen.razor
- HxDialog_Demo_NonModal.razor
- HxDialog_Demo_Scrollable.razor
- HxDialog_Demo_Scrolling.razor
- HxDialog_Demo_Size.razor
- HxDialog_Demo_Swapping.razor

