using System.Threading.Tasks;
using Grpc.Core;

namespace Havit.Blazor.Grpc.Client.ServerExceptions
{
	public interface IServerExceptionGrpcClientListener
	{
		Task ProcessExceptionAsync(RpcException e);
	}
}