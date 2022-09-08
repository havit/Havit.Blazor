using Havit.Blazor.Components.Web.Bootstrap;

namespace BlazorAppTest.Pages;

public class MyInputText : HxInputText
{
	protected override string FormatValueAsString(string value)
	{
		if (value == "24h")
		{
			return "24:00:00";
		}
		return value;
	}
}
