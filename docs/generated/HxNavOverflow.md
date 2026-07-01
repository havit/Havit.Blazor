# HxNavOverflow

Bootstrap Nav overflow component (new in Bootstrap 6, "Priority+" pattern). Automatically moves nav items which do not fit the available width into a "More" overflow menu. The overflow is responsive to the container width (uses a `ResizeObserver`), not the viewport width. Unlike the Bootstrap `NavOverflow` plugin (which clones nav items into the menu and is therefore incompatible with Blazor rendering), the component renders the nav content twice (once in the nav, once in the overflow menu) and only toggles the visibility of the individual items. All items remain fully Blazor-owned (re-rendering, event handlers, and active state work in both places).

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto the underlying `<nav>` element. |
| ChildContent | `RenderFragment` | Content of the nav (typically `HxNavLink` components). The content is rendered twice—once as the nav items and once as the overflow menu items (visibility of the individual items is toggled based on the available width). |
| CollapseBelow | `string` | Container width threshold below which all items collapse into the overflow menu (mirrors the Bootstrap `collapseBelow` option). A breakpoint name (e.g. `md`, resolved from the `--bs-breakpoint-{name}` CSS variable) or a pixel value (e.g. `768`). Items with the `HxNavOverflow.KeepVisibleCssClass` CSS class remain visible. The default is `null` (disabled). |
| CssClass | `string` | Additional CSS class. |
| IconPlacement | `NavOverflowIconPlacement?` | Position of the icon relative to the text in the "More" toggle button. The default is `NavOverflowIconPlacement.Start`. |
| Id | `string` | ID of the nav element. |
| MenuPlacement | `MenuPlacement?` | Placement of the overflow menu relative to the "More" toggle button. The default is `MenuPlacement.BottomEnd`. |
| MinimumVisibleItems | `int?` | Minimum number of items to keep visible before the remaining items overflow into the menu (mirrors the Bootstrap `threshold` option). The default is `0`. |
| MoreIcon | `IconBase` | Icon of the "More" toggle button. The default is `BootstrapIcon.ThreeDots`. |
| MoreText | `string` | Text of the "More" toggle button. The default is `More`. Set to `null` or an empty string to render an icon-only toggle. |
| Settings | `NavOverflowSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Variant | `NavVariant` | The visual variant of the nav items. The default value is `NavVariant.Standard`. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnOverflowChanged | `EventCallbackNavOverflowChangedEventArgs>` | Raised when the number of items in the overflow menu changes (including back to zero). |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| UpdateAsync() | `Task` | Recalculates which items should overflow. Called automatically on container resize and on nav content changes, use only when the automatic detection is not sufficient. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `NavOverflowSettings` | Application-wide defaults for `HxNavOverflow` and derived components. |

## Available demo samples

- HxNavOverflow_Demo.razor
- HxNavOverflow_Demo_CollapseBelow.razor
- HxNavOverflow_Demo_KeepVisible.razor
- HxNavOverflow_Demo_MinimumVisibleItems.razor
- HxNavOverflow_Demo_OnOverflowChanged.razor
- HxNavOverflow_Demo_Toggle.razor
- HxNavOverflow_Demo_Variants.razor

