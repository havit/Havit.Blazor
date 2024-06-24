﻿using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Components.Web;

/// <summary>
/// Extension methods for installation of HxMessenger support.
/// </summary>
public static class MessengerServiceCollectionExtensions
{
	/// <summary>
	/// Adds <see cref="IHxMessengerService"/> support to be able to add messages to HxMessenger.
	/// </summary>
	public static IServiceCollection AddHxMessenger(this IServiceCollection services)
	{
		if (OperatingSystem.IsBrowser())
		{
			// Allows gRPC Interceptors and HttpMessageHandlers to pass error messages to the HxMessenger without having to struggle with different DI Scope
			return services.AddSingleton<IHxMessengerService, HxMessengerService>();
		}
		else
		{
			return services.AddScoped<IHxMessengerService, HxMessengerService>();
		}
	}
}
