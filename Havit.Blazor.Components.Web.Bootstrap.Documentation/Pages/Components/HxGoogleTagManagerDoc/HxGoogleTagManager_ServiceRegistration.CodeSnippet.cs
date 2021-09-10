builder.Services.AddHxGoogleTagManager(options =>
{
	builder.Configuration.Bind("HxGoogleTagManager", options);
});