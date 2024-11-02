﻿using Havit.Blazor.Documentation.Shared.Components;
using static System.Net.WebRequestMethods;

namespace Havit.Blazor.Documentation.Shared;

public partial class MainLayout
{
	private const string TitleBase = "HAVIT Blazor Bootstrap - Free components for ASP.NET Core Blazor";
	private const string TitleSeparator = " | ";

	[Inject] protected NavigationManager NavigationManager { get; set; }

	private string _title;

	private PageCanonicalUrlTracker _pageCanonicalUrlTracker;

	protected override void OnInitialized()
	{
		_pageCanonicalUrlTracker = new PageCanonicalUrlTracker(NavigationManager);
	}

	protected override void OnParametersSet()
	{
		var path = new Uri(NavigationManager.Uri).AbsolutePath.TrimEnd('/');

		var lastSegmentStart = path.LastIndexOf("/");
		if (lastSegmentStart > 0)
		{
			_title = path.Substring(lastSegmentStart + 1) + TitleSeparator + TitleBase;
			return;
		}

		_title = "HAVIT Blazor | Free Bootstrap 5 components for Blazor";
	}
}
