using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Havit.Blazor.Components.Web;

public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Adds services needed for HAVIT Blazor library.
	/// </summary>
	public static IServiceCollection AddHxServices(this IServiceCollection services)
	{
		services.AddLocalization();
		services.TryAddSingleton(TimeProvider.System);

		return services;
	}
}
