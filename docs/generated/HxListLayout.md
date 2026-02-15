# HxListLayout

Provides a unified layout for data presentation components and associated filtering controls. This component orchestrates the interaction between filter controls and the data presentation component. The data list is typically implemented using a `HxGrid` component. Filters are displayed in a `HxOffcanvas` component, while filter values are shown as `HxChipList`. Additionally, it supports predefined named views for quick switching between different filter configurations and other features such as a title, search box, and commands.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| CardSettings | `CardSettings` | Settings for the wrapping `HxCard`. |
| CommandsTemplate | `RenderFragment` |  |
| CssClass | `string` | Additional CSS classes for the wrapping `div`. |
| DataTemplate | `RenderFragment` |  |
| DetailTemplate | `RenderFragment` |  |
| FilterModel | `TFilterModel` |  |
| FilterModelChanged | `EventCallback<TFilterModel>` |  |
| FilterOffcanvasSettings | `OffcanvasSettings` | Settings for the `HxOffcanvas` with the filter. |
| FilterOpenButtonSettings | `ButtonSettings` | Settings for the `HxButton` opening the filtering offcanvas. |
| FilterSubmitButtonSettings | `ButtonSettings` | Settings for the `HxButton` submitting the filter. |
| FilterTemplate | `RenderFragment<TFilterModel>` |  |
| NamedViews | `IEnumerable<NamedView<TFilterModel>>` | Represents the collection of Named Views available for selection. Each Named View defines a pre-set filter configuration that can be applied to the data. |
| SearchTemplate | `RenderFragment` |  |
| SelectedNamedView | `NamedView<TFilterModel>` | Selected named view (highlighted in the list with `.active` CSS class). |
| SelectedNamedViewChanged | `EventCallback<NamedView<TFilterModel>>` |  |
| Settings | `ListLayoutSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Title | `string` | Title of the component. If `TitleFromNamedView` is `true` and `SelectedNamedView` is not `null`, the component's title displays the name of the currently selected Named View. |
| TitleFromNamedView | `bool` | Indicates whether the name of the selected Named View (`SelectedNamedView`) is automatically used as title. If `true`, the component's title changes to match the name of the currently selected Named View. Useful for dynamic title updates based on user selections from predefined views. The default value is `true`. |
| TitleTemplate | `RenderFragment` | Title of the component (in form of RenderFragment). If `TitleFromNamedView` is `true` and `SelectedNamedView` is not `null`, the component's title displays the name of the currently selected Named View. |

## Methods

| Method | Returns | Description |
|--------|---------|-------------|
| HideFilterOffcanvasAsync() | `Task` | Closes the filter offcanvas. |
| ShowFilterOffcanvasAsync() | `Task` | Opens the filter offcanvas. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `ListLayoutSettings` | Application-wide defaults for `HxListLayout` and derived components. |

## Available demo samples

- HxListLayout_Demo_Basic.razor
- HxListLayout_Demo_InfiniteScrollSticky.razor
- HxListLayout_Demo_NamedViews.razor
- HxListLayout_Demo_SearchTemplate.razor
- HxListLayout_Demo_TitleTemplate.razor

