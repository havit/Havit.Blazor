namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Settings for the <see cref="HxProgressIndicator"/> and derived components.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/types/ProgressIndicatorSettings">https://havit.blazor.eu/types/ProgressIndicatorSettings</see>
	/// </summary>
	public record ProgressIndicatorSettings
	{
		/// <summary>
		/// Debounce delay in miliseconds.
		/// </summary>
		public int? Delay { get; set; }
	}
}
