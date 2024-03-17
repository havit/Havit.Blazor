
namespace Havit.Blazor.Grpc.Client.Circuits.Infrastructure;

public interface ICircuitServicesAccessor
{
	IServiceProvider Services { get; set; }
}