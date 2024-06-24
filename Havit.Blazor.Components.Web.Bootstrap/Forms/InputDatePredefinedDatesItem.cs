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
	/// Date. Overrides any <see cref="DateSelector"/>.
	/// </summary>
	public DateTime? Date { get; set; }

	/// <summary>
	/// Used to supply the date at runtime, especially to use
	/// a TimeProvider. Not used if <see cref="Date"/> is set.
	/// </summary>
	public Func<TimeProvider, DateTime> DateSelector { get; set; }

	/// <summary>
	/// Defaults to returning the <see cref="Date"/>; otherwise, calls <see cref="DateSelector"/>.<br/>
	/// Used to resolve the date at runtime, particularly when a TimeProvider is needed.
	/// </summary>
	/// <param name="timeProvider"><see cref="HxCalendar.TimeProvider"/></param>
	/// <returns></returns>
	public DateTime GetDateEffective(TimeProvider timeProvider)
	{
		return Date ?? DateSelector(timeProvider);
	}
}
