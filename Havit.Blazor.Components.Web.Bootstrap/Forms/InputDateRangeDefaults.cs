using System;
using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Default values for <see cref="HxInputDateRange"/> and derived components.
	/// </summary>
	public class InputDateRangeDefaults : IInputDefaultsWithSize
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
		public DateTime MinDate { get; set; } = CalendarDefaults.DefaultMinDate;

		/// <summary>
		/// Last date selectable from the dropdown calendar.
		/// Default is <c>31.12.2099</c>
		/// </summary>
		public DateTime MaxDate { get; set; } = CalendarDefaults.DefaultMaxDate;
	}
}