using Havit.Blazor.Components.Web;            // <------ ADD THIS LINE
using Havit.Blazor.Components.Web.Bootstrap;  // <------ ADD THIS LINE

public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);
	builder.RootComponents.Add<App>("app");

	// ... shortened for brevity

	builder.Services.AddHxServices();        // <------ ADD THIS LINE

	await builder.Build().RunAsync();
}