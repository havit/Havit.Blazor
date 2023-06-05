using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.DemoData;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components.DocColorMode;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation;

public class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);

		var cultureInfo = new CultureInfo("en-US");
		CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
		CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

		builder.Services.AddHxServices();
		builder.Services.AddHxMessenger();
		builder.Services.AddHxMessageBoxHost();

		builder.Services.AddTransient<IComponentApiDocModelBuilder, ComponentApiDocModelBuilder>();
		builder.Services.AddSingleton<IDocXmlProvider, DocXmlProvider>();
		builder.Services.AddSingleton<IDocPageNavigationItemsHolder, DocPageNavigationItemsHolder>();
		builder.Services.AddSingleton<IDocColorModeResolver, DocColorModeClientResolver>();

		builder.Services.AddTransient<IDemoDataService, DemoDataService>();

		await builder.Build().RunAsync();
	}
}
