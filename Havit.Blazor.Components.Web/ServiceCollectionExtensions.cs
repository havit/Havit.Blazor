using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddHxServices(this IServiceCollection services)
		{
			services.AddLocalization();

			return services;
		}
	}
}
