using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Havit.Blazor.Grpc.Client.ServerExceptions
{
	public class ServerExceptionsGrpcClientInterceptor : Interceptor
	{
		private readonly IOperationFailedExceptionPublisher operationFailedExceptionPublisher;
		private readonly ILogger<ServerExceptionsGrpcClientInterceptor> logger;

		// do not inject scoped services here, the scope is not available
		public ServerExceptionsGrpcClientInterceptor(
			IOperationFailedExceptionPublisher operationFailedExceptionPublisher,
			ILogger<ServerExceptionsGrpcClientInterceptor> logger)
		{
			this.operationFailedExceptionPublisher = operationFailedExceptionPublisher;
			this.logger = logger;
		}

		public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
					TRequest request,
					ClientInterceptorContext<TRequest, TResponse> context,
					AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
		{
			var call = continuation(request, context);

			return new AsyncUnaryCall<TResponse>(HandleResponseAsync(call.ResponseAsync), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
		}

		public async Task<TResponse> HandleResponseAsync<TResponse>(Task<TResponse> responseTask)
		{
			try
			{
				return await responseTask;
			}
			catch (RpcException e) when (e.Status.StatusCode == StatusCode.FailedPrecondition)
			{
				string errorMessage = e.Status.Detail;

				logger.LogWarning($"{nameof(OperationFailedException)}: {errorMessage}");

				await operationFailedExceptionPublisher.PublishAsync(errorMessage);

				throw new OperationFailedException(errorMessage);
			}
		}
	}
}
