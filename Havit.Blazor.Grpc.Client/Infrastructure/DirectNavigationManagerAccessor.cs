using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Grpc.Client.Infrastructure;

public class DirectNavigationManagerAccessor : INavigationManagerAccessor
{
	public DirectNavigationManagerAccessor(NavigationManager navigationManager)
	{
		NavigationManager = navigationManager;
	}

	public NavigationManager NavigationManager { get; }
}
