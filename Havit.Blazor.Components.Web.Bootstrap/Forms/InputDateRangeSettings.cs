using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxInputDateRange"/>.
/// </summary>
public record InputDateRangeSettings : InputsSettings, IInputSettingsWithSize
{
	/// <summary>
	/// Input size.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// Indicates whether the <i>Clear</i> and <i>OK</i> buttons in calendar should be visible.<br/>
	/// </summary>
	[Obsolete("ShowCalendarButtons is obsolete, use ShowClearButton instead.")]
	public bool? ShowCalendarButtons
	{
		get => ShowClearButton;
		set => ShowClearButton = value;
	}

	/// <summary>
	/// Indicates whether the <i>Clear</i> button in dropdown calendar should be visible.<br/>
	/// </summary>
	public bool? ShowClearButton { get; set; }

	/// <summary>
	/// First date selectable from the dropdown calendar.
	/// </summary>
	public DateTime? MinDate { get; set; }

	/// <summary>
	/// Last date selectable from the dropdown calendar.
	/// </summary>
	public DateTime? MaxDate { get; set; }

	/// <summary>
	/// Allows customization of the dates in dropdown calendars.
	/// </summary>
	public CalendarDateCustomizationProviderDelegate CalendarDateCustomizationProvider { get; set; }

	/// <summary>
	/// When enabled, shows predefined day ranges (from <see cref="HxInputDateRange.PredefinedDateRanges"/>, e.g. Today).
	/// </summary>
	public bool? ShowPredefinedDateRanges { get; set; }

	/// <summary>
	/// Predefined date ranges to be displayed.
	/// </summary>
	public IEnumerable<InputDateRangePredefinedRangesItem> PredefinedDateRanges { get; set; }
}