using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation
{
	public class Program
	{
		public static async Task Main(string[] args)
		{
			var builder = WebAssemblyHostBuilder.CreateDefault(args);

			var cultureInfo = new CultureInfo("en-US");
			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

			builder.Services.AddHxServices();
			builder.Services.AddHxMessenger();
			builder.Services.AddHxMessageBoxHost();

			await builder.Build().RunAsync();
		}
	}
}
