namespace Havit.Blazor.Documentation.Server;

public class Program
{
	public static void Main(string[] args)
	{
		CreateHostBuilder(args).Build().Run();
	}

	public static IHostBuilder CreateHostBuilder(string[] args) =>
		Host.CreateDefaultBuilder(args)
			.ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			})
			.ConfigureAppConfiguration((hostContext, config) =>
			{
				config
					.AddJsonFile("appsettings.json", optional: false)
					.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true)
#if DEBUG
					.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.local.json", optional: true)
#endif
					.AddEnvironmentVariables();
			});

}
