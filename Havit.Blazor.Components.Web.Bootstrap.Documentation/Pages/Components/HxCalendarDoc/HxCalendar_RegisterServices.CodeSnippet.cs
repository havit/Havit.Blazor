using Havit.Blazor.Components.Web;							// <------ ADD THIS LINE
using Havit.Blazor.Components.Web.Bootstrap;				// <------ ADD THIS LINE
using System;												// <------ ADD THIS LINE

public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);
	builder.RootComponents.Add<App>("app");

	// ... shortened for brevity

	// So the provider can access the context
	builder.Services.AddHttpContextAccessor();				// <------ ADD THIS LINE

	// Do this before AddHxServices
	builder.Services.AddScoped<TimeProvider>(provider =>	// <------ ADD THIS BLOCK
	{
		NavigationManager? navigationManager = provider.GetService<NavigationManager>();
		TimeZoneInfo ? requestTimeZoneInfo = null;
		try
		{
			var uri = navigationManager?.ToAbsoluteUri(navigationManager.Uri);
			if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("tz", out var vals))
			{
				requestTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(vals.FirstOrDefault() ?? "Not Found");
			}
		}
		catch (Exception)
		{
			// Ignore and fallback to local timezone
		}
		requestTimeZoneInfo ??= TimeZoneInfo.Local;
		return ZonedTimeProvider(requestTimeZoneInfo);
	});

	builder.Services.AddHxServices();						// <------ ADD THIS LINE

	await builder.Build().RunAsync();
}