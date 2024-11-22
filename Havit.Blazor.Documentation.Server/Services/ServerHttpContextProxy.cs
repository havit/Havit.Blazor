using Havit.Blazor.Documentation.Services;

namespace Havit.Blazor.Documentation.Server.Services;

public class ServerHttpContextProxy(
	IHttpContextAccessor httpContextAccessor)
	: IHttpContextProxy
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	public string GetCookieValue(string key)
	{
		return _httpContextAccessor.HttpContext.Request.Cookies[key];
	}

	public bool IsSupported() => true;
}
