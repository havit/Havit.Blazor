await AppInsights.AddTelemetryInitializerAsync(new TelemetryInitializer
{
	CloudRoleName = "MyBlazorApp",
	ApplicationVersion = "1.2.3"
});
