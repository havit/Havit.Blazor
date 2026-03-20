using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Havit.Blazor.ApplicationInsights.TestApp.Client;

internal class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);

		builder.Logging.AddBlazorApplicationInsights();
		builder.Services.AddBlazorApplicationInsights(options => options.JsSdkOptions.ConnectionString = ConnectionStrings.ApplicationInsights);

		await builder.Build().RunAsync();
	}
}
