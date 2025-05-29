namespace Havit.Blazor.Documentation.Services;

public static class UrlHelper
{
	public static string RemoveFragmentFromUrl(string url)
	{
		if (url == null)
		{
			return null;
		}

		int hashIndex = url.IndexOf('#');
		return (hashIndex == -1) ? url : url.Substring(0, hashIndex);
	}
}
