using Grpc.Net.Client.Web;

namespace Havit.Blazor.Grpc.Client;

public class GrpcClientInfrastructureOptions
{
	public Func<IServiceProvider, GrpcWebHandler> GrpcWebHandlerFactory { get; set; }
}