using Havit.Blazor.Grpc.Client.Infrastructure;
using Havit.Blazor.Grpc.Client.ServerSideRendering.Infrastructure;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Grpc.Client.ServerSideRendering;

public static class GrpcClientSsrServicesServiceCollectionExtensions
{
	public static IServiceCollection AddGrpcClientServerSideRenderingServices(
		this IServiceCollection services)
	{
		services.AddScoped<ICircuitServicesAccessor, CircuitServicesAccessor>();
		services.AddScoped<CircuitHandler, ServicesAccessorCircuitHandler>();
		services.AddScoped<IGrpcClientClientUriResolver, ServerSideRenderingGrpcClientClientUriResolver>();

		return services;
	}
}
