namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Automation mode for <see cref="HxAnchorFragmentNavigation"/>.
/// </summary>
public enum AnchorFragmentNavigationAutomationMode
{
	/// <summary>
	/// Scrolls to anchor on <c>firstRender</c> and whenever location changes (<see cref="NavigationManager.LocationChanged"/>).
	/// With <see cref="HxScrollspy"/> this mode is suitable only for static pages (where the size/offset of individual sections remains the same from the very beginning).
	/// Use <see cref="SamePage"/> or <see cref="Manual"/> for when the page contents load asynchronously and the layout changes.
	/// </summary>
	Full = 0,

	/// <summary>
	/// Explicit calls to <see cref="HxAnchorFragmentNavigation.ScrollToCurrentUriFragmentAsync()"/> or <see cref="HxAnchorFragmentNavigation.ScrollToAnchorAsync(string)"/> needed.
	/// Use this mode with <see cref="HxScrollspy"/> when you need scrollspy to call <see cref="HxScrollspy.RefreshAsync"/> to recalculate the target offsets before scrolling (scrollspy does not work properly on scrolled content).
	/// </summary>
	Manual = 1,

	/// <summary>
	/// Same as <see cref="Manual"/> but scrolls to anchor on <see cref="NavigationManager.LocationChanged"/> if the page remains the same (just the #fragment portion changed).
	/// Works for most scenarios when you refresh the <see cref="HxScrollspy"/> after data load and then you just need to navigate over the page.
	/// </summary>
	SamePage = 2
}