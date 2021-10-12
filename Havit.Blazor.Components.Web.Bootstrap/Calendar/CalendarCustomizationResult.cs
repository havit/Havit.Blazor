namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Calendar display customization result.
	/// </summary>
	public class CalendarCustomizationResult
	{
		/// <summary>
		/// Indicates whether the date is enabled.
		/// Default value is true.
		/// </summary>
		public bool Enabled { get; init; } = true;

		/// <summary>
		/// Specifies css class for the date date.
		/// </summary>
		public string CssClass { get; init; }
	}
}