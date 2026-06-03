builder.Services.AddBlazorApplicationInsights(options =>
{
	options.JsSdkOptions.ConnectionString = "your-connection-string";
});
