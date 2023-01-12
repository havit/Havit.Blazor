using System.Reflection;
using Grpc.Net.Client.Web;
using Havit.Blazor.Grpc.Client.Cancellation;
using Havit.Blazor.Grpc.Client.HttpHeaders;
using Havit.Blazor.Grpc.Client.ServerExceptions;
using Havit.Blazor.Grpc.Core;
using Havit.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Meta;

namespace Havit.Blazor.Grpc.Client;

public static class GrpcClientServiceCollectionExtensions
{
	public static void AddGrpcClientInfrastructure(
		this IServiceCollection services,
		Assembly assemblyToScanForDataContracts)
	{
		services.AddTransient<ServerExceptionsGrpcClientInterceptor>();
		services.AddSingleton<GlobalizationLocalizationGrpcClientInterceptor>();
		services.AddSingleton<ClientUriGrpcClientInterceptor>();
		//services.AddTransient<CancellationWorkaroundGrpcClientInterceptor>();
		services.AddTransient<GrpcWebHandler>(provider => new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
		services.AddSingleton<ClientFactory>(ClientFactory.Create(BinderConfiguration.Create(marshallerFactories: new[] { ProtoBufMarshallerFactory.Create(RuntimeTypeModel.Create().RegisterApplicationContracts(assemblyToScanForDataContracts)) }, binder: new ProtoBufServiceBinder())));
	}

	public static void AddGrpcClientsByApiContractAttributes(
		this IServiceCollection services,
		Assembly assemblyToScan,
		Action<IHttpClientBuilder> configureGrpcClientWithAuthorization = null,
		Action<IHttpClientBuilder> configureGrpClientAll = null)
	{
		var interfacesAndAttributes = (from type in assemblyToScan.GetTypes()
									   from apiContractAttribute in type.GetCustomAttributes(typeof(ApiContractAttribute), false).Cast<ApiContractAttribute>()
									   select new { Interface = type, Attribute = apiContractAttribute }).ToArray();

		var addGrpcClientCoreMethodInfo = typeof(GrpcClientServiceCollectionExtensions).GetMethod(nameof(AddGrpcClientCore), BindingFlags.NonPublic | BindingFlags.Static);

		foreach (var item in interfacesAndAttributes)
		{
			// services.AddGrpcClientCore<TService>(configureGrpcClientWithAuthorization, configureGrpClientAll);
			var grpcClient = (IHttpClientBuilder)addGrpcClientCoreMethodInfo.MakeGenericMethod(item.Interface)
				.Invoke(null, new object[] { services, item.Attribute.RequireAuthorization ? configureGrpcClientWithAuthorization : null, configureGrpClientAll });
		}
	}

	private static void AddGrpcClientCore<TService>(
		this IServiceCollection services,
		Action<IHttpClientBuilder> configureGrpcClientWithAuthorization = null,
		Action<IHttpClientBuilder> configureGrpClientAll = null)
		where TService : class
	{
		var grpcClient = services
			.AddCodeFirstGrpcClient<TService>((provider, options) =>
			{
				var navigationManager = provider.GetRequiredService<NavigationManager>();
				var backendUrl = navigationManager.BaseUri;

				options.Address = new Uri(backendUrl);
			})
			.ConfigurePrimaryHttpMessageHandler<GrpcWebHandler>()
			.ConfigureChannel(options =>
			{
				options.ThrowOperationCanceledOnCancellation = true;
			})
			.AddInterceptor<GlobalizationLocalizationGrpcClientInterceptor>()
			.AddInterceptor<ClientUriGrpcClientInterceptor>()
			.AddInterceptor<ServerExceptionsGrpcClientInterceptor>();
		//.AddInterceptor<CancellationWorkaroundGrpcClientInterceptor>();

		configureGrpClientAll?.Invoke(grpcClient);
		configureGrpcClientWithAuthorization?.Invoke(grpcClient);

#if NET6_0
		// NET6 failing GC workaround https://github.com/dotnet/runtime/issues/62054
		services.AddSingleton<Func<TService>>(sp => () => sp.GetRequiredService<TService>());
#endif
	}
}
