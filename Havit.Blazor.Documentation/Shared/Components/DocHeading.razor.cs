using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Shared.Components;

public partial class DocHeading(
	IDocPageNavigationItemsTracker docPageNavigationItemsTracker,
	NavigationManager navigationManager)
{
	private readonly IDocPageNavigationItemsTracker _docPageNavigationItemsTracker = docPageNavigationItemsTracker;
	private readonly NavigationManager _navigationManager = navigationManager;

	/// <summary>
	/// Id of the section.
	/// </summary>
	[Parameter] public string Id { get; set; }

	/// <summary>
	/// Title of the section. If not set, <c>Title</c> is extracted from the <c>Href</c>.
	/// </summary>
	[Parameter, EditorRequired] public string Title { get; set; }

	/// <summary>
	/// Determines the heading tag to be used. Level <c></c> should be used for sections and higher integers for subsections.
	/// </summary>
	[Parameter] public int Level { get; set; } = 2;

	private string _idEffective;
	private string _headingTag;
	private string _hrefEffective;

	protected override void OnParametersSet()
	{
		_idEffective = Id ?? Title.NormalizeForUrl();
		_headingTag = "h" + Math.Min(Level, 6);

		string currentUri = _navigationManager.Uri;
		_hrefEffective = UrlHelper.RemoveFragmentFromUrl(currentUri) + "#" + _idEffective;

		_docPageNavigationItemsTracker.RegisterNavigationItem(currentUri, new DocPageNavigationItem()
		{
			Id = _idEffective,
			Level = Level,
			Title = Title
		});
	}
}
