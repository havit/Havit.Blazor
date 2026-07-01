# HxBadge

Bootstrap Badge component.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying HTML element. |
| ChildContent | `RenderFragment` | Content of the component. |
| Color | `ThemeColor?` | Badge color (background). |
| CssClass | `string` | Any additional CSS class to apply. |
| Settings | `BadgeSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Type | `BadgeType?` | Badge type - Regular or rounded-pills. The default is `BadgeType.Regular`. |
| Variant | `BadgeVariant?` | Visual variant of the badge (solid, subtle, outline), composed with `Color`. The default is `BadgeVariant.Solid`. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `BadgeSettings` | Application-wide defaults for `HxBadge` and derived components. |

## Available demo samples

- HxBadge_Demo_Buttons.razor
- HxBadge_Demo_Colors.razor
- HxBadge_Demo_Headings.razor
- HxBadge_Demo_Pills.razor
- HxBadge_Demo_Positioned.razor
- HxBadge_Demo_Positioned_RoundedCircle.razor
- HxBadge_Demo_Variants.razor

