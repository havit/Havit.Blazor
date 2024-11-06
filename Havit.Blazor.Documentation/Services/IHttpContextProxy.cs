namespace Havit.Blazor.Documentation.Services;

/// <summary>
/// Provides methods to interact with the server-side HTTP context (during prerendering).
/// Avoids direct dependency on HttpContext in Client project (WASM).
/// </summary>
public interface IHttpContextProxy
{
	string GetCookieValue(string key);
}
