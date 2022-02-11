using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

public partial class SectionTitle
{
	[Inject] public NavigationManager NavigationManager { get; set; }

	/// <summary>
	/// Id of the section.
	/// </summary>
	[Parameter] public string Id { get; set; }

	/// <summary>
	/// Title of the section. If not set, <c>Title</c> is extracted from the <c>Href</c>.
	/// </summary>
	[Parameter] public string Title { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	private string TitleEffective => Title ?? (ChildContent is null ? GetTitleFromHref() : string.Empty);

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

	private string GetSectionUrl()
	{
		string uri = NavigationManager.Uri;
		uri = uri.Split('?')[0];

		return $"{uri}#{Id}";
	}
}
