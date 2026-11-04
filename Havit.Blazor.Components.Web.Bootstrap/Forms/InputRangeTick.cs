namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A tick mark for <see cref="HxInputRange{TValue}"/>. The ticks are rendered from a linked <c>&lt;datalist&gt;</c>
/// by the Bootstrap Range plugin, each positioned at its <see cref="Value"/> (even when the values are unevenly spaced).
/// </summary>
public record InputRangeTick
{
	/// <summary>
	/// The value at which the tick is positioned (on the same scale as <c>Min</c>/<c>Max</c>).
	/// </summary>
	public double Value { get; set; }

	/// <summary>
	/// The optional label shown beneath the tick. When <c>null</c> or empty, only the tick mark is rendered.
	/// </summary>
	public string Label { get; set; }

	public InputRangeTick()
	{
	}

	public InputRangeTick(double value, string label = null)
	{
		Value = value;
		Label = label;
	}
}
