using Havit.ComponentModel;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Havit.Blazor.Grpc.Server.Tests;

[TestClass]
public class EndpointRouteBuilderGrpcExtensionsTests
{
	[ApiContract]
	public interface IDummyFacade { }

	/// <summary>
	/// Repro test: MapGrpcServicesByApiContractAttributes must resolve the correct MapGrpcService overload
	/// via reflection and invoke configureEndpointAll for each [ApiContract] interface.
	/// Previously, GetMethod("MapGrpcService") threw AmbiguousMatchException when multiple overloads existed.
	/// </summary>
	[TestMethod]
	public void MapGrpcServicesByApiContractAttributes_InvokesConfigureEndpointAll_ForApiContractInterface()
	{
		// Arrange
		var builder = WebApplication.CreateBuilder();
		builder.Services.AddGrpc();
		var app = builder.Build();

		var configuredEndpoints = new List<GrpcServiceEndpointConventionBuilder>();

		// Act
		app.MapGrpcServicesByApiContractAttributes(
			typeof(IDummyFacade).Assembly,
			configureEndpointAll: endpoint => configuredEndpoints.Add(endpoint));

		// Assert — configureEndpointAll must have been called (at least for IDummyFacade)
		Assert.IsNotEmpty(configuredEndpoints, "configureEndpointAll was not invoked for any [ApiContract] interface.");
	}
}
