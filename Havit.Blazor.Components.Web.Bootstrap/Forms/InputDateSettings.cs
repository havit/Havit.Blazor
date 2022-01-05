using System;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for <see cref="HxInputDate{TValue}"/>.
	/// </summary>
	public record InputDateSettings : IInputSettingsWithSize
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
		/// Indicates whether the <i>Clear</i> and <i>OK</i> buttons in calendar should be visible.<br/>
		/// </summary>
		public bool? ShowCalendarButtons { get; set; }

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
	}
}