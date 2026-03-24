using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Havit.Blazor.ApplicationInsights.TestApp.Client;

internal class Program
{
	public static async Task Main(string[] args)
	{
		var builder = WebAssemblyHostBuilder.CreateDefault(args);

		// Reads logging configuration from configuration providers (appsettings.json).
		builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));

		builder.Logging.AddBlazorApplicationInsights();
		builder.Services.AddBlazorApplicationInsights(options =>
		{
			options.JsSdkOptions.ConnectionString = ConnectionStrings.ApplicationInsights;
			options.DefaultTelemetryInitializer = new Havit.Blazor.ApplicationInsights.Telemetry.TelemetryInitializer
			{
				CloudRoleName = TestDefaults.DefaultTelemetryInitializerCloudRoleName
			};
		});

		await builder.Build().RunAsync();
	}
}
