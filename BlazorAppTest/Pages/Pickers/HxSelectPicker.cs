using System.Globalization;

namespace BlazorAppTest.Pages.Pickers;

public class HxSelectPicker : HxSelectBase<int?, CultureInfo>
{
	public HxSelectPicker()
	{
		TextSelectorImpl = (ci => ci.EnglishName);
		ValueSelectorImpl = (ci => ci.LCID);
		NullDataTextImpl = "Null data";
		NullTextImpl = "null text";
	}

	protected override async Task OnInitializedAsync()
	{
		await Task.Delay(1000);

		DataImpl = CultureInfo.GetCultures(CultureTypes.SpecificCultures)
							.Where(c => c.LCID != 4096) // see Remarks: https://docs.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.lcid#System_Globalization_CultureInfo_LCID
							.OrderBy(c => c.EnglishName)
							.Take(100)
							.ToList();
	}
}
