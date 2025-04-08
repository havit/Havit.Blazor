namespace Havit.Blazor.Components.Web.ECharts;

/// <summary>
/// Represents the arguments for the <see cref="HxEChart.OnClick" /> event callback.
/// </summary>
public record EChartClickEventArgs
{
	/// <summary>
	/// Gets or sets the index of the series.
	/// </summary>
	public int? SeriesIndex { get; set; }

	/// <summary>
	/// Gets or sets the index of the data point.
	/// </summary>
	public int? DataIndex { get; set; }

	/// <summary>
	/// Gets or sets the name of the data point.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Gets or sets the value (string serialization) of the data point.
	/// </summary>
	public string Value { get; set; }

	/// <summary>
	/// Indicates what type of click event is it.
	/// </summary>
	public string TargetType { get; set; }
}
