# HxMarkdown_Demo_RichContent.razor

```razor
<HxMarkdown Content="@_markdown" />

@code {
	private string _markdown = """
		# Heading 1
		## Heading 2

		A paragraph with **bold**, *italic*, ~~strikethrough~~, and `code`.

		> This is a blockquote.

		- First item
		- Second item
		- Third item

		1. Ordered one
		2. Ordered two
		3. Ordered three

		---

		Visit [Havit Blazor](https://havit.blazor.eu "Havit Blazor documentation").
		""";
}

```
