# HxPager

Pager.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| CurrentPageIndex **[REQUIRED]** | `int` | Current page index. Zero-based. Displayed numbers start with 1, which is page index 0. |
| TotalPages **[REQUIRED]** | `int` | Total number of pages of data items. Has to be equal to or greater than `1`. |
| CssClass | `string` | Any additional CSS class to apply. |
| CurrentPageIndexChanged | `EventCallback<int>` | Event raised when the page index is changed. |
| FirstPageContentTemplate | `RenderFragment` | Content for the "First page" button. If not set, the `FirstPageIcon` is used. |
| FirstPageIcon | `IconBase` | Icon for the "First page" button. |
| LastPageContentTemplate | `RenderFragment` | Content for the "Last page" button. If not set, the `LastPageIcon` is used. |
| LastPageIcon | `IconBase` | Icon for the "Last page" button. |
| NextPageContentTemplate | `RenderFragment` | Content for the "Next page" button. If not set, the `NextPageIcon` is used. |
| NextPageIcon | `IconBase` | Icon for the "Next page" button. |
| NumericButtonsCount | `int?` | Count of numbers to display. The default value is 10. |
| PreviousPageContentTemplate | `RenderFragment` | Content for the "Previous page" button. If not set, the `PreviousPageIcon` is used. |
| PreviousPageIcon | `IconBase` | Icon for the "Previous page" button. |
| Settings | `PagerSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `PagerSettings` | Application-wide defaults for `HxPager`. |

## Available demo samples

- HxPager_Demo.razor
- HxPager_Demo_Customization.razor

