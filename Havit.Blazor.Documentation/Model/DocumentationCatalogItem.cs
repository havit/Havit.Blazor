namespace Havit.Blazor.Documentation.Model;

/// <summary>
/// Represents a single item in the documentation catalog (component, concept, type, etc.).
/// When <see cref="Level"/> is not specified (0), it is auto-computed from the number of '&gt;' characters in <see cref="Title"/>.
/// </summary>
public record DocumentationCatalogItem
{
	public string Href { get; }
	public string Title { get; }
	public string Keywords { get; }
	public int Level { get; }

	public DocumentationCatalogItem(string href, string title, string keywords, int level = 0)
	{
		Href = href;
		Title = title;
		Keywords = keywords;
		Level = (level == 0) ? title.Count(c => c == '>') : level;
	}
}
