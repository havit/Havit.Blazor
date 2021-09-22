using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Default values for <see cref="HxInputDate{TValue}"/> and derived components.
	/// </summary>
	public class InputDateDefaults : IInputDefaultsWithSize
	{
		/// <summary>
		/// Input size.
		/// </summary>
		public InputSize InputSize { get; set; } = InputSize.Regular;

		/// <summary>
		/// Optional icon to display within the input.
		/// </summary>
		public IconBase CalendarIcon { get; set; }

		/// <summary>
		/// Indicates whether the <i>Clear</i> and <i>OK</i> buttons in calendar should be visible.<br/>
		/// Default is <c>true</c>.
		/// </summary>
		public bool ShowCalendarButtons { get; set; } = true;
	}
}