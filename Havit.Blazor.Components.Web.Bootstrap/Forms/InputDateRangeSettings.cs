using System;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for <see cref="HxInputDateRange"/>.
	/// </summary>
	public class InputDateRangeSettings : IInputSettingsWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		public InputSize InputSize { get; set; } = InputSize.Regular;

		/// <summary>
		/// Indicates whether the <i>Clear</i> and <i>OK</i> buttons in calendars should be visible.<br/>
		/// Default is <c>true</c> (configurable in <see cref="HxInputDateRange.Defaults"/>).
		/// </summary>
		public bool ShowCalendarButtons { get; set; } = true;

		/// <summary>
		/// First date selectable from the dropdown calendar.
		/// Default is <c>1.1.1900</c>.
		/// </summary>
		public DateTime MinDate { get; set; } = CalendarSettings.DefaultMinDate;

		/// <summary>
		/// Last date selectable from the dropdown calendar.
		/// Default is <c>31.12.2099</c>
		/// </summary>
		public DateTime MaxDate { get; set; } = CalendarSettings.DefaultMaxDate;

		/// <summary>
		/// Allows customization of the dates in dropdown calendars.
		/// </summary>
		public CalendarDateCustomizationProviderDelegate CalendarDateCustomizationProvider { get; set; } = (request) => null;
	}
}