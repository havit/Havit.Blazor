using Havit.Blazor.ApplicationInsights.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.ApplicationInsights;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddBlazorApplicationInsights(this IServiceCollection services, Action<BlazorApplicationInsightsClientOptions> configureAction)
	{
		services.AddScoped<IBlazorApplicationInsights, BrowserBlazorApplicationInsights>();
		services.Configure(configureAction);

		return services;
	}
}
