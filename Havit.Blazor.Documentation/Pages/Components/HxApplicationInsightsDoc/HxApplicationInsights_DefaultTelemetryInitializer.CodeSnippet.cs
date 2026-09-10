builder.Services.AddBlazorApplicationInsights(options =>
{
	options.JsSdkOptions.ConnectionString = "...";
	options.DefaultTelemetryInitializer = new TelemetryInitializer
	{
		CloudRoleName = "MyBlazorApp",
		ApplicationVersion = "1.2.3"
	};
});
