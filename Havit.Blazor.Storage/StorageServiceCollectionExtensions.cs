using System.Text.Json;
using Havit.Blazor.Storage.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Havit.Blazor.Storage;

/// <summary>
/// <see cref="IServiceCollection"/> extensions for registering the browser storage services
/// (<see cref="ILocalStorageService"/> and <see cref="ISessionStorageService"/>).
/// </summary>
public static class StorageServiceCollectionExtensions
{
	/// <summary>
	/// Registers <see cref="ILocalStorageService"/> and <see cref="ISessionStorageService"/> providing access to the
	/// browser <c>localStorage</c> and <c>sessionStorage</c>.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="configureOptions">
	/// An optional delegate to configure the <see cref="StorageServiceOptions"/> (e.g. the default
	/// <see cref="JsonSerializerOptions"/> used by <c>SetValue</c>/<c>GetValue</c> when no options are passed to the call).
	/// </param>
	/// <param name="lifetime">
	/// The lifetime to use for the registered services. The default is <see cref="ServiceLifetime.Scoped"/>.
	/// </param>
	public static IServiceCollection AddHavitBlazorStorage(
		this IServiceCollection services,
		Action<StorageServiceOptions> configureOptions = null,
		ServiceLifetime lifetime = ServiceLifetime.Scoped)
	{
		services.TryAdd(new ServiceDescriptor(typeof(BrowserStorageAccessor), typeof(BrowserStorageAccessor), lifetime));
		services.TryAdd(new ServiceDescriptor(typeof(ILocalStorageService), typeof(LocalStorageService), lifetime));
		services.TryAdd(new ServiceDescriptor(typeof(ISessionStorageService), typeof(SessionStorageService), lifetime));

		if (configureOptions != null)
		{
			services.Configure(configureOptions);
		}

		return services;
	}

	/// <summary>
	/// Registers <see cref="ILocalStorageService"/> and <see cref="ISessionStorageService"/> providing access to the
	/// browser <c>localStorage</c> and <c>sessionStorage</c>.
	/// </summary>
	/// <param name="services">The service collection.</param>
	/// <param name="jsonSerializerOptions">
	/// The default <see cref="JsonSerializerOptions"/> used by <c>SetValue</c>/<c>GetValue</c> when no options are passed to the call.
	/// </param>
	/// <param name="lifetime">
	/// The lifetime to use for the registered services. The default is <see cref="ServiceLifetime.Scoped"/>.
	/// </param>
	public static IServiceCollection AddHavitBlazorStorage(
		this IServiceCollection services,
		JsonSerializerOptions jsonSerializerOptions,
		ServiceLifetime lifetime = ServiceLifetime.Scoped)
	{
		return services.AddHavitBlazorStorage(options => options.JsonSerializerOptions = jsonSerializerOptions, lifetime);
	}
}