using Havit.Blazor.Grpc.Client.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Grpc.Client.ServerSideRendering.Infrastructure;

public class ServerSideRenderingGrpcClientClientUriResolver(
	ICircuitServicesAccessor circuitServicesAccessor,
	IServiceProvider serviceProvider)
	: IGrpcClientClientUriResolver
{
	private readonly ICircuitServicesAccessor _circuitServicesAccessor = circuitServicesAccessor;
	private readonly IServiceProvider _serviceProvider = serviceProvider;

	public string GetCurrentClientUri()
	{
		if (_circuitServicesAccessor.Services != null)
		{
			// interactive SSR (SignalR circuit)
			return _circuitServicesAccessor.Services.GetRequiredService<NavigationManager>().Uri;
		}

		// static SSR + prerendering
		var httpContextAccessor = _serviceProvider.GetRequiredService<IHttpContextAccessor>();
		Contract.Assert<InvalidOperationException>(httpContextAccessor != null, "IHttpContextAccessor not available. (To support static SSR or prerendering, use 'AddHttpContextAccessor()' when configuring your services.)");
		return httpContextAccessor.HttpContext.Request.GetDisplayUrl();
	}
}
