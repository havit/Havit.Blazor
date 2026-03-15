using System.Reflection;
using Grpc.Net.Client.Web;
using Grpc.Net.ClientFactory;
using Havit.Blazor.Grpc.Client.HttpHeaders;
using Havit.Blazor.Grpc.Client.Infrastructure;
using Havit.Blazor.Grpc.Client.ServerExceptions;
using Havit.Blazor.Grpc.Core;
using Havit.ComponentModel;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProtoBuf.Grpc.ClientFactory;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Meta;

namespace Havit.Blazor.Grpc.Client;

/// <summary>
/// Extension methods for configuring gRPC client services.
/// </summary>
public static class GrpcClientServiceCollectionExtensions
{
	/// <summary>
	/// Adds the necessary infrastructure for gRPC clients.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
	/// <param name="assemblyToScanForDataContracts">Assembly to scan for data contracts.</param>
	public static void AddGrpcClientInfrastructure(
		this IServiceCollection services,
		Assembly assemblyToScanForDataContracts)
	{
		AddGrpcClientInfrastructure(services, [assemblyToScanForDataContracts]);
	}

	/// <summary>
	/// Adds the necessary infrastructure for gRPC clients.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
	/// <param name="assembliesToScanForDataContracts">Assemblies to scan for data contracts.</param>
	public static void AddGrpcClientInfrastructure(
		this IServiceCollection services,
		Assembly[] assembliesToScanForDataContracts)
	{
		services.AddTransient<ServerExceptionsGrpcClientInterceptor>();
		services.AddSingleton<GlobalizationLocalizationGrpcClientInterceptor>();
		services.AddScoped<ClientUriGrpcClientInterceptor>();

		// we want to allow the application to provide its own GrpcWebHandler
		services.TryAddTransient<GrpcWebHandler>((provider => new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler())));

		services.AddSingleton<ClientFactory>(ClientFactory.Create(BinderConfiguration.Create(
			marshallerFactories: CreateMarshallerFactories(assembliesToScanForDataContracts),
			binder: new ProtoBufServiceBinder())));

		services.TryAddScoped<IGrpcClientClientUriResolver, NavigationManagerGrpcClientClientUriResolver>();
	}

	/// <summary>
	/// Adds gRPC clients based on API contract attributes.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
	/// <param name="assemblyToScan">The assembly to scan for API contract attributes.</param>
	/// <param name="configureGrpcClientWithAuthorization">An optional action to configure gRPC clients with authorization.</param>
	/// <param name="configureGrpClientAll">An optional action to configure all gRPC clients.</param>
	/// <param name="configureGrpcClientFactory">
	/// An optional action to configure the gRPC client factory.
	/// If Not provided, <c>options.Address</c> (backend URL) will be configured from <c>NavigationManager.BaseUri</c>.
	/// </param>
	public static void AddGrpcClientsByApiContractAttributes(
		this IServiceCollection services,
		Assembly assemblyToScan,
		Action<IHttpClientBuilder> configureGrpcClientWithAuthorization = null,
		Action<IHttpClientBuilder> configureGrpClientAll = null,
		Action<IServiceProvider, GrpcClientFactoryOptions> configureGrpcClientFactory = null)
	{
		AddGrpcClientsByApiContractAttributes(services, [assemblyToScan], configureGrpcClientWithAuthorization, configureGrpClientAll, configureGrpcClientFactory);
	}

	/// <summary>
	/// Adds gRPC clients based on API contract attributes.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
	/// <param name="assembliesToScan">The assembly to scan for API contract attributes.</param>
	/// <param name="configureGrpcClientWithAuthorization">An optional action to configure gRPC clients with authorization.</param>
	/// <param name="configureGrpClientAll">An optional action to configure all gRPC clients.</param>
	/// <param name="configureGrpcClientFactory">
	/// An optional action to configure the gRPC client factory.
	/// If Not provided, <c>options.Address</c> (backend URL) will be configured from <c>NavigationManager.BaseUri</c>.
	/// </param>
	public static void AddGrpcClientsByApiContractAttributes(
		this IServiceCollection services,
		Assembly[] assembliesToScan,
		Action<IHttpClientBuilder> configureGrpcClientWithAuthorization = null,
		Action<IHttpClientBuilder> configureGrpClientAll = null,
		Action<IServiceProvider, GrpcClientFactoryOptions> configureGrpcClientFactory = null)
	{
		Contract.Requires<ArgumentNullException>(assembliesToScan is not null);

		var interfacesAndAttributes = (from assembly in assembliesToScan
									   from type in assembly.GetTypes()
									   from apiContractAttribute in type.GetCustomAttributes(typeof(ApiContractAttribute), false).Cast<ApiContractAttribute>()
									   select new { Interface = type, Attribute = apiContractAttribute }).ToArray();

		var addGrpcClientCoreMethodInfo = typeof(GrpcClientServiceCollectionExtensions).GetMethod(nameof(AddGrpcClientCore), BindingFlags.NonPublic | BindingFlags.Static);

		foreach (var item in interfacesAndAttributes)
		{
			// services.AddGrpcClientCore<TService>(configureGrpcClientWithAuthorization, configureGrpClientAll);
			var grpcClient = (IHttpClientBuilder)addGrpcClientCoreMethodInfo.MakeGenericMethod(item.Interface)
				.Invoke(null, new object[]
				{
						services,
						item.Attribute.RequireAuthorization ? configureGrpcClientWithAuthorization : null,
						configureGrpClientAll,
						configureGrpcClientFactory
				});
		}
	}

	private static List<MarshallerFactory> CreateMarshallerFactories(Assembly[] assembliesToScanForDataContracts) =>
		assembliesToScanForDataContracts
			.Select(assembly => ProtoBufMarshallerFactory.Create(RuntimeTypeModel.Create().RegisterApplicationContracts(assembly)))
			.ToList();

	private static void AddGrpcClientCore<TService>(
		this IServiceCollection services,
		Action<IHttpClientBuilder> configureGrpcClientWithAuthorization = null,
		Action<IHttpClientBuilder> configureGrpClientAll = null,
		Action<IServiceProvider, GrpcClientFactoryOptions> configureGrpcClientFactory = null)
		where TService : class
	{
		if (configureGrpcClientFactory is null)
		{
			// default configuration, if not provided explicitly
			configureGrpcClientFactory = (provider, grpcClientFactoryOptions) =>
			{
				var navigationManager = provider.GetRequiredService<NavigationManager>();
				var backendUrl = navigationManager.BaseUri;

				grpcClientFactoryOptions.Address = new Uri(backendUrl);
			};
		}
		var grpcClient = services
			.AddCodeFirstGrpcClient<TService>(configureGrpcClientFactory)
			.ConfigurePrimaryHttpMessageHandler<GrpcWebHandler>()
			.ConfigureChannel(options =>
			{
				options.ThrowOperationCanceledOnCancellation = true;
			})
			.AddInterceptor<GlobalizationLocalizationGrpcClientInterceptor>()
			.AddInterceptor<ClientUriGrpcClientInterceptor>()
			.AddInterceptor<ServerExceptionsGrpcClientInterceptor>();

		configureGrpClientAll?.Invoke(grpcClient);
		configureGrpcClientWithAuthorization?.Invoke(grpcClient);
	}
}
