# NavOverflowSettings

Settings for `HxNavOverflow`.

## Properties

| Name | Type | Description |
|------|------|-------------|
| CollapseBelow | `string` | Container width threshold below which all items collapse into the overflow menu. A breakpoint name (e.g. `md`, resolved from the `--bs-breakpoint-{name}` CSS variable) or a pixel value (e.g. `768`). |
| CssClass | `string` | Additional CSS class(es) for the nav. |
| IconPlacement | `NavOverflowIconPlacement?` | Position of the icon relative to the text in the "More" toggle button. |
| MenuPlacement | `MenuPlacement?` | Placement of the overflow menu relative to the "More" toggle button. |
| MinimumVisibleItems | `int?` | Minimum number of items to keep visible before the remaining items overflow into the menu. |
| MoreIcon | `IconBase` | Icon of the "More" toggle button. |
| MoreText | `string` | Text of the "More" toggle button. |

