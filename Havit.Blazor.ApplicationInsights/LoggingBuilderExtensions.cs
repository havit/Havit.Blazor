using Havit.Blazor.ApplicationInsights.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;

namespace Havit.Blazor.ApplicationInsights;

/// <summary>
/// Extension methods for registering Blazor Application Insights logging provider.
/// </summary>
public static class LoggingBuilderExtensions
{
	/// <summary>
	/// Registers Blazor Application Insights log provider.
	/// </summary>
	public static ILoggingBuilder AddBlazorApplicationInsights(this ILoggingBuilder builder)
	{
		builder.AddConfiguration();
		builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, BlazorApplicationInsightsLoggerProvider>());
		builder.Services.AddBlazorApplicationInsights();
		return builder;
	}
}
