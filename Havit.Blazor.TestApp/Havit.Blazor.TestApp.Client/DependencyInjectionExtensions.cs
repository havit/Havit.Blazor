using Havit.Blazor.GoogleTagManager;
using Havit.Blazor.Storage;

namespace Havit.Blazor.TestApp.Client;

public static class DependencyInjectionExtensions
{
	public static IServiceCollection AddClientServices(this IServiceCollection services)
	{
		services.AddHxServices();
		services.AddHxMessageBoxHost();
		services.AddHxMessenger();

		services.AddHavitBlazorStorage();

		services.AddHxGoogleTagManager(options => options.GtmId = "GTM-TESTID");

		services.AddTransient<IDemoDataService, DemoDataService>();
		services.AddGeneratedResourceWrappers();


		return services;
	}
}
