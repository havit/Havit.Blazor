namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Numeric input in percentages with value normalization (0% = 0, 100% = 1.0).<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxInputPercent">https://havit.blazor.eu/components/HxInputPercent</see>
/// </summary>
public class HxInputPercent<TValue> : HxInputNumber<TValue>
{
	private static HashSet<Type> supportedTypes = new HashSet<Type> { typeof(float), typeof(double), typeof(decimal) };

	public HxInputPercent()
	{
		Type underlyingType = Nullable.GetUnderlyingType(typeof(TValue)) ?? typeof(TValue);
		if (!supportedTypes.Contains(underlyingType))
		{
			throw new InvalidOperationException($"Unsupported type {typeof(TValue)}.");
		}

		InputGroupEndText = "%";
	}

	/// <summary>
	/// Converts value into percentages and formats it as a string.
	/// </summary>
	protected override string FormatValueAsString(TValue value)
	{
		switch (value)
		{
			case null:
				return null;
			case float @float:
				@float *= 100;
				return base.FormatValueAsString((TValue)(object)@float);
			case double @double:
				@double *= 100;
				return base.FormatValueAsString((TValue)(object)@double);
			case decimal @decimal:
				@decimal *= 100;
				return base.FormatValueAsString((TValue)(object)@decimal);
			default:
				throw new InvalidOperationException($"Unsupported type {value.GetType()}.");
		}
	}

	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	protected override bool TryParseValueFromString(string value, out TValue result, out string validationErrorMessage)
	{
		bool success = base.TryParseValueFromString(value, out result, out validationErrorMessage);

		if (!success)
		{
			return false;
		}

		switch (result)
		{
			case float @float:
				@float /= 100;
				result = (TValue)(object)@float;
				break;
			case double @double:
				@double /= 100;
				result = (TValue)(object)@double;
				break;
			case decimal @decimal:
				@decimal /= 100;
				result = (TValue)(object)@decimal;
				break;
			default:
				throw new InvalidOperationException($"Unsupported type {value.GetType()}.");
		}

		return true;
	}
}
