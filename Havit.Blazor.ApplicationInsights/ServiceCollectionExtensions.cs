using Havit.Blazor.ApplicationInsights.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Extension methods for registering Blazor Application Insights services.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Registers Blazor Application Insights services and configures the options.
	/// </summary>
	public static IServiceCollection AddBlazorApplicationInsights(this IServiceCollection services, Action<BlazorApplicationInsightsOptions> configureAction)
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
