using Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components.DocColorMode;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Server;

public class DocColorModeServerResolver : IDocColorModeResolver
{
	private readonly IHttpContextAccessor httpContextAccessor;

	public DocColorModeServerResolver(IHttpContextAccessor httpContextAccessor)
	{
		this.httpContextAccessor = httpContextAccessor;
	}

	public ColorMode GetColorMode()
	{
		var cookie = httpContextAccessor.HttpContext?.Request?.Cookies["ColorMode"];
		if (cookie == null)
		{
			return ColorMode.Auto;
		}
		if (Enum.TryParse<ColorMode>(cookie, ignoreCase: true, out var mode))
		{
			return mode;
		}
		return ColorMode.Auto;
	}
}
