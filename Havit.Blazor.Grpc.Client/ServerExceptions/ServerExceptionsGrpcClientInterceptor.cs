using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Havit.Blazor.Grpc.Client.ServerExceptions;

public class ServerExceptionsGrpcClientInterceptor : Interceptor
{
	private readonly IEnumerable<IOperationFailedExceptionGrpcClientListener> operationFailedExceptionListeners;
	private readonly IEnumerable<IServerExceptionGrpcClientListener> serverExceptionGrpcClientListeners;
	private readonly ILogger<ServerExceptionsGrpcClientInterceptor> logger;

	// do not inject scoped services here, the scope is not available
	public ServerExceptionsGrpcClientInterceptor(
		IEnumerable<IOperationFailedExceptionGrpcClientListener> operationFailedExceptionListeners,
		IEnumerable<IServerExceptionGrpcClientListener> serverExceptionGrpcClientListeners,
		ILogger<ServerExceptionsGrpcClientInterceptor> logger)
	{
		this.operationFailedExceptionListeners = operationFailedExceptionListeners;
		this.serverExceptionGrpcClientListeners = serverExceptionGrpcClientListeners;
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

			foreach (var listener in operationFailedExceptionListeners)
			{
				await listener.ProcessAsync(errorMessage);
			}

			throw new OperationFailedException(errorMessage);
		}
		catch (RpcException e)
		{
			foreach (var listener in serverExceptionGrpcClientListeners)
			{
				await listener.ProcessExceptionAsync(e);

			}

			ExceptionDispatchInfo.Capture(e).Throw();
			throw; // to satisfy the compiler
		}
	}
}
