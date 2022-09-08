using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.GoogleTagManager;

/// <summary>
/// Support for <see href="https://developers.google.com/tag-manager/devguide">Google Tag Manager</see> - initialization and pushing data to data-layer.
/// </summary>
public interface IHxGoogleTagManager
{
	/// <summary>
	/// Initializes the GTM support.
	/// Called automatically within first <c>Push</c> call (incl. <see cref="HxGoogleTagManagerPageViewTracker"/> calls).
	/// To be used explicitly only in those rare cases when you want to initialize GTM without pushing any data.
	/// </summary>
	Task InitializeAsync();

	/// <summary>
	/// Push generic data to GTM data-layer (using regular JSON-serialization).
	/// </summary>
	Task PushAsync(object data);

	/// <summary>
	/// Push event to GTM data-layer.
	/// </summary>
	Task PushEventAsync(string eventName, object eventData = null);

	/// <summary>
	/// Push page-view to GTM data-layer.
	/// Consider using <see cref="HxGoogleTagManagerPageViewTracker"/> instead of manual handling.
	/// </summary>
	Task PushPageViewAsync(object additionalData = null);

	/// <summary>
	/// Used by <see cref="HxGoogleTagManagerPageViewTracker"/> to track location changes.
	/// </summary>
	Task PushPageViewAsync(LocationChangedEventArgs args);
}
