# HxMarkdown_Demo_CustomCss.razor

```razor
<HxMarkdown Content="@_markdown" TableCssClass="table table-striped" BlockquoteCssClass="blockquote border-start border-3 ps-3" ImageCssClass="img-thumbnail" />

@code {
	private string _markdown = """
		| Header 1 | Header 2 |
		|----------|----------|
		| Cell A   | Cell B   |
		| Cell C   | Cell D   |

		> A styled blockquote with custom CSS.
		""";
}

```
