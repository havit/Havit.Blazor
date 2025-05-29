using System.Reflection;
using Havit.ComponentModel;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Havit.Blazor.Grpc.Server;

public static class EndpointRouteBuilderGrpcExtensions
{
	/// <summary>
	/// Maps gRPC services with <see cref="ApiContractAttribute"/> as route endpoints.
	/// </summary>
	/// <param name="builder">Endpoint route builder.</param>
	/// <param name="assemblyToScan">Assembly to scan for interfaces with <see cref="ApiContractAttribute"/>.</param>
	/// <param name="configureEndpointWithAuthorization">Optional configuration for endpoints with authorization (all except <c>[ApiContract(RequireAuthorization = false)]</c>). Usually you want to setup <c>endpoint.RequireAuthorization(...)</c> here."/></param>
	/// <param name="configureEndpointAll">Optional configuration for all endpoints.</param>
	public static void MapGrpcServicesByApiContractAttributes(
		this IEndpointRouteBuilder builder,
		Assembly assemblyToScan,
		Action<GrpcServiceEndpointConventionBuilder> configureEndpointWithAuthorization = null,
		Action<GrpcServiceEndpointConventionBuilder> configureEndpointAll = null)
	{
		MapGrpcServicesByApiContractAttributes(builder, [assemblyToScan], configureEndpointWithAuthorization, configureEndpointAll);
	}

	/// <summary>
	/// Maps gRPC services with <see cref="ApiContractAttribute"/> as route endpoints.
	/// </summary>
	/// <param name="builder">Endpoint route builder.</param>
	/// <param name="assembliesToScan">Assemblies to scan for interfaces with <see cref="ApiContractAttribute"/>.</param>
	/// <param name="configureEndpointWithAuthorization">Optional configuration for endpoints with authorization (all except <c>[ApiContract(RequireAuthorization = false)]</c>). Usually you want to setup <c>endpoint.RequireAuthorization(...)</c> here."/></param>
	/// <param name="configureEndpointAll">Optional configuration for all endpoints.</param>
	public static void MapGrpcServicesByApiContractAttributes(
		this IEndpointRouteBuilder builder,
		Assembly[] assembliesToScan,
		Action<GrpcServiceEndpointConventionBuilder> configureEndpointWithAuthorization = null,
		Action<GrpcServiceEndpointConventionBuilder> configureEndpointAll = null)
	{
		Contract.Requires<ArgumentNullException>(builder is not null, nameof(builder));
		Contract.Requires<ArgumentNullException>(assembliesToScan is not null, nameof(assembliesToScan));

		var interfacesAndAttributes = (from assembly in assembliesToScan
									   from type in assembly.GetTypes()
									   from apiContractAttribute in type.GetCustomAttributes(typeof(ApiContractAttribute), false).Cast<ApiContractAttribute>()
									   select new { Interface = type, Attribute = apiContractAttribute }).ToArray();

		var mapGrpcServiceMethodInfo = typeof(GrpcEndpointRouteBuilderExtensions).GetMethod(nameof(GrpcEndpointRouteBuilderExtensions.MapGrpcService));

		foreach (var item in interfacesAndAttributes)
		{
			var endpoint = (GrpcServiceEndpointConventionBuilder)mapGrpcServiceMethodInfo.MakeGenericMethod(item.Interface).Invoke(null, new[] { builder });

			configureEndpointAll?.Invoke(endpoint);
			if (item.Attribute.RequireAuthorization)
			{
				configureEndpointWithAuthorization?.Invoke(endpoint);
			}
		}
	}
}
