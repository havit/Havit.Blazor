using System.Globalization;
using Grpc.Core.Interceptors;
using Grpc.Core;

namespace Havit.Blazor.Grpc.Server.GlobalizationLocalization;

/// <summary>
/// gRPC Service (server-side) interceptor which looks for <c>hx-sulture</c> request header and uses the value (if found)
/// for setting <see cref="Thread.CurrentCulture"/> and <see cref="Thread.CurrentUICulture"/>.
/// </summary>
public class GlobalizationLocalizationGrpcServerInterceptor : Interceptor   // DI SINGLETON !!
{
	private void SetCultureFromMetadata(ServerCallContext context)
	{
		var cultureInfoName = context.RequestHeaders.SingleOrDefault(h => h.Key == "hx-culture")?.Value;
		if (!String.IsNullOrWhiteSpace(cultureInfoName))
		{
			var cultureInfo = new CultureInfo(cultureInfoName);
			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
		}
	}

	public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
		  TRequest request,
		  ServerCallContext context,
		  UnaryServerMethod<TRequest, TResponse> continuation)
	{
		SetCultureFromMetadata(context);

		return base.UnaryServerHandler(request, context, continuation);
	}

	public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
		IAsyncStreamReader<TRequest> requestStream,
		ServerCallContext context,
		ClientStreamingServerMethod<TRequest, TResponse> continuation)
	{
		SetCultureFromMetadata(context);

		return base.ClientStreamingServerHandler(requestStream, context, continuation);
	}

	public override Task ServerStreamingServerHandler<TRequest, TResponse>(
		TRequest request,
		IServerStreamWriter<TResponse> responseStream,
		ServerCallContext context,
		ServerStreamingServerMethod<TRequest, TResponse> continuation)
	{
		SetCultureFromMetadata(context);

		return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
	}

	public override Task DuplexStreamingServerHandler<TRequest, TResponse>(
		IAsyncStreamReader<TRequest> requestStream,
		IServerStreamWriter<TResponse> responseStream,
		ServerCallContext context,
		DuplexStreamingServerMethod<TRequest, TResponse> continuation)
	{
		SetCultureFromMetadata(context);

		return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
	}
}
