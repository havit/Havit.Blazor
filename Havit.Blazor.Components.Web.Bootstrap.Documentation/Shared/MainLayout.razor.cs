using System;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared
{
	public partial class MainLayout
	{
		private const string TitleBase = "HAVIT Blazor - Free Bootstrap 5 components";
		private const string TitleSeparator = " | ";

		[Inject] protected NavigationManager NavigationManager { get; set; }

		private string title;

		protected override void OnParametersSet()
		{
			var path = new Uri(this.NavigationManager.Uri).AbsolutePath.TrimEnd('/');

			// TODO Temp, replace with .NET 6 HeadContent, duplicates _Host.cshtml.cs:OnGet

			var lastSegmentStart = path.LastIndexOf("/");
			if (lastSegmentStart > 0)
			{
				title = path.Substring(lastSegmentStart + 1) + TitleSeparator + TitleBase;
				return;
			}

			title = "HAVIT Blazor | Free Bootstrap 5 components for Blazor";
		}
	}
}
