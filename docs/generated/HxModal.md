# HxModal

Component for rendering a modal dialog as a Bootstrap Modal. Visit Bootstrap Modal for more information.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Animated | `bool?` | Determines whether modals appear without fading in. Setting to `false` removes the `.fade` class from the modal markup. Default value is `true`. |
| Backdrop | `ModalBackdrop?` | Indicates whether to apply a backdrop on body while the modal is open. If set to `ModalBackdrop.Static`, the modal cannot be closed by clicking on the backdrop. Default value (from `Defaults`) is `ModalBackdrop.True`. |
| BodyCssClass | `string` | Additional body CSS class (`div.modal-body`). |
| BodyTemplate | `RenderFragment` | Body template. |
| Centered | `bool?` | Allows vertical centering of the modal. Default is `false` (horizontal only). |
| CloseButtonIcon | `IconBase` | Close icon to be used in header. If set to `null`, Bootstrap default close-button will be used. |
| CloseOnEscape | `bool?` | Indicates whether the modal closes when escape key is pressed. Default value is `true`. |
| ContentCssClass | `string` | Additional content CSS class (`div.modal-content`). |
| CssClass | `string` | Additional CSS class for the main element (`div.modal`). |
| DialogCssClass | `string` | Additional CSS class for the dialog (`div.modal-dialog` element). |
| FooterCssClass | `string` | Additional footer CSS class (`div.modal-footer`). |
| FooterTemplate | `RenderFragment` | Footer template. |
| Fullscreen | `ModalFullscreen?` | Fullscreen behavior of the modal. Default is `ModalFullscreen.Disabled`. |
| HeaderCssClass | `string` | Additional header CSS class (`div.modal-header`). |
| HeaderTemplate | `RenderFragment` | Header template. |
| RenderMode | `ModalRenderMode` | Determines whether the content is always rendered or only if the modal is open. |
| Scrollable | `bool?` | Allows scrolling the modal body. Default is `false`. |
| Settings | `ModalSettings` | A set of settings applied to this component instance. Overrides `Defaults` and is itself overridden by individual parameters. |
| ShowCloseButton | `bool?` | Indicates whether the modal shows close button in header. Default value is `true`. |
| Size | `ModalSize?` | Size of the modal. Default is `ModalSize.Regular`. |
| Title | `string` | Title in modal header. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnClosed | `EventCallback` | Fired when the modal has finished hiding from the user, after CSS transitions complete. Triggered by `HxModal.HideAsync`, the close button, the Esc key, or other interactions. |
| OnHiding | `EventCallbackModalHidingEventArgs>` | Fired immediately when the 'hide' instance method is called. This can be triggered by `HxModal.HideAsync`, the close button, the Esc key, or other interactions. To cancel hiding, set `ModalHidingEventArgs.Cancel` to `true`. |
| OnShown | `EventCallback` | Fired when a modal element becomes visible to the user, after CSS transitions complete. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| HideAsync() | `Task` | Closes the modal. |
| ShowAsync() | `Task` | Opens the modal. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `ModalSettings` | Application-wide default settings for the `HxModal`. |

## Available demo samples

- HxModal_Demo.razor
- HxModal_Demo_Backdrop.razor
- HxModal_Demo_Fullscreen.razor
- HxModal_Demo_Scrollable.razor
- HxModal_Demo_Scrolling.razor
- HxModal_Demo_Size.razor
- HxModal_Demo_VerticallyCentered.razor

