# HxEChart

Component for convenient rendering of Apache ECharts.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Options **[REQUIRED]** | `object` | Options for the chart. See ECharts Option for more details. |
| AdditionalAttributes | `Dictionary<string, object>` | Additional attributes to be splatted onto the underlying chart container `div`. |
| AutoResize | `bool` | Indicates whether the chart should automatically resize. Default is `false`. |
| ChartId | `string` | Unique identifier for the HTML element representing the chart. |
| CssClass | `string` | Additional CSS classes for the chart container. |
| Height | `string` | The height of the chart. Default is `400px`. Set to `null` or an empty string to omit the `height` declaration and let the surrounding CSS (e.g. a flex parent) drive the height. |
| Width | `string` | The width of the chart (rendered as both `min-width` and `max-width` of the container). Default is `100%`. Set to an empty string to omit the width declarations and let the surrounding CSS drive the width. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnAxisPointerUpdated | `EventCallback<EchartAxisPointerUpdatedEventArgs>` | Invoked when the user moves the axis pointer (e.g., when hovering over a chart). |
| OnClick | `EventCallback<EchartClickEventArgs>` | Invoked when the chart is clicked. |

## Available demo samples

- HxEChart_Demo_Basic.razor
- HxEChart_Demo_JSFunc.razor
- HxEChart_Demo_OnClick.razor

