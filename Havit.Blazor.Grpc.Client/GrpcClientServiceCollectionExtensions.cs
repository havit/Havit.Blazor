using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Grpc.Net.Client.Web;
using Grpc.Net.ClientFactory;
using Havit.Blazor.Grpc.Client.Cancellation;
using Havit.Blazor.Grpc.Client.ServerExceptions;
using Havit.Blazor.Grpc.Core;
using Havit.ComponentModel;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Configuration;
using ProtoBuf.Meta;

namespace Havit.Blazor.Grpc.Client
{
	public static class GrpcClientServiceCollectionExtensions
	{
		public static void AddGrpcClientInfrastructure(this IServiceCollection services)
		{
			services.AddTransient<ServerExceptionsGrpcClientInterceptor>();
			services.AddTransient<CancellationWorkaroundGrpcClientInterceptor>();
			services.AddTransient<GrpcWebHandler>(provider => new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler()));
			services.AddSingleton<ClientFactory>(ClientFactory.Create(BinderConfiguration.Create(marshallerFactories: new[] { ProtoBufMarshallerFactory.Create(RuntimeTypeModel.Create().RegisterApplicationContracts()) }, binder: new ProtoBufServiceBinder())));
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

			var addCodeFirstGrpcClientMethodInfo = typeof(ProtoBuf.Grpc.ClientFactory.ServicesExtensions)
				.GetMethod(nameof(ProtoBuf.Grpc.ClientFactory.ServicesExtensions.AddCodeFirstGrpcClient), new[] { typeof(IServiceCollection), typeof(Action<IServiceProvider, GrpcClientFactoryOptions>) });

			Action<IServiceProvider, GrpcClientFactoryOptions> configureClientAction = (provider, options) =>
			{
				var navigationManager = provider.GetRequiredService<NavigationManager>();
				var backendUrl = navigationManager.BaseUri;

				options.Address = new Uri(backendUrl);
			};

			foreach (var item in interfacesAndAttributes)
			{
				// services.AddCodeFirstGrpcClient<TService>(configureClientAction)
				var grpcClient = (IHttpClientBuilder)addCodeFirstGrpcClientMethodInfo.MakeGenericMethod(item.Interface)
					.Invoke(null, new object[] { services, configureClientAction });

				grpcClient
					.ConfigurePrimaryHttpMessageHandler<GrpcWebHandler>()
					.AddInterceptor<ServerExceptionsGrpcClientInterceptor>()
					.AddInterceptor<CancellationWorkaroundGrpcClientInterceptor>();

				configureGrpClientAll?.Invoke(grpcClient);

				if (item.Attribute.RequireAuthorization)
				{
					configureGrpcClientWithAuthorization?.Invoke(grpcClient);
				}
			}
		}
	}
}
