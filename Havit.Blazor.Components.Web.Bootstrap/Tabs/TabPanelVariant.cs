namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Render variations for <see cref="HxTabPanel"/>.
/// </summary>
public enum TabPanelVariant
{
	/// <summary>
	/// Renders the tab navigation as a nav (see <see cref="HxTabPanel.NavVariant"/>) above (or alongside) the tab content.
	/// </summary>
	Standard = 0,

	/// <summary>
	/// Renders the navigation to a card's header <a href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/card/#navigation">https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/card/#navigation</a>
	/// </summary>
	Card = 1,

	/// <summary>
	/// Renders the tab navigation as a list group <a href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/tab/#list-groups">https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/components/tab/#list-groups</a>.
	/// Combine with <see cref="NavOrientation.Vertical"/> (<see cref="HxTabPanel.Orientation"/>) to place the list group alongside the tab content.
	/// <see cref="HxTabPanel.NavVariant"/> does not apply.
	/// </summary>
	ListGroup = 2
}
