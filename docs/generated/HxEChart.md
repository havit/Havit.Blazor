# HxEChart

Component for convenient rendering of Apache ECharts.

## Parameters

| Name | Type | Description |
|------|------|-------------|
| Options **[REQUIRED]** | `object` | Options for the chart. See ECharts Option for more details. |
| AutoResize | `bool` | Indicates whether the chart should automatically resize. Default is `false`. |
| Height | `string` | The height of the chart. Default is `400px`. |
| ChartId | `string` | Unique identifier for the HTML element representing the chart. |

## Event callbacks

| Name | Type | Description |
|------|------|-------------|
| OnAxisPointerUpdated | `EventCallback<EchartAxisPointerUpdatedEventArgs>` | Invoked when the user moves the axis pointer (e.g., when hovering over a chart). |
| OnClick | `EventCallback<EchartClickEventArgs>` | Invoked when the chart is clicked. |

## Available demo samples

- HxEChart_Demo_Basic.razor
- HxEChart_Demo_JSFunc.razor
- HxEChart_Demo_OnClick.razor

