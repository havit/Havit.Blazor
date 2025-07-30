namespace Havit.Blazor.TestApp.Client.GraphTest;

public record ChartValue
{
	public ChartValue(string label, decimal value, string id = null, string color = null)
	{
		Label = label;
		Value = value;
		Id = id;
		Color = color;
	}

	public string Label { get; init; }

	public decimal Value { get; init; }

	/// <summary>
	/// Id for future callbacks, e.g. for drilldown.
	/// </summary>
	public string Id { get; init; }

	/// <summary>
	/// Color for the value (for those charts that support it, e.g. pie chart).
	/// </summary>
	public string Color { get; set; }
}