using Havit.Blazor.ApplicationInsights.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.ApplicationInsights;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddBlazorApplicationInsights(this IServiceCollection services, Action<BlazorApplicationInsightsClientOptions> configureAction)
	{
		if (OperatingSystem.IsBrowser())
		{
			services.AddScoped<IBlazorApplicationInsights, BrowserBlazorApplicationInsights>();
		}
		else
		{
			services.AddScoped<BrowserBlazorApplicationInsights>();
			services.AddScoped<IBlazorApplicationInsights, AdaptiveBlazorApplicationInsights>();
		}

		services.Configure(configureAction);

		return services;
	}
}
