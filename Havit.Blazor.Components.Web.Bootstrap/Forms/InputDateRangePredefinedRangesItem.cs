namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Item for <see cref="HxInputDateRange{TValue}.PredefinedDateRanges" />.
/// </summary>
public class InputDateRangePredefinedRangesItem
{
	/// <summary>
	/// Custom label.
	/// </summary>
	public string Label { get; set; }

	/// <summary>
	/// Resource type for IStringLocalizer&lt;ResourceType&gt; where the localization will be searched.
	/// </summary>
	public Type ResourceType { get; set; }

	/// <summary>
	/// Date range. Converted internally when the input is bound to DateOnlyRange.
	/// </summary>
	public DateTimeRange DateRange { get; set; }
}
