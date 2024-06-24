using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Grpc.Client.Infrastructure;

public class NavigationManagerGrpcClientClientUriResolver(NavigationManager navigationManager) : IGrpcClientClientUriResolver
{
	private readonly NavigationManager _navigationManager = navigationManager;

	public string GetCurrentClientUri()
	{
		return _navigationManager.Uri;
	}
}
