using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.GoogleTagManager;

public static class GoogleTagManagerServiceCollectionExtensions
{
	/// <summary>
	/// Adds Google Tag Manager (GTM) support. Use <see cref="IHxGoogleTagManager"/> to push data to <c>dataLayer</c> and/or <see cref="HxGoogleTagManagerPageViewTracker"/> to track page-views.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="configureOptions"></param>
	/// <returns></returns>
	public static IServiceCollection AddHxGoogleTagManager(this IServiceCollection services, Action<HxGoogleTagManagerOptions> configureOptions)
	{
		if (OperatingSystem.IsBrowser())
		{
			services.AddSingleton<IHxGoogleTagManager, HxGoogleTagManager>();
		}
		else
		{
			services.AddScoped<IHxGoogleTagManager, HxGoogleTagManager>();
		}

		if (configureOptions != null)
		{
			services.Configure(configureOptions);
		}

		return services;
	}

}
