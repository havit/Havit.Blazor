namespace Havit.Blazor.Grpc.Client.ServerSideRendering.Infrastructure;

/// <summary>
/// https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-8.0#access-server-side-blazor-services-from-a-different-di-scope
/// </summary>
public class CircuitServicesAccessor : ICircuitServicesAccessor
{
	private static readonly AsyncLocal<IServiceProvider> s_blazorServices = new();

	public IServiceProvider Services
	{
		get => s_blazorServices.Value;
		set => s_blazorServices.Value = value;
	}
}
