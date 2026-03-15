# HxMessageBox

Component for displaying message boxes. Usually used via `HxMessageBoxService` and `HxMessageBoxHost`.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying `HxModal`. |
| BodyTemplate | `RenderFragment` | Body (content) template. |
| Buttons | `MessageBoxButtons` | Buttons to show. The default is `MessageBoxButtons.Ok`. |
| CustomButtonText | `string` | Text for `MessageBoxButtons.Custom`. |
| HeaderTemplate | `RenderFragment` | Header template (Header). |
| ModalSettings | `ModalSettings` | Settings for the underlying `HxModal` component. |
| PrimaryButton | `MessageBoxButtons?` | Primary button (if you want to override the default). |
| PrimaryButtonSettings | `ButtonSettings` | Settings for the dialog primary button. |
| SecondaryButtonSettings | `ButtonSettings` | Settings for dialog secondary button(s). |
| Settings | `MessageBoxSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| ShowCloseButton | `bool?` | Indicates whether to show the close button. The default is taken from the underlying `HxModal` (`true`). |
| Text | `string` | Content (body) text. |
| Title | `string` | Title text (Header). |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnClosed | `EventCallbackMessageBoxButtons>` | Raised when the message box gets closed. Returns the button clicked. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| ShowAsync() | `Task` | Displays the message box. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `MessageBoxSettings` | Application-wide defaults for `HxButton` and derived components. |

