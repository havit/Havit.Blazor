namespace Havit.Blazor.TestApp.Client;

public static class DependencyInjectionExtensions
{
	public static IServiceCollection AddClientServices(this IServiceCollection services)
	{
		services.AddHxServices();

		services.AddTransient<IDemoDataService, DemoDataService>();
		services.AddGeneratedResourceWrappers();


		return services;
	}
}
