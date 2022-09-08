using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
