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
	/// Default is <c>true</c>.
	/// <para>
	/// The GTM snippet already announces a document load on its own (the container's built-in Page View trigger,
	/// plus a <c>pageview</c> event pushed for backwards compatibility), so with the default the first page of
	/// a session is announced twice - once as <c>pageview</c> and once as <see cref="PageViewEventName"/>.
	/// That is what you want when the container treats <see cref="PageViewEventName"/> as the single source
	/// of truth and ignores <c>pageview</c>.
	/// </para>
	/// <para>
	/// Set to <c>false</c> when the container instead uses <c>pageview</c> (or its built-in Page View trigger)
	/// for real document loads and <see cref="PageViewEventName"/> only for the navigations that follow.
	/// Without it those containers count the landing page of every session twice.
	/// </para>
	/// <para>
	/// Only automatic tracking is affected - an explicit <see cref="IHxGoogleTagManager.PushPageViewAsync(object)"/>
	/// call always pushes.
	/// </para>
	/// </summary>
	public bool EnableInitialPageViewTracking { get; set; } = true;
}