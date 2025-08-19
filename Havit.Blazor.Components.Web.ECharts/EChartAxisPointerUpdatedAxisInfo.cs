using System.Text.Json;

namespace Havit.Blazor.Components.Web.ECharts;

public record EChartAxisPointerUpdatedAxisInfo
{
	/// <summary>
	/// Dimension of the axis. Either x or y.
	/// </summary>
	public string AxisDim { get; set; }

	/// <summary>
	/// Index of the axis.
	/// </summary>
	public int AxisIndex { get; set; }

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
