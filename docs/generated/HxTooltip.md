# HxTooltip

Bootstrap Tooltip component, activates on hover. Rendered as a `span` wrapper to fully support tooltips on disabled elements (see example in Disabled elements in the Bootstrap tooltip documentation).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Animation | `bool?` | Apply a CSS fade transition to the tooltip (enable/disable). Default is `true`. |
| Container | `string` | Appends the tooltip/popover to a specific element. Default is `body`. |
| CssClass | `string` | Custom CSS class to add. |
| Html | `bool` | Allows you to insert HTML. If `false`, `innerText` property will be used to insert content into the DOM. Use text if you're worried about XSS attacks. |
| ChildContent | `RenderFragment` | Child content to wrap. |
| Offset | `ValueTuple<int, int>?` | Offset of the component relative to its target (ChildContent). |
| Placement | `TooltipPlacement?` | Tooltip placement. The default is "not set" (which Bootstrap defaults to `top`). |
| Sanitize | `bool` | Enable or disable the sanitization. If activated, HTML content will be sanitized. See the sanitizer section in Bootstrap JavaScript documentation. Default is `true`. |
| Settings | `TooltipSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Text | `string` | Tooltip text. |
| Trigger | `TooltipTrigger?` | Tooltip trigger(s). The default is "not set" (which Bootstrap defaults to `hover` + `focus`). |
| WrapperCssClass | `string` | Custom CSS class to render with the `span` wrapper of the child-content. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnHidden | `EventCallback` | Fired when the content has finished being hidden from the user and CSS transitions have completed. |
| OnShown | `EventCallback` | Fired when the content has been made visible to the user and CSS transitions have completed. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| DisableAsync() | `Task` | Removes the ability for the component to be shown. It will only be able to be shown if it is re-enabled. |
| EnableAsync() | `Task` | Gives the component the ability to be shown. The component is enabled by default (i.e. this method is not necessary to be called if the component has not been disabled). |
| HideAsync() | `Task` | Hides the component. |
| ShowAsync() | `Task` | Shows the component. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `TooltipSettings` | Application-wide defaults for the `HxTooltip` and derived components. |

## Available demo samples

- HxTooltip_Demo_BasicUsage.razor
- HxTooltip_Demo_Button.razor
- HxTooltip_Demo_EnableDisable.razor
- HxTooltip_Demo_HtmlContent.razor
- HxTooltip_Demo_MethodsEvents.razor
- HxTooltip_Demo_Placement.razor

