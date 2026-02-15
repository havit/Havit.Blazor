# HxBadge

Bootstrap Badge component.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying HTML element. |
| Color | `ThemeColor?` | Badge color (background). |
| CssClass | `string` | Any additional CSS class to apply. |
| ChildContent | `RenderFragment` | Content of the component. |
| Settings | `BadgeSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| TextColor | `ThemeColor?` | Color of badge text. Use `Color` for the background color. The default is `ThemeColor.None` (color automatically selected to work with the chosen background color). |
| Type | `BadgeType?` | Badge type - Regular or rounded-pills. The default is `BadgeType.Regular`. |

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

