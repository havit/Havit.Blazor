using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Server.Pages
{
	public class _Host : PageModel
	{
		private const string TitleBase = "HAVIT Blazor Documentation";
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

			Title = TitleBase + TitleSeparator + "Free Bootstrap 5 Blazor components";
		}

	}
}
