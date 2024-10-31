namespace Havit.Blazor.Documentation.Services;

public class CanonicalLinkManager(NavigationManager NavigationManager)
{
	/// <summary>
	/// Indicates, whether a <c>ComponentApiDoc</c> component has already been registered on this page.
	/// </summary>
	private bool _componentApiDocRegistered = false;

	private string _lastPageRelativeUrl;

	/// <summary>
	/// Registers a <c>ComponentApiDoc</c> component and determines, whether a <c>rel="canonical</c> <c>link</c> tag should be rendered.
	/// </summary>
	/// <returns><c>true</c> if this is the first <c>ComponentApiDoc</c> on the current page, <c>false</c> if a <c>ComponentApiDoc</c> has already been registered.</returns>
	public bool RegisterComponentApiDoc()
	{
		string currentRelativeUrl = NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
		if (_lastPageRelativeUrl != currentRelativeUrl)
		{
			_componentApiDocRegistered = false;
			_lastPageRelativeUrl = currentRelativeUrl;
		}

		bool firstRegistration = !_componentApiDocRegistered;
		_componentApiDocRegistered = true;
		return firstRegistration;
	}
}
