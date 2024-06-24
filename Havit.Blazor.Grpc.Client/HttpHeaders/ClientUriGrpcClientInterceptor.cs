using Grpc.Core.Interceptors;
using Havit.Blazor.Grpc.Client.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Grpc.Client.HttpHeaders;

/// <summary>
/// gRPC Service interceptor (client-side) which adds "hx-client-uri" HTTP header (to be able to log calling page on server side).
/// </summary>
public class ClientUriGrpcClientInterceptor : CallerMetadataGrpcClientInterceptorBase
{
	private readonly IGrpcClientClientUriResolver _grpcClientClientUriResolver;

	public ClientUriGrpcClientInterceptor(IGrpcClientClientUriResolver grpcClientClientUriResolver)
	{
		_grpcClientClientUriResolver = grpcClientClientUriResolver;
	}
	protected override void AddCallerMetadata<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context)
	{
		context.Options.Headers.Add("hx-client-uri", _grpcClientClientUriResolver.GetCurrentClientUri());
	}
}
