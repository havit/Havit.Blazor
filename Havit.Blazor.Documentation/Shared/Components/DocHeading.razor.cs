using System.Text.RegularExpressions;
using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Shared.Components;

public partial class DocHeading : IDocPageNavigationItem
{
	/// <summary>
	/// Which heading tags are to be used for which levels.
	/// </summary>
	protected static readonly Dictionary<int, string> LevelHeadingTags = new()
	{
		{ 1, "h1" },
		{ 2, "h2" },
		{ 3, "h3" },
		{ 4, "h4" },
		{ 5, "h5" },
		{ 6, "h6" }
	};

	[Inject] public NavigationManager NavigationManager { get; set; }

	/// <summary>
	/// Id of the section.
	/// </summary>
	[Parameter] public string Id { get; set; }

	/// <summary>
	/// Title of the section. If not set, <c>Title</c> is extracted from the <c>Href</c>.
	/// </summary>
	[Parameter] public string Title { get; set; }

	/// <summary>
	/// Determines the heading tag to be used. Level <c></c> should be used for sections and higher integers for subsections.
	/// </summary>
	[Parameter] public int Level { get; set; } = 2;

	[Parameter] public RenderFragment ChildContent { get; set; }

	[Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> AdditionalAttributes { get; set; }

	/// <summary>
	/// Tag for the section title (<see cref="Level"/> will be used if you won't set the parameter).
	/// </summary>
	[Parameter] public string HeadingTag { get; set; }

	[Inject] public IDocPageNavigationItemsHolder DocPageNavigationItemsHolder { get; set; }

	protected string IdEffective => Id ?? GetIdFromTitle();
	string IDocPageNavigationItem.Id => IdEffective;

	protected string HeadingTagEffective => HeadingTag ?? (LevelHeadingTags.ContainsKey(Level) ? LevelHeadingTags[Level] : LevelHeadingTags.Values.LastOrDefault());

	protected override void OnParametersSet()
	{
		AdditionalAttributes ??= new Dictionary<string, object>();
		AdditionalAttributes["id"] = IdEffective;

		DocPageNavigationItemsHolder?.RegisterNew(this, NavigationManager.Uri);
	}

	private string GetIdFromTitle()
	{
		if (String.IsNullOrWhiteSpace(Title))
		{
			return null;
		}
		return Regex.Replace(Title.ToLower(), @"[^A-Za-z]+", "-").Trim('-');
	}

	public string GetItemUrl(string currentUrl)
	{
		string uri = currentUrl.Split('?')[0];
		uri = uri.Split('#')[0];

		return $"{uri}#{IdEffective}";
	}

	private string GetItemUrl()
	{
		return GetItemUrl(NavigationManager.Uri);
	}
}
