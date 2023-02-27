using System.Text.Json;

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
			var result = await client.GetStreamAsync("https://bootswatch.com/api/5.json");
			var themesHolder = await JsonSerializer.DeserializeAsync<ThemeHolder>(result, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
			themes = themesHolder.Themes;
			themes.ForEach(t => t.Name += " (bootswatch.com)");
		}
		catch
		{
			Console.WriteLine("Unable to fetch themes from Bootswatch API.");
			themes = new();
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
