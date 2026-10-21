# HxAnchorFragmentNavigation

Enables navigation to in-page anchors (`page#id` URLs) by scrolling to the target element — Blazor routing does not do this on its own (tracking issue). Pairs with `HxScrollspy`.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Automation | `AnchorFragmentNavigationAutomationMode` | The level of automation. The default is `AnchorFragmentNavigationAutomationMode.Full`. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| ScrollToAnchorAsync(string anchor) | `Task` |  |
| ScrollToCurrentUriFragmentAsync() | `Task` |  |

## Available demo samples

- HxAnchorFragmentNavigation_Demo.razor

