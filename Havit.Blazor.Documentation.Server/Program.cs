using System.Globalization;
using Havit.Blazor.Documentation.DemoData;
using Havit.Blazor.Documentation.Server.Services;
using Havit.Blazor.Documentation.Services;
using Havit.Blazor.Documentation.Shared.Components.DocColorMode;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartComponents.Inference.OpenAI;
using SmartComponents.LocalEmbeddings;

namespace Havit.Blazor.Documentation.Server;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

#if DEBUG
		builder.Configuration.AddJsonFile($"appsettings.Development.local.json", optional: true);
#endif

		// enforce en-US culture
		var cultureInfo = new CultureInfo("en-US");
		CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
		CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

		// Add services to the container.
		builder.Services.AddRazorComponents()
			.AddInteractiveWebAssemblyComponents();
		builder.Services.AddControllers();

		builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
		builder.Services.AddSingleton<IHttpContextProxy, ServerHttpContextProxy>();

		builder.Services.AddHxServices();
		builder.Services.AddHxMessenger();
		builder.Services.AddHxMessageBoxHost();

		builder.Services.AddSmartComponents()
			.WithInferenceBackend<OpenAIInferenceBackend>();
		builder.Services.AddSingleton<LocalEmbedder>();

		builder.Services.AddTransient<IComponentApiDocModelBuilder, ComponentApiDocModelBuilder>();
		builder.Services.AddSingleton<IDocXmlProvider, DocXmlProvider>();
		builder.Services.AddSingleton<IDocPageNavigationItemsTracker, DocPageNavigationItemsTracker>();
		builder.Services.AddTransient<IDocColorModeResolver, DocColorModeServerResolver>();

		builder.Services.AddTransient<IDemoDataService, DemoDataService>();

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
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

			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			// app.UseHsts();
		}

		app.UseHttpsRedirection();

		app.UseAntiforgery();

		// SmartComboBox
		var embedder = app.Services.GetRequiredService<LocalEmbedder>();
		var expenseCategories = embedder.EmbedRange(
			["Groceries", "Utilities", "Rent", "Mortgage", "Car Payment", "Car Insurance", "Health Insurance", "Life Insurance", "Home Insurance", "Gas", "Public Transportation", "Dining Out", "Entertainment", "Travel", "Clothing", "Electronics", "Home Improvement", "Gifts", "Charity", "Education", "Childcare", "Pet Care", "Other"]);
		var issueLabels = embedder.EmbedRange(
			["Bug", "Docs", "Enhancement", "Question", "UI (Android)", "UI (iOS)", "UI (Windows)", "UI (Mac)", "Performance", "Security", "Authentication", "Accessibility"]);
		app.MapSmartComboBox("/api/SmartComboBox/expense-category",
			request => embedder.FindClosest(request.Query, expenseCategories));

		app.MapSmartComboBox("/api/SmartComboBox/issue-label",
			request => embedder.FindClosest(request.Query, issueLabels));


		app.MapStaticAssets();
		app.MapControllers();
		app.MapRazorComponents<App>()
			.AddInteractiveWebAssemblyRenderMode()
			.AddAdditionalAssemblies(typeof(Havit.Blazor.Documentation._Imports).Assembly);

		app.Run();
	}
}
