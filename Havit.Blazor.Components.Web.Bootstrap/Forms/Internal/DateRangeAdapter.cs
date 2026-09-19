namespace Havit.Blazor.Components.Web.Bootstrap.Forms.Internal;

/// <summary>
/// Adapts supported model values to the calendar's DateTime representation without time-zone conversion.
/// The public input owns the original model expression and validation field.
/// </summary>
internal static class DateRangeAdapter<TValue>
{
	internal static void EnsureSupportedType()
	{
		if ((typeof(TValue) != typeof(DateTimeRange)) && (typeof(TValue) != typeof(DateOnlyRange)))
		{
			throw new InvalidOperationException($"Unsupported range type {typeof(TValue)}. Use DateTimeRange or DateOnlyRange.");
		}
	}

	internal static DateTimeRange ToDateTimeRange(TValue value)
	{
		return value switch
		{
			DateTimeRange range => range,
			DateOnlyRange range => new DateTimeRange(range.StartDate?.ToDateTime(TimeOnly.MinValue), range.EndDate?.ToDateTime(TimeOnly.MinValue)),
			_ => throw new InvalidOperationException($"Unsupported range type {typeof(TValue)}. Use DateTimeRange or DateOnlyRange.")
		};
	}

	internal static TValue FromDateTimeRange(DateTimeRange value)
	{
		if (typeof(TValue) == typeof(DateTimeRange))
		{
			return (TValue)(object)value;
		}
		if (typeof(TValue) == typeof(DateOnlyRange))
		{
			return (TValue)(object)new DateOnlyRange(
				value.StartDate.HasValue ? DateOnly.FromDateTime(value.StartDate.Value) : null,
				value.EndDate.HasValue ? DateOnly.FromDateTime(value.EndDate.Value) : null);
		}
		throw new InvalidOperationException($"Unsupported range type {typeof(TValue)}. Use DateTimeRange or DateOnlyRange.");
	}
}
