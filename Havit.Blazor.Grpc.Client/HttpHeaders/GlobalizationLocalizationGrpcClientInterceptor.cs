using Grpc.Core.Interceptors;

namespace Havit.Blazor.Grpc.Client.HttpHeaders;

/// <summary>
/// gRPC Service (client-side) interceptor which adds "hx-culture" value to the HTTP header (to be consumed by GlobalizationLocalizationGrpcServerInterceptor on server-side).
/// </summary>
public class GlobalizationLocalizationGrpcClientInterceptor : CallerMetadataGrpcClientInterceptorBase // DI SINGLETON !!
{
	protected override void AddCallerMetadata<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context)
		where TRequest : class
		where TResponse : class
	{
		context.Options.Headers.Add("hx-culture", Thread.CurrentThread.CurrentCulture.ToString());
	}
}
