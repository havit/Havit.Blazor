using Havit.Blazor.Documentation.Shared.Components.DocColorMode;

namespace Havit.Blazor.Documentation.Server;

public class DocColorModeServerResolver : IDocColorModeResolver
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public DocColorModeServerResolver(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public ColorMode GetColorMode()
	{
		var cookie = _httpContextAccessor.HttpContext?.Request?.Cookies["ColorMode"];
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
