using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core.Interceptors;
using Grpc.Core;
using static Grpc.Core.Interceptors.Interceptor;

namespace Havit.Blazor.Grpc.Client.GlobalizationLocalization
{
	/// <summary>
	/// gRPC Service (client-side) interceptor which adds "hx-culture" value to the HTTP header (to be consumed by GlobalizationLocalizationGrpcServerInterceptor on server-side).
	/// </summary>
	public class GlobalizationLocalizationGrpcClientInterceptor : Interceptor // DI SINGLETON !!
	{
		private void AddCallerMetadata<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context)
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

			context.Options.Headers.Add("hx-culture", Thread.CurrentThread.CurrentCulture.ToString());
		}

		public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
		{
			AddCallerMetadata(ref context);

			return continuation(request, context);
		}

		public override AsyncClientStreamingCall<TRequest, TResponse> AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
		{
			AddCallerMetadata(ref context);

			return continuation(context);
		}

		public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request, ClientInterceptorContext<TRequest, TResponse> context, AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
		{
			AddCallerMetadata(ref context);

			return continuation(request, context);
		}

		public override AsyncDuplexStreamingCall<TRequest, TResponse> AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context, AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
		{
			AddCallerMetadata(ref context);

			return continuation(context);
		}
	}
}
