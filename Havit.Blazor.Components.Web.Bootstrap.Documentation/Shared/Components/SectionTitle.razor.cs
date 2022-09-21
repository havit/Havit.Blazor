using System.Text.RegularExpressions;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

public partial class SectionTitle
{
	/// <summary>
	/// Which heading tags are to be used for which levels.
	/// </summary>
	protected static readonly Dictionary<int, string> LevelHeadingTags = new()
	{
		{ 0, "h3" },
		{ 1, "h4" },
		{ 2, "h5" }
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
	/// Determines the heading tag to be used. Level <c>0</c> should be used for sections and higher integers for subsections.
	/// </summary>
	[Parameter] public int Level { get; set; } = 0;

	[Parameter] public RenderFragment ChildContent { get; set; }

	[Inject] public ISectionTitleHolder SectionTitleHolder { get; set; }

	public string TitleEffective => Title ?? (ChildContent is null ? GetTitleFromHref() : string.Empty);
	protected string HeadingTagEffective => LevelHeadingTags.ContainsKey(Level) ? LevelHeadingTags[Level] : LevelHeadingTags.Values.LastOrDefault();

	protected override void OnParametersSet()
	{
		SectionTitleHolder?.RegisterNew(this, NavigationManager.Uri);
	}

	private string GetTitleFromHref()
	{
		string result = string.Empty;

		string[] splitHref = Regex.Split(Id, @"(?<!^)(?=[A-Z])");

		if (splitHref is null || splitHref.Length == 0)
		{
			return string.Empty;
		}

		result += splitHref[0];
		for (int i = 1; i < splitHref.Length; i++)
		{
			result += $" {splitHref[i].ToLower()}";
		}

		return result;
	}

	public string GetSectionUrl()
	{
		string uri = NavigationManager.Uri;
		uri = uri.Split('?')[0];
		uri = uri.Split('#')[0];

		return $"{uri}#{Id}";
	}
}
