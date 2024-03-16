using System.Globalization;
using Havit.Blazor.Documentation.DemoData;
using Havit.Blazor.Documentation.Services;
using Havit.Blazor.Documentation.Shared.Components.DocColorMode;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartComponents.Inference.OpenAI;
using SmartComponents.LocalEmbeddings;

namespace Havit.Blazor.Documentation.Server;

public class Startup
{
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddRazorPages();
		services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

		services.AddHxServices();
		services.AddHxMessenger();
		services.AddHxMessageBoxHost();

		services.AddSmartComponents()
			.WithInferenceBackend<OpenAIInferenceBackend>();
		services.AddSingleton<LocalEmbedder>();

		services.AddTransient<IComponentApiDocModelBuilder, ComponentApiDocModelBuilder>();
		services.AddSingleton<IDocXmlProvider, DocXmlProvider>();
		services.AddSingleton<IDocPageNavigationItemsHolder, DocPageNavigationItemsHolder>();
		services.AddTransient<IDocColorModeResolver, DocColorModeServerResolver>();

		services.AddTransient<IDemoDataService, DemoDataService>();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		var cultureInfo = new CultureInfo("en-US");
		CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
		CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
			app.UseWebAssemblyDebugging();
		}
		else
		{
			app.UseExceptionHandler("/Error");

			// old domain redirect
			app.Use(async (context, next) =>
			{

				if (context.Request.Host.Host.Contains("havit.blazor.cz"))
				{
					var uriBuilder = new UriBuilder(UriHelper.GetDisplayUrl(context.Request));
					uriBuilder.Host = "havit.blazor.eu";
					context.Response.Redirect(uriBuilder.Uri.ToString(), permanent: true);

					return;
				}

				await next();
			});
		}

		app.UseBlazorFrameworkFiles();
		app.UseStaticFiles();

		app.UseRouting();

		// SmartComboBox
		var embedder = app.ApplicationServices.GetRequiredService<LocalEmbedder>();
		var expenseCategories = embedder.EmbedRange(
			["Groceries", "Utilities", "Rent", "Mortgage", "Car Payment", "Car Insurance", "Health Insurance", "Life Insurance", "Home Insurance", "Gas", "Public Transportation", "Dining Out", "Entertainment", "Travel", "Clothing", "Electronics", "Home Improvement", "Gifts", "Charity", "Education", "Childcare", "Pet Care", "Other"]);
		var issueLabels = embedder.EmbedRange(
			["Bug", "Docs", "Enhancement", "Question", "UI (Android)", "UI (iOS)", "UI (Windows)", "UI (Mac)", "Performance", "Security", "Authentication", "Accessibility"]);

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapSmartComboBox("/api/SmartComboBox/expense-category",
				request => embedder.FindClosest(request.Query, expenseCategories));

			endpoints.MapSmartComboBox("/api/SmartComboBox/issue-label",
				request => embedder.FindClosest(request.Query, issueLabels));

			endpoints.MapRazorPages();
			endpoints.MapControllers();
			endpoints.MapFallbackToPage("/_Host");
		});
	}
}