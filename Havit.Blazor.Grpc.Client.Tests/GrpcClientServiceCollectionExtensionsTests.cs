using Havit.Blazor.Grpc.TestContracts;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Grpc.Client.Tests;

public class GrpcClientServiceCollectionExtensionsTests
{
	[Fact]
	public void GrpcClientServiceCollectionExtensions_AddGrpcClientsByApiContractAttributes_RegistersServicesWithAttribute()
	{
		// arrange
		var services = new ServiceCollection();

		// act
		services.AddGrpcClientsByApiContractAttributes(typeof(Dto).Assembly);

		// assert
		Assert.NotNull(services.FirstOrDefault(sd => sd.ServiceType == typeof(ITestFacade)));
	}
}
