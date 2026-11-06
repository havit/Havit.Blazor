namespace Havit.Blazor.Documentation.Server.Infrastructure;

/// <summary>
/// Redirects requests coming to any other hostname (alternative domains, *.azurewebsites.net, ...)
/// to the root of the primary hostname.
/// </summary>
/// <remarks>
/// Opt-in via configuration (<c>PrimaryHostnameRedirect:Hostname</c>, e.g. App Service app setting <c>PrimaryHostnameRedirect__Hostname</c>).
/// When the hostname is not configured (localhost, stage, ...), the middleware is not registered at all.
/// </remarks>
public static class PrimaryHostnameRedirectExtensions
{
	public const string HostnameConfigurationKey = "PrimaryHostnameRedirect:Hostname";

	public static IApplicationBuilder UsePrimaryHostnameRedirect(this IApplicationBuilder app, IConfiguration configuration)
	{
		var primaryHostname = configuration[HostnameConfigurationKey]?.Trim();
		if (String.IsNullOrEmpty(primaryHostname))
		{
			return app;
		}

		var redirectUrl = $"https://{primaryHostname}/";

		return app.Use(async (context, next) =>
		{
			if (!String.Equals(context.Request.Host.Host, primaryHostname, StringComparison.OrdinalIgnoreCase))
			{
				context.Response.Redirect(redirectUrl, permanent: true);
				return;
			}

			await next();
		});
	}
}
