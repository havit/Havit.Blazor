namespace Havit.Blazor.Documentation.Shared.Components;

/// <summary>
/// Tracks usages of <see cref="DocHeadContent"/> component during page rendering
/// and returns the canonical URL of the first registration.
/// </summary>
public class DocHeadContentTracker(NavigationManager navigationManager)
{
	private readonly NavigationManager _navigationManager = navigationManager;

	private string _activePageUri;
	private string _canonicalUrl;

	/// <summary>
	/// Registers a canonical URL for the current page and returns true
	/// if the registration was successful (first registration for the current page rendered).
	/// </summary>
	public bool TryRegisterCanonicalUrlForCurrentPage(string canonicalUrl)
	{
		Contract.Requires<ArgumentNullException>(canonicalUrl != null);

		ResetIfCurrentPageUrlChanged();

		if (_canonicalUrl is null)
		{
			_canonicalUrl = canonicalUrl;
			return true;
		}

		return false;
	}

	private void ResetIfCurrentPageUrlChanged()
	{
		string currentUri = _navigationManager.Uri;
		if (_activePageUri != currentUri)
		{
			_canonicalUrl = null;
			_activePageUri = currentUri;
		}
	}

	private string GetRegisteredCanonicalUrl()
	{
		if (_navigationManager.Uri == _activePageUri)
		{
			// if there was an explicit registration for this page, return the canonical URL
			return _canonicalUrl;
		}
		return null;
	}

	public string GetAbsoluteCanonicalUrl()
	{
		string result = null;
		var relativeUrl = GetRegisteredCanonicalUrl();
		if (relativeUrl != null)
		{
			result = "https://havit.blazor.eu/" + relativeUrl.TrimStart('/');
			result = result.TrimEnd('/');
		}

		return result;
	}
}
