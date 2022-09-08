namespace Havit.Blazor.GoogleTagManager;

/// <summary>
/// Options for <see cref="HxGoogleTagManager"/>.
/// </summary>
public class HxGoogleTagManagerOptions
{
	/// <summary>
	/// GTM-ID
	/// </summary>
	public string GtmId { get; set; }

	/// <summary>
	/// Name of the event pushed when page-view is tracked.
	/// </summary>
	public string PageViewEventName { get; set; } = "virtualPageView";

	/// <summary>
	/// Name of the variabel to be used for URL when page-view is tracked.
	/// </summary>
	public string PageViewUrlVariableName { get; set; } = "pageUrl";
}