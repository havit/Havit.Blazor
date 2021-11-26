using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core.Interceptors;
using Grpc.Core;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Grpc.Client.HttpHeaders
{
	/// <summary>
	/// gRPC Service interceptor (client-side) which adds "Referrer" HTTP header from NavigationManager.Uri (to be able to log calling page on server side).
	/// </summary>
	public class HttpReferrerGrpcClientInterceptor : CallerMetadataGrpcClientInterceptorBase
	{
		private readonly NavigationManager navigationManager;

		public HttpReferrerGrpcClientInterceptor(NavigationManager navigationManager)
		{
			this.navigationManager = navigationManager;
		}
		protected override void AddCallerMetadata<TRequest, TResponse>(ref ClientInterceptorContext<TRequest, TResponse> context)
		{
			context.Options.Headers.Add("Referrer", navigationManager.Uri);
		}
	}
}
