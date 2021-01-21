//using System;
//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Options;
//using Microsoft.Net.Http.Headers;

//namespace Havit.Blazor.Components.Web.Bootstrap
//{
//	internal class ConfigureStaticFileOptionsForBootstrapIcons : IPostConfigureOptions<StaticFileOptions>
//	{
//		private readonly IWebHostEnvironment webHostEnvironment;

//		public ConfigureStaticFileOptionsForBootstrapIcons(IWebHostEnvironment webHostEnvironment)
//		{
//			this.webHostEnvironment = webHostEnvironment;
//		}

//		public void PostConfigure(string name, StaticFileOptions options)
//		{
//			if (!webHostEnvironment.IsDevelopment())
//			{
//				name = name ?? throw new ArgumentNullException(nameof(name));
//				options = options ?? throw new ArgumentNullException(nameof(options));

//				var previousOnPrepareResponse = options.OnPrepareResponse;

//				options.OnPrepareResponse = ctx =>
//				{
//					previousOnPrepareResponse.Invoke(ctx);

//					if (ctx.Context.Request.Path.Equals("/_content/Havit.Blazor.Components.Web.Bootstrap/bootstrap-icons.svg", StringComparison.OrdinalIgnoreCase))
//					{
//						ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public; max-age=3600";
//					}
//				};
//			}
//		}
//	}

//}