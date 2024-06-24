namespace Havit.Blazor.Grpc.Client.ServerSideRendering.Infrastructure;

public interface ICircuitServicesAccessor
{
	IServiceProvider Services { get; set; }
}