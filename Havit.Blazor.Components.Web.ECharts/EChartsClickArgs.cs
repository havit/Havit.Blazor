namespace Havit.Blazor.Components.Web.ECharts;

public record EChartsClickArgs
{
	public int SeriesIndex { get; set; }
	public int DataIndex { get; set; }
	public string Name { get; set; }
	public decimal Value { get; set; }
}
