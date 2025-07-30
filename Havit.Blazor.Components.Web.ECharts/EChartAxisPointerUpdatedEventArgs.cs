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
	public int? SeriesIndex { get; set; }

	/// <summary>
	/// Data index in incoming data array.
	/// </summary>
	public int? DataIndex { get; set; }

	/// <summary>
	/// Dimension of the axis.
	/// </summary>
	public string AxisDimension { get; set; }

	/// <summary>
	/// Incoming data value. Use <see cref="GetValue{T}"/> to extract the value as the specified type.
	/// </summary>
	public object Value { get; set; }

	/// <summary>
	/// Returns the <see cref="Value"/> as the specified type <typeparamref name="T"/>.
	/// </summary>
	public T GetValue<T>()
	{
		if (Value is null)
		{
			return default;
		}

		if (Value is T typedValue)
		{
			return typedValue;
		}

		if (Value is JsonElement jValue)
		{
			if (typeof(T) == typeof(string))
			{
				return (T)(object)jValue.ToString();
			}

			try
			{
				return jValue.Deserialize<T>();
			}
			catch (JsonException ex)
			{
				throw new InvalidOperationException($"Unable to deserialize JsonElement '{Value}' to type '{typeof(T)}'.", ex);
			}
		}

		throw new NotSupportedException($"Value of type '{Value.GetType()}' is not supported.");
	}
}
