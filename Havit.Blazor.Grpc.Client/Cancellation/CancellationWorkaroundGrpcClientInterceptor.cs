using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace Havit.Blazor.Grpc.Client.Cancellation
{
	/// <summary>
	/// Provides correct OperationCanceledException (with original CancellationToken) for client cancellations.
	/// </summary>
	/// <remarks>
	/// gRPC stack does not preserve CancellationToken:
	/// * when RpcException.Status.DebugException is OperationCanceledException (CancellationToken is None)
	/// * RpcException.Status.DebugException is HttpRequestException (the HttpMessageHandler.Send[Async]() gets another CancellationToken)
	/// </remarks>
	public class CancellationWorkaroundGrpcClientInterceptor : Interceptor
	{
		public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
					TRequest request,
					ClientInterceptorContext<TRequest, TResponse> context,
					AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
		{
			var cancellationToken = context.Options.CancellationToken;
			var call = continuation(request, context);

			return new AsyncUnaryCall<TResponse>(HandleResponseAsync(call.ResponseAsync, cancellationToken), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
		}

		public async Task<TResponse> HandleResponseAsync<TResponse>(Task<TResponse> responseTask, CancellationToken cancellationToken)
		{
			try
			{
				return await responseTask;
			}
			catch
			{
				cancellationToken.ThrowIfCancellationRequested();
				throw;
			}
		}
	}
}
