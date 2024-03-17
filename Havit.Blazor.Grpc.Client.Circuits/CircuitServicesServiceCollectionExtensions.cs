using Havit.Blazor.Grpc.Client.Circuits.Infrastructure;
using Havit.Blazor.Grpc.Client.Infrastructure;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Grpc.Client.Circuits;

public static class CircuitServicesServiceCollectionExtensions
{
	public static IServiceCollection AddGrpcClientCircuitServices(
		this IServiceCollection services)
	{
		services.AddScoped<ICircuitServicesAccessor, CircuitServicesAccessor>();
		services.AddScoped<CircuitHandler, ServicesAccessorCircuitHandler>();
		services.AddScoped<INavigationManagerAccessor, CircuitsNavigationManagerAccessor>();

		return services;
	}
}
