# HxProgressIndicator

Displays the content of the component as "in progress".

## Parameters

| Name | Type | Description |
|------|------|-------------|
| CssClass | `string` | Additional CSS class to be applied. |
| Delay | `int?` | Debounce delay in milliseconds. The default is `300 ms`. |
| ChildContent | `RenderFragment` | Wrapped content. |
| InProgress | `bool` | Indicates whether the content should be displayed as "in progress". |
| Settings | `ProgressIndicatorSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `ProgressIndicatorSettings` | Application-wide defaults for the `HxProgressIndicator`. |

## Available demo samples

- HxProgressIndicator_Demo.razor

