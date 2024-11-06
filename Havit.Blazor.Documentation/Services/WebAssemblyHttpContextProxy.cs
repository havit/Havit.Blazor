namespace Havit.Blazor.Documentation.Services;

public class WebAssemblyHttpContextProxy : IHttpContextProxy
{
	public string GetCookieValue(string key) => throw new NotSupportedException();
}
