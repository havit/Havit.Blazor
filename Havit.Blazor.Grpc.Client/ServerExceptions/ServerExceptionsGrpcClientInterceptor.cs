using System.Runtime.ExceptionServices;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace Havit.Blazor.Grpc.Client.ServerExceptions;

public class ServerExceptionsGrpcClientInterceptor : Interceptor
{
	private readonly IEnumerable<IOperationFailedExceptionGrpcClientListener> _operationFailedExceptionListeners;
	private readonly IEnumerable<IServerExceptionGrpcClientListener> _serverExceptionGrpcClientListeners;
	private readonly ILogger<ServerExceptionsGrpcClientInterceptor> _logger;

	// do not inject scoped services here, the scope is not available
	public ServerExceptionsGrpcClientInterceptor(
		IEnumerable<IOperationFailedExceptionGrpcClientListener> operationFailedExceptionListeners,
		IEnumerable<IServerExceptionGrpcClientListener> serverExceptionGrpcClientListeners,
		ILogger<ServerExceptionsGrpcClientInterceptor> logger)
	{
		_operationFailedExceptionListeners = operationFailedExceptionListeners;
		_serverExceptionGrpcClientListeners = serverExceptionGrpcClientListeners;
		_logger = logger;
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
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
			return await responseTask;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
		}
		catch (RpcException e) when (e.Status.StatusCode == StatusCode.FailedPrecondition)
		{
			string errorMessage = e.Status.Detail;

			_logger.LogWarning($"{nameof(OperationFailedException)}: {errorMessage}");

			foreach (var listener in _operationFailedExceptionListeners)
			{
				await listener.ProcessAsync(errorMessage);
			}

			throw new OperationFailedException(errorMessage);
		}
		catch (RpcException e)
		{
			foreach (var listener in _serverExceptionGrpcClientListeners)
			{
				await listener.ProcessExceptionAsync(e);

			}

			ExceptionDispatchInfo.Capture(e).Throw();
			throw; // to satisfy the compiler
		}
	}
}
