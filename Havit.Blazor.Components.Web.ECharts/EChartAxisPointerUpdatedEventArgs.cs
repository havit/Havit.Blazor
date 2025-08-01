using System.Text.Json;

namespace Havit.Blazor.Components.Web.ECharts;

/// <summary>
/// Represents the arguments for the <see cref="HxEChart.OnAxisPointerUpdated" /> event callback.
/// </summary>
public record EChartAxisPointerUpdatedEventArgs
{
	/// <summary>
	/// Series index in incoming <c>Options.Series</c>.
	/// </summary>
	public int SeriesIndex { get; set; }

	/// <summary>
	/// Data index in inside the incoming data array.
	/// </summary>
	public int DataIndexInside { get; set; }

	/// <summary>
	/// Data index in incoming data array.
	/// </summary>
	public int DataIndex { get; set; }

	/// <summary>
	/// List of all axis and their info.
	/// </summary>
	public List<EChartAxisPointerUpdatedAxisInfo> AxesInfo { get; set; } = new();
}
