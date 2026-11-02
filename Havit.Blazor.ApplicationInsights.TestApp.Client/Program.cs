using Havit.Blazor.ApplicationInsights.Options;
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
			// the SDK remote (CDN) configuration enables gzip compression of the telemetry payload (since 2026-10),
			// the E2E tests intercept the track endpoint and deserialize the payload as plain JSON
			// (BlockCdnCfg: the CDN configuration must not be able to force the feature on again)
			options.JsSdkOptions.FeatureOptIn = new Dictionary<string, FeatureOptInDetails>
			{
				["zipPayload"] = new FeatureOptInDetails { Mode = FeatureOptInMode.Disable, BlockCdnCfg = true }
			};
			options.DefaultTelemetryInitializer = new Havit.Blazor.ApplicationInsights.Telemetry.TelemetryInitializer
			{
				CloudRoleName = TestDefaults.DefaultTelemetryInitializerCloudRoleName
			};
		});

		await builder.Build().RunAsync();
	}
}
