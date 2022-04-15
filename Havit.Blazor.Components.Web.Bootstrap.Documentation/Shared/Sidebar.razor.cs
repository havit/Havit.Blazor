using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared;

public partial class Sidebar
{
	private static readonly HttpClient client = new HttpClient();

	private List<Theme> themes = new();
	private Theme selectedTheme;

	protected override async Task OnInitializedAsync()
	{
		try
		{
			var result = await client.GetStringAsync("https://bootswatch.com/api/5.json");
			themes = JsonConvert.DeserializeObject<ThemeHolder>(result).Themes;
		}
		catch
		{
			Console.WriteLine("Unable to fetch themes from Bootswatch API.");
		}

		themes = themes.Prepend(new() { Name = "Bootstrap 5", CssCdn = "FULL_LINK_HARDCODED_IN_RAZOR" }).ToList();
	}
}

public class ThemeHolder
{
	public List<Theme> Themes { get; set; }
}

public class Theme
{
	public string Name { get; set; }
	public string CssCdn { get; set; }
}
