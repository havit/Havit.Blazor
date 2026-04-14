# HxMarkdown_Demo_Basic.razor

```razor
<HxMarkdown Content="@_markdown" />

@code {
	private string _markdown = """
		**Hello, Markdown!** This is a paragraph with *italic* and `inline code`.
		""";
}

```
