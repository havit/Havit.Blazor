using Havit.Blazor.Components.Web;

namespace Havit.Blazor.TestApp.Client;

public static class DependencyInjectionExtensions
{
	public static IServiceCollection AddClientServices(this IServiceCollection services)
	{
		services.AddHxServices();

		return services;
	}
}
