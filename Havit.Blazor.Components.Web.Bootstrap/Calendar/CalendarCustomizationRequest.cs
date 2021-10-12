using System;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Calendar display customization request.
	/// </summary>
	public record CalendarCustomizationRequest
	{
		/// <summary>
		/// Who is asking for a customization.
		/// Useful for distinquishing From and To inputs in <see cref="HxInputDateRange"/>.
		/// </summary>
		public CalendarCustomizationTarget Target { get; init; }

		/// <summary>
		/// Customized date.
		/// </summary>
		public DateTime Date { get; init; }
	}
}