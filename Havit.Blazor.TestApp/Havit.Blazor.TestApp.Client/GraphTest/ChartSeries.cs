
namespace Havit.Blazor.TestApp.Client.GraphTest;

public record ChartSeries
{
	public int SeriesId { get; set; }

	public string Name { get; set; }

	public string Unit { get; set; }

	/// <summary>
	/// Money, hours, other... determines value formatting
	/// </summary>
	public ChartValueType ValueType { get; set; }

	/// <summary>
	/// Color for the series (for those charts that support it, e.g. line chart).
	/// </summary>
	public string Color { get; set; }

	public List<ChartValue> Values { get; set; }
}
