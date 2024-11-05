using System.Reflection;
using Grpc.AspNetCore.Server;
using Havit.Blazor.Grpc.Core;
using Havit.Blazor.Grpc.Server.GlobalizationLocalization;
using Havit.Blazor.Grpc.Server.ServerExceptions;
using Havit.Diagnostics.Contracts;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Grpc.Server;
using ProtoBuf.Meta;

namespace Havit.Blazor.Grpc.Server;

public static class GrpcServerServiceCollectionExtensions
{
	/// <summary>
	/// Adds the necessary infrastructure for gRPC servers.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
	/// <param name="assembliesToScanForDataContracts">Assembly to scan for data contracts</param>
	/// <param name="configureOptions">gRPC Service options</param>
	public static void AddGrpcServerInfrastructure(
		this IServiceCollection services,
		Assembly assemblyToScanForDataContracts,
		Action<GrpcServiceOptions> configureOptions = null)
	{
		AddGrpcServerInfrastructure(services, [assemblyToScanForDataContracts], configureOptions);
	}

	/// <summary>
	/// Adds the necessary infrastructure for gRPC servers.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
	/// <param name="assembliesToScanForDataContracts">Assemblies to scan for data contracts</param>
	/// <param name="configureOptions">gRPC Service options</param>
	public static void AddGrpcServerInfrastructure(
		this IServiceCollection services,
		Assembly[] assembliesToScanForDataContracts,
		Action<GrpcServiceOptions> configureOptions = null)
	{
		Contract.Requires<ArgumentNullException>(assembliesToScanForDataContracts is not null);

		services.AddSingleton<GlobalizationLocalizationGrpcServerInterceptor>();
		services.AddSingleton<ServerExceptionsGrpcServerInterceptor>();
		services.AddSingleton(BinderConfiguration.Create(
			marshallerFactories: CreateMarshallerFactories(assembliesToScanForDataContracts),
			binder: new ServiceBinderWithServiceResolutionFromServiceCollection(services)));

		services.AddCodeFirstGrpc(options =>
		{
			options.Interceptors.Add<GlobalizationLocalizationGrpcServerInterceptor>();
			options.Interceptors.Add<ServerExceptionsGrpcServerInterceptor>();
			options.ResponseCompressionLevel = System.IO.Compression.CompressionLevel.Optimal;

			configureOptions?.Invoke(options);
		});
	}

	/// <summary>
	/// Creates marshaller factories for the specified assemblies.
	/// Each assembly has its own marshaller factory.
	/// </summary>
	private static List<MarshallerFactory> CreateMarshallerFactories(Assembly[] assembliesToScanForDataContracts) =>
		assembliesToScanForDataContracts
			.Select(assembly => ProtoBufMarshallerFactory.Create(RuntimeTypeModel.Create().RegisterApplicationContracts(assembly)))
			.ToList();
}
