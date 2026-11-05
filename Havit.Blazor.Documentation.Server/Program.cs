using System.Globalization;
using Havit.Blazor.Documentation.DemoData;
using Havit.Blazor.Documentation.Pages.Showcase.Data;
using Havit.Blazor.Documentation.Server.Services;
using Havit.Blazor.Documentation.Services;
using Havit.Blazor.Documentation.Shared.Components.DocColorMode;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SmartComponents.Inference.OpenAI;
using SmartComponents.LocalEmbeddings;

namespace Havit.Blazor.Documentation.Server;

public class Program
{
	/// <summary>
	/// Doc pages of components and types that were renamed or removed in v6 (Bootstrap 6), redirected to their v6 successors
	/// so that external links to the v4 documentation keep working. Matched case-insensitively against the request path.
	/// </summary>
	private static readonly Dictionary<string, string> s_removedPageRedirects = new(StringComparer.OrdinalIgnoreCase)
	{
		// HxModal → HxDialog
		["/components/HxModal"] = "/components/HxDialog",
		["/types/HxModal"] = "/components/HxDialog",
		["/types/ModalSettings"] = "/types/DialogSettings",
		["/types/Modal Settings"] = "/types/DialogSettings", // v4 search catalog entry
		["/types/ModalSize"] = "/types/DialogSize",
		["/types/ModalFullscreen"] = "/types/DialogFullscreen",
		["/types/ModalBackdrop"] = "/types/DialogBackdrop",
		["/types/ModalRenderMode"] = "/types/DialogRenderMode",
		["/types/ModalHidingEventArgs"] = "/types/DialogHidingEventArgs",
		["/types/CloseButtonSettings"] = "/components/HxCloseButton",

		// HxOffcanvas → HxDrawer
		["/components/HxOffcanvas"] = "/components/HxDrawer",
		["/types/HxOffcanvas"] = "/components/HxDrawer",
		["/types/OffcanvasSettings"] = "/types/DrawerSettings",
		["/types/OffcanvasPlacement"] = "/types/DrawerPlacement",
		["/types/OffcanvasSize"] = "/types/DrawerSize",
		["/types/OffcanvasBackdrop"] = "/types/DrawerBackdrop",
		["/types/OffcanvasRenderMode"] = "/types/DrawerRenderMode",
		["/types/OffcanvasResponsiveBreakpoint"] = "/types/DrawerResponsiveBreakpoint",
		["/types/OffcanvasHidingEventArgs"] = "/types/DrawerHidingEventArgs",

		// HxNavbarCollapse → HxNavbarDrawer
		["/components/HxNavbarCollapse"] = "/components/HxNavbarDrawer",
		["/types/HxNavbarCollapse"] = "/components/HxNavbarDrawer",

		// HxDropdown → HxMenu
		["/components/HxDropdown"] = "/components/HxMenu",
		["/components/HxDropdownToggle"] = "/components/HxMenuToggleButton",
		["/components/HxDropdownButtonGroup"] = "/components/HxMenu",
		["/components/HxDropdownToggleButton"] = "/components/HxMenuToggleButton",
		["/components/HxDropdownToggleElement"] = "/components/HxMenuToggleElement",
		["/components/HxDropdownMenu"] = "/components/HxMenu",
		["/components/HxDropdownContent"] = "/components/HxMenu",
		["/components/HxDropdownItem"] = "/components/HxMenuItem",
		["/components/HxDropdownHeader"] = "/components/HxMenuHeader",
		["/components/HxDropdownDivider"] = "/components/HxMenuDivider",
		["/types/HxDropdown"] = "/components/HxMenu",
		["/types/HxDropdownButtonGroup"] = "/components/HxMenu",
		["/types/HxDropdownToggleButton"] = "/components/HxMenuToggleButton",
		["/types/HxDropdownToggleElement"] = "/components/HxMenuToggleElement",
		["/types/HxDropdownMenu"] = "/components/HxMenu",
		["/types/HxDropdownContent"] = "/components/HxMenu",
		["/types/HxDropdownItem"] = "/components/HxMenuItem",
		["/types/HxDropdownItemNavLink"] = "/components/HxMenuItemNavLink",
		["/types/HxDropdownItemText"] = "/components/HxMenuText",
		["/types/HxDropdownHeader"] = "/components/HxMenuHeader",
		["/types/HxDropdownDivider"] = "/components/HxMenuDivider",
		["/types/DropdownAutoClose"] = "/types/MenuAutoClose",
		["/types/DropdownDirection"] = "/types/MenuPlacement",
		["/types/DropdownMenuAlignment"] = "/types/MenuPlacement",
		["/types/DropdownMenuAlignmentExtensions"] = "/types/MenuPlacementExtensions",
		["/types/DropdownHidingEventArgs"] = "/types/MenuHidingEventArgs",
		["/types/IDropdownContainer"] = "/types/IMenuContainer",
		["/types/PopperStrategy"] = "/types/FloatingStrategy",

		// HxCarousel
		["/types/HxCarouselCaption"] = "/components/HxCarousel",
		["/types/CarouselRide"] = "/components/HxCarousel",

		// HxSidebar
		["/types/SidebarResponsiveBreakpoint"] = "/components/HxSidebar",
		["/types/SidebarResponsiveBreakpointExtensions"] = "/components/HxSidebar",
	};

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

		builder.Services.AddSingleton<IShowcaseDataService, ShowcaseDataService>();
		builder.Services.AddSingleton<IApiDocModelBuilder, ApiDocModelBuilder>();
		builder.Services.AddSingleton<IApiDocModelProvider, ApiDocModelProvider>();
		builder.Services.AddSingleton<IDocXmlProvider, DocXmlProvider>();
		builder.Services.AddSingleton<IDocumentationCatalogService, DocumentationCatalogService>();
		builder.Services.AddSingleton<IDocPageNavigationItemsTracker, DocPageNavigationItemsTracker>();

		builder.Services.AddScoped<IDocColorModeProvider, DocColorModeProvider>();
		builder.Services.AddCascadingValue<ColorMode>(services =>
		{
			var docColorModeStateProvider = services.GetRequiredService<IDocColorModeProvider>();
			return new DocColorModeCascadingValueSource(docColorModeStateProvider);
		});

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

		// v4 doc pages renamed or removed in v6 (see s_removedPageRedirects)
		app.Use(async (context, next) =>
		{
			if (context.Request.Path.HasValue
				&& s_removedPageRedirects.TryGetValue(context.Request.Path.Value, out var newPath))
			{
				context.Response.Redirect(newPath + context.Request.QueryString, permanent: true);
				return;
			}

			await next();
		});

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
