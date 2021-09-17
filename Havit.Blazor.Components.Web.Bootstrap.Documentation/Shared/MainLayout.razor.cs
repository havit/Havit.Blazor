using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Extensions.Head;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared
{
	public partial class MainLayout
	{
		private const string TitleBase = "HAVIT Blazor Documentation";
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

			title = TitleBase + TitleSeparator + "Free Bootstrap 5 Blazor components";
		}
	}
}
