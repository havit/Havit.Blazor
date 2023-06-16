namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Settings for the <see cref="HxProgressIndicator"/> and derived components.
/// </summary>
public record ProgressIndicatorSettings
{
	/// <summary>
	/// Debounce delay in milliseconds.
	/// </summary>
	public int? Delay { get; set; }
}
