using Havit.Blazor.ApplicationInsights.TestApp.Client;
using Havit.Blazor.ApplicationInsights.TestApp.Components;

namespace Havit.Blazor.ApplicationInsights.TestApp;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents()
			.AddInteractiveWebAssemblyComponents();

		builder.Services.AddBlazorApplicationInsights(options =>
		{
			options.JsSdkOptions.ConnectionString = ConnectionStrings.ApplicationInsights;
			options.DefaultTelemetryInitializer = new Havit.Blazor.ApplicationInsights.Telemetry.TelemetryInitializer
			{
				CloudRoleName = TestDefaults.DefaultTelemetryInitializerCloudRoleName
			};
		});

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseWebAssemblyDebugging();
		}
		else
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
		app.UseHttpsRedirection();

		app.UseAntiforgery();

		app.MapStaticAssets();
		app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode()
			.AddInteractiveWebAssemblyRenderMode()
			.AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

		app.Run();
	}
}
