namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// A single block-level element parsed from markdown.
/// </summary>
internal class MarkdownBlock
{
	public MarkdownBlockType Type { get; set; }

	/// <summary>
	/// Raw text lines of the block (before inline parsing).
	/// </summary>
	public List<string> Lines { get; set; } = new();

	/// <summary>
	/// Heading level (1-6). Only relevant for <see cref="MarkdownBlockType.Heading"/>.
	/// </summary>
	public int HeadingLevel { get; set; }

	/// <summary>
	/// Language hint for code blocks (e.g. "csharp"). Only relevant for <see cref="MarkdownBlockType.CodeBlock"/>.
	/// </summary>
	public string CodeLanguage { get; set; }

	/// <summary>
	/// Child blocks (e.g. list items contain paragraph blocks).
	/// </summary>
	public List<MarkdownBlock> Children { get; set; }
}
