using Havit.Blazor.Grpc.Client.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Grpc.Client.Circuits.Infrastructure;

public class CircuitsNavigationManagerAccessor(
	ICircuitServicesAccessor circuitServicesAccessor)
	: INavigationManagerAccessor
{
	private readonly ICircuitServicesAccessor _circuitServicesAccessor = circuitServicesAccessor;

	public NavigationManager NavigationManager => _circuitServicesAccessor.Services.GetRequiredService<NavigationManager>();
}
