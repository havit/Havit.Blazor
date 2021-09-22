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
	}
}