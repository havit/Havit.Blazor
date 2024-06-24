using System.Reflection;
using Havit.Blazor.Grpc.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Grpc.Server;

// SRC: https://github.com/protobuf-net/protobuf-net.Grpc/blob/main/examples/pb-net-grpc/Server_CS/ServiceBinderWithServiceResolutionFromServiceCollection.cs
public class ServiceBinderWithServiceResolutionFromServiceCollection : ProtoBufServiceBinder
{
	private readonly IServiceCollection _services;

	public ServiceBinderWithServiceResolutionFromServiceCollection(IServiceCollection services)
	{
		_services = services;
	}

	public override IList<object> GetMetadata(MethodInfo method, Type contractType, Type serviceType)
	{
		var resolvedServiceType = serviceType;
		if (serviceType.IsInterface)
		{
			resolvedServiceType = _services.SingleOrDefault(x => x.ServiceType == serviceType)?.ImplementationType ?? serviceType;
		}

		return base.GetMetadata(method, contractType, resolvedServiceType);
	}
}
