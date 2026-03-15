using Havit.Blazor.TestApp.Components;
using Havit.Blazor.TestApp.Client;
using Havit.Blazor.TestApp.MinimalApi;

namespace Havit.Blazor.TestApp;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.
		builder.Services.AddHttpContextAccessor();
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents()
			.AddInteractiveWebAssemblyComponents();

		builder.Services.AddAntiforgery(options =>
		{
			options.HeaderName = "X-Custom-CSRF-Token";
		});

		builder.Services.AddClientServices();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseWebAssemblyDebugging();
		}
		else
		{
			app.UseExceptionHandler("/Error", createScopeForErrors: true);
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseFileUploadEndpoint();

		app.MapStaticAssets();
		app.UseAntiforgery();

		app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode()
			.AddInteractiveWebAssemblyRenderMode()
			.AddAdditionalAssemblies(typeof(Havit.Blazor.TestApp.Client._Imports).Assembly);

		app.Run();
	}
}
