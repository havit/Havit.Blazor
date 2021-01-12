using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Havit.Blazor.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);

			builder.Services.AddLocalization();
			builder.Services.AddHxMessenger();
			builder.Services.AddHxMessageBoxHost();

			await builder.Build().RunAsync();
		}
	}
}
