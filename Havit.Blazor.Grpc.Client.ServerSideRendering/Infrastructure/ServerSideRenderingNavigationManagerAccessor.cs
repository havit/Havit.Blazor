using Havit.Blazor.Grpc.Client.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Grpc.Client.ServerSideRendering.Infrastructure;

public class ServerSideRenderingNavigationManagerAccessor(
	ICircuitServicesAccessor circuitServicesAccessor,
	IServiceProvider serviceProvider)
	: INavigationManagerAccessor
{
	private readonly ICircuitServicesAccessor _circuitServicesAccessor = circuitServicesAccessor;
	private readonly IServiceProvider _serviceProvider = serviceProvider;

	public NavigationManager NavigationManager
	{
		get
		{
			if (_circuitServicesAccessor.Services != null)
			{
				// interactive SSR (SignalR circuit)
				return _circuitServicesAccessor.Services.GetRequiredService<NavigationManager>();
			}

			// static SSR + prerendering
			return _serviceProvider.GetRequiredService<NavigationManager>();
		}
	}
}
