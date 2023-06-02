using System.Globalization;
using Havit.Blazor.Components.Web.Bootstrap;

namespace BlazorAppTest.Pages.Pickers;

public class HxSelectPicker : HxSelectBase<int?, CultureInfo>
{
	public HxSelectPicker()
	{
		this.TextSelectorImpl = (ci => ci.EnglishName);
		this.ValueSelectorImpl = (ci => ci.LCID);
		this.NullDataTextImpl = "Null data";
		this.NullTextImpl = "null text";
	}

	protected override async Task OnInitializedAsync()
	{
		await Task.Delay(1000);

		this.DataImpl = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
							.Where(c => c.LCID != 4096) // see Remarks: https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.lcid#System_Globalization_CultureInfo_LCID
							.OrderBy(c => c.EnglishName)
							.Take(100)
							.ToList();
	}
}
