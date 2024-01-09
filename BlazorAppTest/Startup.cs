using System.Globalization;
using BlazorAppTest.Resources;
using Havit.Blazor.Components.Web;
using Havit.Blazor.Documentation.DemoData;
using Havit.Blazor.GoogleTagManager;

namespace BlazorAppTest;

public class Startup
{
	public Startup(IConfiguration configuration)
	{
		Configuration = configuration;
	}

	public IConfiguration Configuration { get; }

	// This method gets called by the runtime. Use this method to add services to the container.
	// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
	public void ConfigureServices(IServiceCollection services)
	{
		services.AddLogging();

		services.AddLocalization();
		services.AddGeneratedResourceWrappers();

		services.AddRazorPages();
		services.AddServerSideBlazor();

		services.AddHxServices();
		services.AddHxMessenger();
		services.AddHxMessageBoxHost();
		services.AddHxGoogleTagManager(options =>
		{
			options.GtmId = "GTM-W2CT4P6"; // Havit.Blazor.GoogleTagManager DEV test
		});

		services.AddTransient<IDemoDataService, DemoDataService>();

		// TESTs for Defaults
		//HxAutosuggest.Defaults.InputSize = InputSize.Large;
		//HxInputText.Defaults.InputSize = InputSize.Large;
		//HxInputNumber.Defaults.InputSize = InputSize.Large;
		//HxSelect.Defaults.InputSize = InputSize.Large;
		//HxInputDate.Defaults.InputSize = InputSize.Large;
		//HxInputDateRange.Defaults.InputSize = InputSize.Large;
		//HxCalendar.Defaults.DateCustomizationProvider = request => new CalendarDateCustomizationResult { Enabled = request.Date < DateTime.Today };
		//HxMessageBox.Defaults.ModalSettings.Centered = true;
	}

	// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
	public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
	{
		if (env.IsDevelopment())
		{
			app.UseDeveloperExceptionPage();
		}
		else
		{
			app.UseExceptionHandler("/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseHttpsRedirection();
		app.UseStaticFiles();
		app.UseRequestLocalization(o =>
		{
			CultureInfo cs = new CultureInfo("cs-CZ");
			o.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(cs);
			o.AddSupportedCultures("cs", "en-US");
			o.AddSupportedUICultures("cs", "en-US");
		});

		app.UseRouting();

		app.UseEndpoints(endpoints =>
		{
			endpoints.MapBlazorHub();
			endpoints.MapControllers();
			endpoints.MapFallbackToPage("/_Host");
		});
	}
}
