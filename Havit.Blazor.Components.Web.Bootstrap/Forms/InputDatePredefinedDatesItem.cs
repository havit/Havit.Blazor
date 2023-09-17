namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Item for <see cref="HxInputDate{TValue}.PredefinedDates" />.
/// </summary>
public class InputDatePredefinedDatesItem
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
	/// Date.
	/// </summary>
	public DateTime? Date { get; set; }

	/// <summary>
	/// Used to supply the date at runtime especially to use
	/// a TimeProvider
	/// </summary>
	public Func<TimeProvider, DateTime> DateSelector { get; set; }
}
