using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for <see cref="HxInputDate{TValue}"/>.
/// </summary>
public record InputDateSettings : InputSettings, IInputSettingsWithSize
{
	/// <summary>
	/// Input size.
	/// </summary>
	public InputSize? InputSize { get; set; }

	/// <summary>
	/// Optional icon to display within the input.
	/// </summary>
	public IconBase CalendarIcon { get; set; }

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
	/// When enabled, shows predefined days (from <see cref="HxInputDate{TValue}.PredefinedDates"/>, e.g. Today).
	/// </summary>
	public bool? ShowPredefinedDates { get; set; }

	/// <summary>
	/// Predefined dates to be displayed.
	/// </summary>
	public IEnumerable<InputDatePredefinedDatesItem> PredefinedDates { get; set; }

	/// <summary>
	/// TimeProvider to use, note: override the 'Today' in PredefinedDates
	/// </summary>
	public TimeProvider TimeProvider { get; set; }
}