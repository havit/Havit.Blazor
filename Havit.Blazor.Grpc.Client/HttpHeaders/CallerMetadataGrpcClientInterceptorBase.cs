using Grpc.Core.Interceptors;
using Grpc.Core;

namespace Havit.Blazor.Grpc.Client.HttpHeaders;

public abstract class CallerMetadataGrpcClientInterceptorBase : Interceptor
{
	protected abstract void AddCallerMetadata<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context)
		where TRequest : class
		where TResponse : class;


	private void AddCallerMetadataCore<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context)
		where TRequest : class
		where TResponse : class
	{
		// Call doesn't have a headers collection to add to.
		// Need to create a new context with headers for the call.
		// https://github.com/grpc/grpc-dotnet/blob/master/examples/Interceptor/Client/ClientLoggerInterceptor.cs
		if (context.Options.Headers == null)
		{
			var headers = new Metadata();
			var options = context.Options.WithHeaders(headers);
			context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
		}

		AddCallerMetadata(ref context);
	}

	public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
	{
		AddCallerMetadataCore(ref context);

		return continuation(context);
	}

	public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
	{
		AddCallerMetadataCore(ref context);

		return continuation(context);
	}

	public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
	{
		AddCallerMetadataCore(ref context);

		return continuation(request, context);
	}

	public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
	{
		AddCallerMetadataCore(ref context);

		return continuation(request, context);
	}
}
