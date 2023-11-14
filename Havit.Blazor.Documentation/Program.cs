using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using Havit.Blazor.Documentation.Services;
using Havit.Blazor.Documentation.DemoData;
using Havit.Blazor.Documentation.Shared.Components.DocColorMode;

namespace Havit.Blazor.Documentation;

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
