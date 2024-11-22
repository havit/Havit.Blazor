namespace Havit.Blazor.Documentation.Services;

public class WebAssemblyHttpContextProxy : IHttpContextProxy
{
	public bool IsSupported() => false;
	public string GetCookieValue(string key) => throw new NotSupportedException();
}
