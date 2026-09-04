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

	/// <summary>
	/// Whether <see cref="PageViewEventName"/> is pushed for the page the document was loaded with.
	/// <para>
	/// Set to <c>false</c> when the GTM container already covers document loads with the <c>pageview</c> event
	/// or its built-in Page View trigger — otherwise the landing page of every session is counted twice.
	/// Explicit <see cref="IHxGoogleTagManager.PushPageViewAsync(object)"/> calls are not affected.
	/// </para>
	/// Default: <c>true</c>.
	/// </summary>
	public bool EnableInitialPageViewTracking { get; set; } = true;
}