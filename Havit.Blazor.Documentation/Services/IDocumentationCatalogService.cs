using Havit.Blazor.Documentation.Model;

namespace Havit.Blazor.Documentation.Services;

/// <summary>
/// Provides the documentation catalog – a list of all documented components, concepts, and types.
/// </summary>
public interface IDocumentationCatalogService
{
	/// <summary>
	/// Returns all catalog items (components, concepts, supportive types, etc.).
	/// </summary>
	IReadOnlyList<DocumentationCatalogItem> GetAll();

	/// <summary>
	/// Returns only items whose title starts with "Hx" (i.e. the actual Blazor components).
	/// </summary>
	IReadOnlyList<DocumentationCatalogItem> GetComponents();
}
