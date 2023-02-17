using System.Globalization;
using Havit.Blazor.Components.Web.Bootstrap;

namespace BlazorAppTest.Pages;

public partial class HxGrid_Issue470_Test
{
	private List<CultureInfo> localCultureInfos = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
	private string lcidSearchString = string.Empty, displayNameSearchString = string.Empty, nameSearchString = string.Empty, englishNameSearchString = string.Empty;

	private HxGrid<CultureInfo> grid;

	private Task<GridDataProviderResult<CultureInfo>> GetGridData(GridDataProviderRequest<CultureInfo> request)
	{
		// Remove items not containing the search strings. Where statements were separated to increase readability.
		var filteredCultureInfos = localCultureInfos?
			.Where(i => i.LCID.ToString().Contains(lcidSearchString))
			.Where(i => i.DisplayName?.Contains(displayNameSearchString) ?? true)
			.Where(i => i.Name?.Contains(nameSearchString) ?? true)
			.Where(i => i.EnglishName?.Contains(englishNameSearchString) ?? true) ?? new List<CultureInfo>();

		return Task.FromResult(request.ApplyTo(filteredCultureInfos));
	}

	private async Task RefreshData()
	{
		await grid.RefreshDataAsync();
	}
}
