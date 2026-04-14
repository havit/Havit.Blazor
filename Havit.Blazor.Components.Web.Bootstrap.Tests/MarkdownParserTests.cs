using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class MarkdownParserTests
{
	private static MarkdownRenderOptions DefaultOptions => new MarkdownRenderOptions
	{
		SanitizeHtml = true,
		TableCssClass = "table",
		BlockquoteCssClass = "blockquote",
		ImageCssClass = "img-fluid"
	};

	#region Null / Empty / Whitespace

	[TestMethod]
	public void MarkdownParser_NullInput_ReturnsEmpty()
	{
		var result = MarkdownParser.ToHtml(null, DefaultOptions);
		Assert.AreEqual(string.Empty, result);
	}

	[TestMethod]
	public void MarkdownParser_EmptyInput_ReturnsEmpty()
	{
		var result = MarkdownParser.ToHtml("", DefaultOptions);
		Assert.AreEqual(string.Empty, result);
	}

	[TestMethod]
	public void MarkdownParser_WhitespaceOnlyInput_ReturnsEmpty()
	{
		var result = MarkdownParser.ToHtml("   \n  \n  ", DefaultOptions);
		Assert.AreEqual(string.Empty, result);
	}

	#endregion

	#region Paragraphs

	[TestMethod]
	public void MarkdownParser_SimpleText_RendersAsParagraph()
	{
		var result = MarkdownParser.ToHtml("Hello world", DefaultOptions);
		Assert.AreEqual("<p>Hello world</p>", result);
	}

	[TestMethod]
	public void MarkdownParser_TwoParagraphs_SeparatedByBlankLine()
	{
		var result = MarkdownParser.ToHtml("First paragraph\n\nSecond paragraph", DefaultOptions);
		Assert.AreEqual("<p>First paragraph</p><p>Second paragraph</p>", result);
	}

	[TestMethod]
	public void MarkdownParser_MultipleLinesWithinParagraph_JoinedWithSpace()
	{
		var result = MarkdownParser.ToHtml("Line one\nLine two", DefaultOptions);
		Assert.AreEqual("<p>Line one Line two</p>", result);
	}

	#endregion

	#region Headings

	[TestMethod]
	[DataRow("# Heading 1", "<h1>Heading 1</h1>", DisplayName = "H1")]
	[DataRow("## Heading 2", "<h2>Heading 2</h2>", DisplayName = "H2")]
	[DataRow("### Heading 3", "<h3>Heading 3</h3>", DisplayName = "H3")]
	[DataRow("#### Heading 4", "<h4>Heading 4</h4>", DisplayName = "H4")]
	[DataRow("##### Heading 5", "<h5>Heading 5</h5>", DisplayName = "H5")]
	[DataRow("###### Heading 6", "<h6>Heading 6</h6>", DisplayName = "H6")]
	public void MarkdownParser_Headings_RenderedCorrectly(string markdown, string expected)
	{
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.AreEqual(expected, result);
	}

	[TestMethod]
	public void MarkdownParser_HeadingWithTrailingHashes_Stripped()
	{
		var result = MarkdownParser.ToHtml("## Heading ##", DefaultOptions);
		Assert.AreEqual("<h2>Heading</h2>", result);
	}

	[TestMethod]
	public void MarkdownParser_HashWithoutSpace_NotAHeading()
	{
		var result = MarkdownParser.ToHtml("#NotAHeading", DefaultOptions);
		Assert.AreEqual("<p>#NotAHeading</p>", result);
	}

	#endregion

	#region Code Blocks

	[TestMethod]
	public void MarkdownParser_FencedCodeBlock_RendersPreCode()
	{
		var markdown = "```\nvar x = 1;\n```";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.AreEqual("<pre><code>var x = 1;</code></pre>", result);
	}

	[TestMethod]
	public void MarkdownParser_FencedCodeBlock_WithLanguage()
	{
		var markdown = "```csharp\nvar x = 1;\n```";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.AreEqual("<pre><code class=\"language-csharp\">var x = 1;</code></pre>", result);
	}

	[TestMethod]
	public void MarkdownParser_FencedCodeBlock_HtmlEncoded()
	{
		var markdown = "```\n<div>test</div>\n```";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("&lt;div&gt;test&lt;/div&gt;", result);
	}

	[TestMethod]
	public void MarkdownParser_TildeFencedCodeBlock_RendersPreCode()
	{
		var markdown = "~~~\ncode here\n~~~";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.AreEqual("<pre><code>code here</code></pre>", result);
	}

	#endregion

	#region Blockquotes

	[TestMethod]
	public void MarkdownParser_Blockquote_RenderedWithClass()
	{
		var result = MarkdownParser.ToHtml("> This is a quote", DefaultOptions);
		Assert.AreEqual("<blockquote class=\"blockquote\"><p>This is a quote</p></blockquote>", result);
	}

	[TestMethod]
	public void MarkdownParser_Blockquote_CustomCssClass()
	{
		var options = new MarkdownRenderOptions { SanitizeHtml = true, BlockquoteCssClass = "custom-quote" };
		var result = MarkdownParser.ToHtml("> Quote text", options);
		Assert.Contains("class=\"custom-quote\"", result);
	}

	[TestMethod]
	public void MarkdownParser_MultilineBlockquote_JoinedCorrectly()
	{
		var markdown = "> Line one\n> Line two";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("Line one", result);
		Assert.Contains("Line two", result);
	}

	#endregion

	#region Horizontal Rules

	[TestMethod]
	[DataRow("---", DisplayName = "Dashes")]
	[DataRow("***", DisplayName = "Asterisks")]
	[DataRow("___", DisplayName = "Underscores")]
	[DataRow("- - -", DisplayName = "Spaced dashes")]
	public void MarkdownParser_HorizontalRule_RendersHr(string markdown)
	{
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.AreEqual("<hr />", result);
	}

	#endregion

	#region Unordered Lists

	[TestMethod]
	public void MarkdownParser_UnorderedList_WithDash()
	{
		var markdown = "- Item 1\n- Item 2\n- Item 3";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.AreEqual("<ul><li>Item 1</li><li>Item 2</li><li>Item 3</li></ul>", result);
	}

	[TestMethod]
	public void MarkdownParser_UnorderedList_WithAsterisk()
	{
		var markdown = "* Item A\n* Item B";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.AreEqual("<ul><li>Item A</li><li>Item B</li></ul>", result);
	}

	[TestMethod]
	public void MarkdownParser_UnorderedList_WithPlus()
	{
		var markdown = "+ First\n+ Second";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.AreEqual("<ul><li>First</li><li>Second</li></ul>", result);
	}

	#endregion

	#region Ordered Lists

	[TestMethod]
	public void MarkdownParser_OrderedList_RenderedCorrectly()
	{
		var markdown = "1. First\n2. Second\n3. Third";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.AreEqual("<ol><li>First</li><li>Second</li><li>Third</li></ol>", result);
	}

	#endregion

	#region Tables

	[TestMethod]
	public void MarkdownParser_Table_RenderedWithBootstrapClass()
	{
		var markdown = "| Header 1 | Header 2 |\n|---|---|\n| Cell 1 | Cell 2 |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);

		Assert.Contains("<table class=\"table\">", result);
		Assert.Contains("<th>Header 1</th>", result);
		Assert.Contains("<th>Header 2</th>", result);
		Assert.Contains("<td>Cell 1</td>", result);
		Assert.Contains("<td>Cell 2</td>", result);
	}

	[TestMethod]
	public void MarkdownParser_Table_CustomCssClass()
	{
		var options = new MarkdownRenderOptions { SanitizeHtml = true, TableCssClass = "table table-striped" };
		var markdown = "| A | B |\n|---|---|\n| 1 | 2 |";
		var result = MarkdownParser.ToHtml(markdown, options);
		Assert.Contains("class=\"table table-striped\"", result);
	}

	[TestMethod]
	public void MarkdownParser_Table_MultipleDataRows()
	{
		var markdown = "| Name | Value |\n|---|---|\n| A | 1 |\n| B | 2 |\n| C | 3 |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);

		Assert.Contains("<thead>", result);
		Assert.Contains("<tbody>", result);
		Assert.Contains("<td>A</td>", result);
		Assert.Contains("<td>3</td>", result);
	}

	#endregion

	#region Inline Elements

	[TestMethod]
	public void MarkdownParser_BoldWithDoubleAsterisks_RendersStrong()
	{
		var result = MarkdownParser.ToHtml("This is **bold** text", DefaultOptions);
		Assert.AreEqual("<p>This is <strong>bold</strong> text</p>", result);
	}

	[TestMethod]
	public void MarkdownParser_BoldWithDoubleUnderscores_RendersStrong()
	{
		var result = MarkdownParser.ToHtml("This is __bold__ text", DefaultOptions);
		Assert.AreEqual("<p>This is <strong>bold</strong> text</p>", result);
	}

	[TestMethod]
	public void MarkdownParser_ItalicWithSingleAsterisk_RendersEm()
	{
		var result = MarkdownParser.ToHtml("This is *italic* text", DefaultOptions);
		Assert.AreEqual("<p>This is <em>italic</em> text</p>", result);
	}

	[TestMethod]
	public void MarkdownParser_ItalicWithSingleUnderscore_RendersEm()
	{
		var result = MarkdownParser.ToHtml("This is _italic_ text", DefaultOptions);
		Assert.AreEqual("<p>This is <em>italic</em> text</p>", result);
	}

	[TestMethod]
	public void MarkdownParser_InlineCode_RendersCodeTag()
	{
		var result = MarkdownParser.ToHtml("Use `var x = 1;` here", DefaultOptions);
		Assert.AreEqual("<p>Use <code>var x = 1;</code> here</p>", result);
	}

	[TestMethod]
	public void MarkdownParser_Strikethrough_RendersSTag()
	{
		var result = MarkdownParser.ToHtml("This is ~~deleted~~ text", DefaultOptions);
		Assert.AreEqual("<p>This is <s>deleted</s> text</p>", result);
	}

	[TestMethod]
	public void MarkdownParser_Link_RendersAnchor()
	{
		var result = MarkdownParser.ToHtml("[Click here](https://example.com)", DefaultOptions);
		Assert.AreEqual("<p><a href=\"https://example.com\">Click here</a></p>", result);
	}

	[TestMethod]
	public void MarkdownParser_LinkWithTitle_RendersTitleAttribute()
	{
		var result = MarkdownParser.ToHtml("[Link](https://example.com \"My Title\")", DefaultOptions);
		Assert.Contains("title=\"My Title\"", result);
	}

	[TestMethod]
	public void MarkdownParser_Image_RendersImgTag()
	{
		var result = MarkdownParser.ToHtml("![Alt text](https://example.com/img.png)", DefaultOptions);
		Assert.Contains("<img src=\"https://example.com/img.png\" alt=\"Alt text\"", result);
		Assert.Contains("class=\"img-fluid\"", result);
	}

	[TestMethod]
	public void MarkdownParser_Image_CustomCssClass()
	{
		var options = new MarkdownRenderOptions { SanitizeHtml = true, ImageCssClass = "custom-img" };
		var result = MarkdownParser.ToHtml("![Alt](url.png)", options);
		Assert.Contains("class=\"custom-img\"", result);
	}

	#endregion

	#region HTML Sanitization

	[TestMethod]
	public void MarkdownParser_SanitizeHtml_True_EscapesHtmlTags()
	{
		var options = new MarkdownRenderOptions { SanitizeHtml = true };
		var result = MarkdownParser.ToHtml("<script>alert('xss')</script>", options);
		Assert.Contains("&lt;script>", result);
		Assert.DoesNotContain("<script>", result);
	}

	[TestMethod]
	public void MarkdownParser_SanitizeHtml_False_PreservesHtmlTags()
	{
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("<em>italic</em>", options);
		Assert.Contains("<em>italic</em>", result);
	}

	#endregion

	#region Mixed Content

	[TestMethod]
	public void MarkdownParser_MixedContent_HeadingAndParagraph()
	{
		var markdown = "# Title\n\nSome text here.";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.StartsWith("<h1>Title</h1>", result);
		Assert.Contains("<p>Some text here.</p>", result);
	}

	[TestMethod]
	public void MarkdownParser_MixedContent_ListFollowedByParagraph()
	{
		var markdown = "- Item 1\n- Item 2\n\nParagraph text.";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("<ul>", result);
		Assert.Contains("<p>Paragraph text.</p>", result);
	}

	[TestMethod]
	public void MarkdownParser_BoldAndItalicCombined()
	{
		var result = MarkdownParser.ToHtml("***bold and italic***", DefaultOptions);
		// ** wraps *, so we get <strong><em>bold and italic</em></strong>
		Assert.Contains("<strong>", result);
		Assert.Contains("<em>", result);
	}

	#endregion

	#region Line Endings

	[TestMethod]
	public void MarkdownParser_CrLfLineEndings_ParsedCorrectly()
	{
		var markdown = "# Title\r\n\r\nParagraph text";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.AreEqual("<h1>Title</h1><p>Paragraph text</p>", result);
	}

	#endregion

	#region Inline Code - Protection and Encoding

	[TestMethod]
	public void MarkdownParser_InlineCode_MarkdownInsideCode_NotProcessed()
	{
		// Bold markers inside a code span must not be processed — content is literal
		var result = MarkdownParser.ToHtml("`**bold**`", DefaultOptions);
		Assert.AreEqual("<p><code>**bold**</code></p>", result);
	}

	[TestMethod]
	public void MarkdownParser_InlineCode_LinkInsideCode_NotProcessed()
	{
		// Link syntax inside a code span must not be rendered as a hyperlink
		var result = MarkdownParser.ToHtml("`[link](https://example.com)`", DefaultOptions);
		Assert.DoesNotContain("<a ", result);
		Assert.Contains("<code>[link](https://example.com)</code>", result);
	}

	[TestMethod]
	public void MarkdownParser_InlineCode_HtmlTagInCode_AlwaysEncoded()
	{
		// Code content must always be HTML-encoded, regardless of SanitizeHtml
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("`<script>alert(1)</script>`", options);
		Assert.DoesNotContain("<script>", result);
		Assert.Contains("<code>&lt;script>", result);
	}

	#endregion

	#region Links - Relative URL Handling

	[TestMethod]
	public void MarkdownParser_Link_BareRelativeUrl_IsPreserved()
	{
		// A bare relative URL (no leading /) must not be replaced with #
		var result = MarkdownParser.ToHtml("[Link](page.html)", DefaultOptions);
		Assert.Contains("href=\"page.html\"", result);
	}

	[TestMethod]
	public void MarkdownParser_Image_BareRelativeUrl_IsPreserved()
	{
		// A bare relative image path (no leading /) must not be replaced with #
		var result = MarkdownParser.ToHtml("![alt](photo.jpg)", DefaultOptions);
		Assert.Contains("src=\"photo.jpg\"", result);
	}

	#endregion

	#region Security - XSS and Attribute Injection

	[TestMethod]
	public void MarkdownParser_Image_AltWithQuote_AttributeInjectionPrevented()
	{
		// A " in the alt text must be encoded to prevent breaking out of the attribute and injecting new attributes.
		var result = MarkdownParser.ToHtml("![alt\" onerror=\"alert(1)](image.png)", DefaultOptions);
		// The output should have " encoded as &quot; so the attribute cannot be broken
		Assert.DoesNotContain("\" onerror=\"", result);
		Assert.Contains("alt=\"alt&quot; onerror=&quot;", result);
	}

	[TestMethod]
	public void MarkdownParser_Image_AltWithQuote_EncodedAsHtmlEntity()
	{
		var result = MarkdownParser.ToHtml("![say \"hello\"](image.png)", DefaultOptions);
		Assert.Contains("alt=\"say &quot;hello&quot;\"", result);
	}

	[TestMethod]
	public void MarkdownParser_Image_JavascriptUrlInSrc_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("![alt](javascript:alert(1))", DefaultOptions);
		Assert.DoesNotContain("javascript:", result);
	}

	[TestMethod]
	public void MarkdownParser_Image_DataUrlInSrc_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("![alt](data:text/html,payload)", DefaultOptions);
		Assert.DoesNotContain("data:", result);
	}

	[TestMethod]
	public void MarkdownParser_Image_SafeHttpsUrl_IsPreserved()
	{
		var result = MarkdownParser.ToHtml("![alt](https://example.com/img.png)", DefaultOptions);
		Assert.Contains("src=\"https://example.com/img.png\"", result);
	}

	[TestMethod]
	public void MarkdownParser_Image_SafeRelativeUrl_IsPreserved()
	{
		var result = MarkdownParser.ToHtml("![alt](/images/photo.png)", DefaultOptions);
		Assert.Contains("src=\"/images/photo.png\"", result);
	}

	[TestMethod]
	public void MarkdownParser_Image_JavascriptUrl_SanitizeHtmlFalse_IsPreserved()
	{
		// SanitizeHtml=false opts out of all sanitization; URL scheme is kept as-is.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("![alt](javascript:alert(1))", options);
		Assert.Contains("javascript:", result);
	}

	[TestMethod]
	public void MarkdownParser_Link_JavascriptUrlInHref_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("[Click here](javascript:alert(1))", DefaultOptions);
		Assert.DoesNotContain("javascript:", result);
	}

	[TestMethod]
	public void MarkdownParser_Link_DataUrlInHref_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("[Click here](data:text/html,payload)", DefaultOptions);
		Assert.DoesNotContain("data:", result);
	}

	[TestMethod]
	public void MarkdownParser_Link_SafeHttpsUrl_IsPreserved()
	{
		var result = MarkdownParser.ToHtml("[Link](https://example.com)", DefaultOptions);
		Assert.Contains("href=\"https://example.com\"", result);
	}

	[TestMethod]
	public void MarkdownParser_Link_JavascriptUrl_SanitizeHtmlFalse_IsPreserved()
	{
		// SanitizeHtml=false opts out of all sanitization; URL scheme is kept as-is.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("[Click here](javascript:alert(1))", options);
		Assert.Contains("javascript:", result);
	}

	[TestMethod]
	public void MarkdownParser_Image_ProtocolRelativeUrl_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("![alt](//evil.com/image.png)", DefaultOptions);
		Assert.DoesNotContain("//evil.com", result);
	}

	[TestMethod]
	public void MarkdownParser_Link_ProtocolRelativeUrl_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("[Link](//evil.com)", DefaultOptions);
		Assert.DoesNotContain("//evil.com", result);
	}

	#endregion
}
