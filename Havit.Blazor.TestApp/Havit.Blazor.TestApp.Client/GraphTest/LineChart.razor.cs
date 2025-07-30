using Havit.Blazor.Components.Web.ECharts;

namespace Havit.Blazor.TestApp.Client.GraphTest;

public partial class LineChart
{
	[Parameter, EditorRequired] public List<ChartSeries> Data { get; set; }

	[Parameter, EditorRequired] public List<int> SelectedSeriesIds { get; set; }

	[Parameter] public string Height { get; set; } = "300px";
	[Parameter] public bool AutoResize { get; set; } = false;

	[Parameter] public EventCallback<ChartValue> OnValueClick { get; set; }
	[Parameter] public EventCallback<List<EChartAxisPointerUpdatedEventArgs>> OnAxisPointerUpdate { get; set; }


	private object _options;

	protected override void OnParametersSet()
	{
		if (!Data.Any())
		{
			return;
		}

		var yAxisTypes = Data.Select(s => s.ValueType).Distinct().ToList();

		_options = new
		{
			Tooltip = new
			{
				Trigger = "axis",
				BorderColor = "var(--bs-border-color)",
			},
			Legend = new
			{
				Show = false,
				Data = Data.Select(s => s.Name).ToArray(),
				Selected = Data.ToDictionary(s => s.Name, s => SelectedSeriesIds.Contains(s.SeriesId)),
			},
			XAxis = new
			{
				Data = Data.First().Values.Select(i => i.Label).ToArray()
			},
			YAxis = yAxisTypes.Select(axisType =>
				new
				{
					AlignTicks = true,
					AxisLabel = new
					{
						// Axis formatter uses ValueFormatter-like callback
						Formatter = axisType.CreateSimpleValueFormatterJsFunc(Data.First(d => d.ValueType == axisType).Unit, round: true),
					}
				}).ToArray(),
			Toolbox = new
			{
				Show = true,
				Feature = new
				{
					SaveAsImage = new { }
				}
			},
			Grid = new
			{
				ContainLabel = true,
				Left = "1%",
				Right = "1%",
				Top = "10%",
				Bottom = "5%",
			},
			Series = Data.Select(series =>
				new
				{
					Name = series.Name,
					Type = "line",
					Data = series.Values.Select(i => i.Value).ToArray(),
					ItemStyle = new
					{
						Color = series.Color
					},
					Label = new
					{
						Show = true,
						Position = "top",
						Formatter = series.ValueType.CreateSimpleFormatterJsFunc(series.Unit, round: true),
					},
					Tooltip = new
					{
						ValueFormatter = series.ValueType.CreateSimpleValueFormatterJsFunc(series.Unit, round: false),
					},
					yAxisIndex = yAxisTypes.IndexOf(series.ValueType)
				}
			).ToArray()
		};
	}

	private async Task HandleClick(EChartClickEventArgs eventParams)
	{
		if ((eventParams.SeriesIndex < Data.Count) && (eventParams.DataIndex < Data[eventParams.SeriesIndex.Value].Values.Count))
		{
			var item = Data[eventParams.SeriesIndex.Value].Values[eventParams.DataIndex.Value];
			await OnValueClick.InvokeAsync(item);
		}
	}
}
