using Microsoft.AspNetCore.Components.Web.Extensions.Head;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Server.Pages
{
	public class _Host : PageModel
	{
		private const string TitleBase = "HAVIT Blazor - Free Bootstrap 5 components";
		private const string TitleSeparator = " | ";

		public string Title { get; set; }

		public void OnGet()
		{
			var path = this.Request.Path.ToString().TrimEnd('/');

			// TODO Temp, replace with .NET 6 HeadContent, duplicates MainLayout.razor.cs:OnParametersSet

			var lastSegmentStart = path.LastIndexOf("/");
			if (lastSegmentStart > 0)
			{
				Title = path.Substring(lastSegmentStart + 1) + TitleSeparator + TitleBase;
				return;

			}

			Title = "HAVIT Blazor | Free Bootstrap 5 components for Blazor";
		}

	}
}
