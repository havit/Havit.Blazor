using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared;

public partial class Sidebar
{
	private static readonly HttpClient client = new HttpClient();

	private List<Theme> themes = new()
	{
		new() { Name = "Bootstrap 5", CssCdn = "https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css" }
	};
	private Theme selectedTheme;

	protected override async Task OnInitializedAsync()
	{
		var result = await client.GetStringAsync("https://bootswatch.com/api/5.json");
		themes = JsonConvert.DeserializeObject<ThemeHolder>(result).Themes;
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
