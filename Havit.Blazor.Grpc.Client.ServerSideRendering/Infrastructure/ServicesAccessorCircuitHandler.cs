using Microsoft.AspNetCore.Components.Server.Circuits;

namespace Havit.Blazor.Grpc.Client.ServerSideRendering.Infrastructure;

public class ServicesAccessorCircuitHandler(
	IServiceProvider services,
	ICircuitServicesAccessor servicesAccessor) : CircuitHandler
{
	private readonly IServiceProvider _services = services;
	private readonly ICircuitServicesAccessor _circuitServicesAccessor = servicesAccessor;

	public override Func<CircuitInboundActivityContext, Task> CreateInboundActivityHandler(
		Func<CircuitInboundActivityContext, Task> next)
	{
		return async context =>
		{
			_circuitServicesAccessor.Services = _services;
			await next(context);
			_circuitServicesAccessor.Services = null;
		};
	}
}