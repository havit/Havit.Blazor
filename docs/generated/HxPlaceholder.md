# HxPlaceholder

Bootstrap 5 Placeholder component, also known as Skeleton. Use loading placeholders for your components or pages to indicate that something may still be loading.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto an underlying HTML element. |
| Color | `ThemeColor?` | Color of the placeholder. |
| Columns | `string` | Number of template columns to span. Responsive setting for all devices including the extra-small ones (XS) below "small" breakpoint (`576px`). The value can be any integer number between `1` and `12` (`.col-1`), `auto` (`.col-auto`) or `true` (`.col`). |
| ColumnsExtraLargeUp | `string` | Number of template columns to span for viewports above the "extra-large" breakpoint (`1200px`). The value can be any integer number between `1` and `12`, `auto`, or `true`. |
| ColumnsLargeUp | `string` | Number of template columns to span for viewports above the "large" breakpoint (`992px`). The value can be any integer number between `1` and `12`, `auto`, or `true`. |
| ColumnsMediumUp | `string` | Number of template columns to span for viewports above the "medium" breakpoint (`768px`). The value can be any integer number between `1` and `12`, `auto`, or `true`. |
| ColumnsSmallUp | `string` | Number of template columns to span for viewports above the "small" breakpoint (`576px`). The value can be any integer number between `1` and `12`, `auto`, or `true`. |
| ColumnsXxlUp | `string` | Number of template columns to span for viewports above the "XXL" breakpoint (`1400px`). The value can be any integer number between `1` and `12`, `auto`, or `true`. |
| CssClass | `string` | Additional CSS class. |
| ChildContent | `RenderFragment` | Optional content of the placeholder (usually not used). |
| Settings | `PlaceholderSettings` | Set of settings to be applied to the component instance (overrides `Defaults`, overridden by individual parameters). |
| Size | `PlaceholderSize?` | Size of the placeholder. |

## Static properties

| Property | Type | Description |
|----------|------|-------------|
| Defaults | `PlaceholderSettings` | Application-wide defaults for the `HxPlaceholder` and derived components. |

## Available demo samples

- HxPlaceholder_Demo_Animation.razor
- HxPlaceholder_Demo_BasicExample.razor
- HxPlaceholder_Demo_Colors.razor
- HxPlaceholder_Demo_Sizing.razor
- HxPlaceholder_Demo_Width.razor

