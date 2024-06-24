using Grpc.Core.Interceptors;
using Grpc.Core;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Diagnostics.CodeAnalysis;

namespace Havit.Blazor.Grpc.Client.WebAssembly;

/// <summary>
/// gRPC Client Interceptor which redirects UI to login when access token is not available (gRPC call to service which requires auth).
/// </summary>
public class AuthorizationGrpcClientInterceptor : Interceptor
{
	public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(
				TRequest request,
				ClientInterceptorContext<TRequest, TResponse> context,
				AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
	{
		var call = continuation(request, context);

		return new AsyncUnaryCall<TResponse>(HandleAsyncUnaryResponse(call.ResponseAsync), call.ResponseHeadersAsync, call.GetStatus, call.GetTrailers, call.Dispose);
	}

	[SuppressMessage("Style", "VSTHRD200:Use \"Async\" suffix for async methods", Justification = "Async by nature")]
	private async Task<TResponse> HandleAsyncUnaryResponse<TResponse>(Task<TResponse> inputResponse)
	{
		try
		{
#pragma warning disable VSTHRD003 // Avoid awaiting foreign Tasks
			return await inputResponse;
#pragma warning restore VSTHRD003 // Avoid awaiting foreign Tasks
		}
		catch (RpcException e) when (e.Status.DebugException is AccessTokenNotAvailableException innerException)
		{
			innerException.Redirect();

			return default;
		}
	}
}
