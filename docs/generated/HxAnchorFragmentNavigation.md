# HxAnchorFragmentNavigation

Temporarily (?) compensates for a Blazor deficiency in anchor fragments (scrolling to `page#id` URLs). Supports navigation with the `HxScrollspy` component. GitHub Issue: Blazor 0.8.0: hash routing to named element #8393.

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

