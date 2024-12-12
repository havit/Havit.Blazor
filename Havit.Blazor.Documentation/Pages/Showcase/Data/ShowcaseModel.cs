namespace Havit.Blazor.Documentation.Pages.Showcase.Data;

public record ShowcaseModel
{
	public required string Id { get; init; }
	public required string Title { get; init; }
	public required string Description { get; init; }
	public required string ImageUrl { get; init; }
	public required string Author { get; init; }
	public string AuthorUrl { get; init; }
	public string ProjectUrl { get; init; }
	public string SourceCodeUrl { get; init; }
	public string SourceCodeTitle { get; init; }

}

