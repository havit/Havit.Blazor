# HxNavLink

Bootstrap nav-link component.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying `NavLink` component. |
| CssClass | `string` | Additional CSS class. |
| Enabled | `bool?` | When `null` (default), the Enabled value is received from cascading `FormState`. When value is `false`, input is rendered as disabled. To set multiple controls as disabled use `HxFormState`. |
| Href | `string` | Navigation target. |
| ChildContent | `RenderFragment` | Content. |
| Match | `NavLinkMatch?` | URL matching behavior for the underlying `NavLink`. Default is `NavLinkMatch.Prefix`. You can set the value to `null` to disable the matching. |
| OnClickPreventDefault | `bool?` | Prevents the default action for the onclick event. Default is `null`, which means `true` when `OnClick` is set. |
| OnClickStopPropagation | `bool?` | Stops event propagation when the item is clicked. Default is `null`, which means `true` when `OnClick` is set. |
| Text | `string` | Text of the item. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnClick | `EventCallback<MouseEventArgs>` | Raised when the item is clicked. |

