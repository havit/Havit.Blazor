# HxMessenger

`HxToastContainer` wrapper for displaying `HxToast` messages dispatched through `IHxMessengerService`.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto the underlying `div.toast-container` element (e.g. `data-bs-theme="dark"` to render all the messages in dark mode). To render a single message in dark mode, use `MessengerMessage.AdditionalAttributes`. |
| CssClass | `string` | Additional CSS class. |
| Position | `ToastContainerPosition` | Position of the messages. The default value is `ToastContainerPosition.None`. |

## Available demo samples

- HxMessenger_Demo.razor
- HxMessenger_Demo_Dark.razor

