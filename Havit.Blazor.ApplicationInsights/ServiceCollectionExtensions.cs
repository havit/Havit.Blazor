using Havit.Blazor.ApplicationInsights.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Extension methods for registering Blazor Application Insights services.
/// </summary>
public static class ServiceCollectionExtensions
{
	/// <summary>
	/// Registers Blazor Application Insights services and configures the options.
	/// </summary>
	public static IServiceCollection AddBlazorApplicationInsights(this IServiceCollection services, Action<BlazorApplicationInsightsOptions> configureAction = null)
	{
		if (OperatingSystem.IsBrowser())
		{
			services.TryAddScoped<IBlazorApplicationInsights, BrowserBlazorApplicationInsights>();
		}
		else
		{
			services.TryAddScoped<BrowserBlazorApplicationInsights>();
			services.TryAddScoped<IBlazorApplicationInsights, AdaptiveBlazorApplicationInsights>();
		}

		if (configureAction != null)
		{
			services.Configure(configureAction);
		}

		return services;
	}
}
