using System.Diagnostics;

namespace Havit.Blazor.Components.Web.ECharts;

public static class HxEChartsSetup
{
	/// <summary>it
	/// Renders the <c>&lt;script&gt;</c> tag that references the corresponding Apache ECharts JavaScript.<br/>
	/// To be used in <c>App.razor</c> as <c>@((MarkupString)HxSetup.RenderEChartsJavaScriptReference())</c>.
	/// </summary>
	/// <remarks>
	/// We do not want to use TagHelper or HTML Helper here as we do not want to introduce a dependency on server-side ASP.NET Core (MVC/Razor) to our library (a separate NuGet package would have to be created).
	/// </remarks>
	public static string RenderEChartsJavaScriptReference()
	{
		return """
			<script src="https://cdn.jsdelivr.net/npm/echarts@5.6.0/dist/echarts.min.js" integrity="sha256-v0oiNSTkC3fDBL7GfhIiz1UfFIgM9Cxp3ARlWOEcB7E=" crossorigin="anonymous"></script>
			""";
	}
}
