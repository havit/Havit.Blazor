namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

/// <summary>
/// Represents a parsed block-level markdown element.
/// </summary>
internal enum MarkdownBlockType
{
	Paragraph,
	Heading,
	CodeBlock,
	Blockquote,
	UnorderedList,
	OrderedList,
	HorizontalRule,
	Table
}
