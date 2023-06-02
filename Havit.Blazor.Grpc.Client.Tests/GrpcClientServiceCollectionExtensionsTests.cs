using Havit.Blazor.Grpc.TestContracts;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Grpc.Client.Tests;

[TestClass]
public class GrpcClientServiceCollectionExtensionsTests
{
	[TestMethod]
	public void GrpcClientServiceCollectionExtensions_AddGrpcClientsByApiContractAttributes_RegistersServicesWithAttribute()
	{
		// arrange
		var services = new ServiceCollection();


		// act
		services.AddGrpcClientsByApiContractAttributes(typeof(Dto).Assembly);

		// assert
		Assert.IsNotNull(services.FirstOrDefault(sd => sd.ServiceType == typeof(ITestFacade)));
	}

#if NET6_0
	[TestMethod]
	public void GrpcClientServiceCollectionExtensions_AddGrpcClientsByApiContractAttributes_RegistersFuncFactoryForServiceWithAttribute()
	{
		// arrange
		var services = new ServiceCollection();


		// act
		services.AddGrpcClientsByApiContractAttributes(typeof(Dto).Assembly);

		// assert
		Assert.IsNotNull(services.FirstOrDefault(sd => sd.ServiceType == typeof(Func<ITestFacade>)));
	}
#endif
}
