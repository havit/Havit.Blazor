using Havit.Blazor.TestApp.Components;
using Havit.Blazor.TestApp.Client;
using Havit.Blazor.TestApp.MinimalApi;

namespace Havit.Blazor.TestApp;

public class Program
{
	public const string CspNonceHttpContextItemKey = "CspNonce";

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

		// Strict (nonce-based) Content Security Policy for /CspTest pages (or any page with ?csp query string), see #1730.
		app.Use(async (context, next) =>
		{
			if (context.Request.Path.StartsWithSegments("/CspTest") || context.Request.Query.ContainsKey("csp"))
			{
				var nonce = Convert.ToBase64String(System.Security.Cryptography.RandomNumberGenerator.GetBytes(16));
				context.Items[CspNonceHttpContextItemKey] = nonce;
				context.Response.Headers.ContentSecurityPolicy = $"script-src 'self' 'wasm-unsafe-eval' 'nonce-{nonce}' https://cdn.jsdelivr.net";
			}
			await next();
		});

		app.MapStaticAssets();
		app.UseAntiforgery();

		app.MapRazorComponents<App>()
			.AddInteractiveServerRenderMode()
			.AddInteractiveWebAssemblyRenderMode()
			.AddAdditionalAssemblies(typeof(Havit.Blazor.TestApp.Client._Imports).Assembly);

		app.Run();
	}
}
