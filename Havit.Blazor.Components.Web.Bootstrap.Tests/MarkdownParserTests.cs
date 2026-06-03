using Havit.Blazor.Components.Web.Bootstrap.Internal;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

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

	[Fact]
	public void MarkdownParser_NullInput_ReturnsEmpty()
	{
		var result = MarkdownParser.ToHtml(null, DefaultOptions);
		Assert.Equal(string.Empty, result);
	}

	[Fact]
	public void MarkdownParser_EmptyInput_ReturnsEmpty()
	{
		var result = MarkdownParser.ToHtml("", DefaultOptions);
		Assert.Equal(string.Empty, result);
	}

	[Fact]
	public void MarkdownParser_WhitespaceOnlyInput_ReturnsEmpty()
	{
		var result = MarkdownParser.ToHtml("   \n  \n  ", DefaultOptions);
		Assert.Equal(string.Empty, result);
	}

	#endregion

	#region Paragraphs

	[Fact]
	public void MarkdownParser_SimpleText_RendersAsParagraph()
	{
		var result = MarkdownParser.ToHtml("Hello world", DefaultOptions);
		Assert.Equal("<p>Hello world</p>", result);
	}

	[Fact]
	public void MarkdownParser_TwoParagraphs_SeparatedByBlankLine()
	{
		var result = MarkdownParser.ToHtml("First paragraph\n\nSecond paragraph", DefaultOptions);
		Assert.Equal("<p>First paragraph</p><p>Second paragraph</p>", result);
	}

	[Fact]
	public void MarkdownParser_MultipleLinesWithinParagraph_JoinedWithSpace()
	{
		var result = MarkdownParser.ToHtml("Line one\nLine two", DefaultOptions);
		Assert.Equal("<p>Line one Line two</p>", result);
	}

	#endregion

	#region Headings

	[Theory]
	[InlineData("# Heading 1", "<h1>Heading 1</h1>")]
	[InlineData("## Heading 2", "<h2>Heading 2</h2>")]
	[InlineData("### Heading 3", "<h3>Heading 3</h3>")]
	[InlineData("#### Heading 4", "<h4>Heading 4</h4>")]
	[InlineData("##### Heading 5", "<h5>Heading 5</h5>")]
	[InlineData("###### Heading 6", "<h6>Heading 6</h6>")]
	public void MarkdownParser_Headings_RenderedCorrectly(string markdown, string expected)
	{
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal(expected, result);
	}

	[Fact]
	public void MarkdownParser_HeadingWithTrailingHashes_Stripped()
	{
		var result = MarkdownParser.ToHtml("## Heading ##", DefaultOptions);
		Assert.Equal("<h2>Heading</h2>", result);
	}

	[Fact]
	public void MarkdownParser_HashWithoutSpace_NotAHeading()
	{
		var result = MarkdownParser.ToHtml("#NotAHeading", DefaultOptions);
		Assert.Equal("<p>#NotAHeading</p>", result);
	}

	#endregion

	#region Code Blocks

	[Fact]
	public void MarkdownParser_FencedCodeBlock_RendersPreCode()
	{
		var markdown = "```\nvar x = 1;\n```";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<pre><code>var x = 1;</code></pre>", result);
	}

	[Fact]
	public void MarkdownParser_FencedCodeBlock_WithLanguage()
	{
		var markdown = "```csharp\nvar x = 1;\n```";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<pre><code class=\"language-csharp\">var x = 1;</code></pre>", result);
	}

	[Fact]
	public void MarkdownParser_FencedCodeBlock_HtmlEncoded()
	{
		var markdown = "```\n<div>test</div>\n```";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("&lt;div&gt;test&lt;/div&gt;", result);
	}

	[Fact]
	public void MarkdownParser_TildeFencedCodeBlock_RendersPreCode()
	{
		var markdown = "~~~\ncode here\n~~~";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<pre><code>code here</code></pre>", result);
	}

	[Fact]
	public void MarkdownParser_FencedCodeBlock_LongerOpeningFence_NotClosedByShorterFence()
	{
		// Opening fence of 4 backticks requires closing fence of >= 4 backticks (CommonMark).
		// A 3-backtick closing line should appear literally in the code content, not close the block.
		var markdown = "````\nvar x = 1;\n```\nvar y = 2;\n````";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<pre><code>var x = 1;\n```\nvar y = 2;</code></pre>", result);
	}

	[Fact]
	public void MarkdownParser_FencedCodeBlock_LongerClosingFence_ClosesBlock()
	{
		// A closing fence longer than the opening fence must also close the block (CommonMark).
		var markdown = "```\nvar x = 1;\n`````";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<pre><code>var x = 1;</code></pre>", result);
	}

	[Fact]
	public void MarkdownParser_FencedCodeBlock_FourBackticksWithLanguage()
	{
		// 4-backtick opening with language hint should be supported
		var markdown = "````csharp\nvar x = 1;\n````";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<pre><code class=\"language-csharp\">var x = 1;</code></pre>", result);
	}

	#endregion

	#region Blockquotes

	[Fact]
	public void MarkdownParser_Blockquote_RenderedWithClass()
	{
		var result = MarkdownParser.ToHtml("> This is a quote", DefaultOptions);
		Assert.Equal("<blockquote class=\"blockquote\"><p>This is a quote</p></blockquote>", result);
	}

	[Fact]
	public void MarkdownParser_Blockquote_CustomCssClass()
	{
		var options = new MarkdownRenderOptions { SanitizeHtml = true, BlockquoteCssClass = "custom-quote" };
		var result = MarkdownParser.ToHtml("> Quote text", options);
		Assert.Contains("class=\"custom-quote\"", result);
	}

	[Fact]
	public void MarkdownParser_MultilineBlockquote_JoinedCorrectly()
	{
		var markdown = "> Line one\n> Line two";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("Line one", result);
		Assert.Contains("Line two", result);
	}

	[Fact]
	public void MarkdownParser_NestedBlockquote_TwoLevels()
	{
		// >> nested should produce a blockquote inside a blockquote
		var result = MarkdownParser.ToHtml(">> nested", DefaultOptions);
		Assert.Equal("<blockquote class=\"blockquote\"><blockquote class=\"blockquote\"><p>nested</p></blockquote></blockquote>", result);
	}

	[Fact]
	public void MarkdownParser_NestedBlockquote_ThreeLevels()
	{
		// >>> triple-nested
		var result = MarkdownParser.ToHtml(">>> triple", DefaultOptions);
		Assert.Equal("<blockquote class=\"blockquote\"><blockquote class=\"blockquote\"><blockquote class=\"blockquote\"><p>triple</p></blockquote></blockquote></blockquote>", result);
	}

	[Fact]
	public void MarkdownParser_NestedBlockquote_WithSpaceSyntax()
	{
		// CommonMark: "> > nested" (space between markers) also produces nested blockquote
		var result = MarkdownParser.ToHtml("> > nested", DefaultOptions);
		Assert.Equal("<blockquote class=\"blockquote\"><blockquote class=\"blockquote\"><p>nested</p></blockquote></blockquote>", result);
	}

	#endregion

	#region Horizontal Rules

	[Theory]
	[InlineData("---")]
	[InlineData("***")]
	[InlineData("___")]
	[InlineData("- - -")]
	public void MarkdownParser_HorizontalRule_RendersHr(string markdown)
	{
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<hr />", result);
	}

	#endregion

	#region Unordered Lists

	[Fact]
	public void MarkdownParser_UnorderedList_WithDash()
	{
		var markdown = "- Item 1\n- Item 2\n- Item 3";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<ul><li>Item 1</li><li>Item 2</li><li>Item 3</li></ul>", result);
	}

	[Fact]
	public void MarkdownParser_UnorderedList_WithAsterisk()
	{
		var markdown = "* Item A\n* Item B";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<ul><li>Item A</li><li>Item B</li></ul>", result);
	}

	[Fact]
	public void MarkdownParser_UnorderedList_WithPlus()
	{
		var markdown = "+ First\n+ Second";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<ul><li>First</li><li>Second</li></ul>", result);
	}

	#endregion

	#region Ordered Lists

	[Fact]
	public void MarkdownParser_OrderedList_RenderedCorrectly()
	{
		var markdown = "1. First\n2. Second\n3. Third";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<ol><li>First</li><li>Second</li><li>Third</li></ol>", result);
	}

	#endregion

	#region Tables

	[Fact]
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

	[Fact]
	public void MarkdownParser_Table_CustomCssClass()
	{
		var options = new MarkdownRenderOptions { SanitizeHtml = true, TableCssClass = "table table-striped" };
		var markdown = "| A | B |\n|---|---|\n| 1 | 2 |";
		var result = MarkdownParser.ToHtml(markdown, options);
		Assert.Contains("class=\"table table-striped\"", result);
	}

	[Fact]
	public void MarkdownParser_Table_MultipleDataRows()
	{
		var markdown = "| Name | Value |\n|---|---|\n| A | 1 |\n| B | 2 |\n| C | 3 |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);

		Assert.Contains("<thead>", result);
		Assert.Contains("<tbody>", result);
		Assert.Contains("<td>A</td>", result);
		Assert.Contains("<td>3</td>", result);
	}

	[Fact]
	public void MarkdownParser_Table_AlignmentLeft_EmitsStyleAttribute()
	{
		// :--- means left alignment
		var markdown = "| A | B |\n| :--- | :--- |\n| 1 | 2 |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("<th style=\"text-align:left\">", result);
		Assert.Contains("<td style=\"text-align:left\">", result);
	}

	[Fact]
	public void MarkdownParser_Table_AlignmentRight_EmitsStyleAttribute()
	{
		// ---: means right alignment
		var markdown = "| A | B |\n| ---: | ---: |\n| 1 | 2 |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("<th style=\"text-align:right\">", result);
		Assert.Contains("<td style=\"text-align:right\">", result);
	}

	[Fact]
	public void MarkdownParser_Table_AlignmentCenter_EmitsStyleAttribute()
	{
		// :---: means center alignment
		var markdown = "| A | B |\n| :---: | :---: |\n| 1 | 2 |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("<th style=\"text-align:center\">", result);
		Assert.Contains("<td style=\"text-align:center\">", result);
	}

	[Fact]
	public void MarkdownParser_Table_AlignmentNone_NoStyleAttribute()
	{
		// --- with no colons means no alignment (default)
		var markdown = "| A |\n| --- |\n| 1 |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.DoesNotContain("style=", result);
	}

	[Fact]
	public void MarkdownParser_Table_MixedAlignments_PerColumn()
	{
		// Each column may have a different alignment
		var markdown = "| Left | Center | Right | None |\n| :--- | :---: | ---: | --- |\n| a | b | c | d |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("<th style=\"text-align:left\">Left</th>", result);
		Assert.Contains("<th style=\"text-align:center\">Center</th>", result);
		Assert.Contains("<th style=\"text-align:right\">Right</th>", result);
		Assert.Contains("<th>None</th>", result);
		Assert.Contains("<td style=\"text-align:left\">a</td>", result);
		Assert.Contains("<td style=\"text-align:center\">b</td>", result);
		Assert.Contains("<td style=\"text-align:right\">c</td>", result);
		Assert.Contains("<td>d</td>", result);
	}

	#endregion

	#region Inline Elements

	[Fact]
	public void MarkdownParser_BoldWithDoubleAsterisks_RendersStrong()
	{
		var result = MarkdownParser.ToHtml("This is **bold** text", DefaultOptions);
		Assert.Equal("<p>This is <strong>bold</strong> text</p>", result);
	}

	[Fact]
	public void MarkdownParser_BoldWithDoubleUnderscores_RendersStrong()
	{
		var result = MarkdownParser.ToHtml("This is __bold__ text", DefaultOptions);
		Assert.Equal("<p>This is <strong>bold</strong> text</p>", result);
	}

	[Fact]
	public void MarkdownParser_ItalicWithSingleAsterisk_RendersEm()
	{
		var result = MarkdownParser.ToHtml("This is *italic* text", DefaultOptions);
		Assert.Equal("<p>This is <em>italic</em> text</p>", result);
	}

	[Fact]
	public void MarkdownParser_ItalicWithSingleUnderscore_RendersEm()
	{
		var result = MarkdownParser.ToHtml("This is _italic_ text", DefaultOptions);
		Assert.Equal("<p>This is <em>italic</em> text</p>", result);
	}

	[Fact]
	public void MarkdownParser_InlineCode_RendersCodeTag()
	{
		var result = MarkdownParser.ToHtml("Use `var x = 1;` here", DefaultOptions);
		Assert.Equal("<p>Use <code>var x = 1;</code> here</p>", result);
	}

	[Fact]
	public void MarkdownParser_Strikethrough_RendersSTag()
	{
		var result = MarkdownParser.ToHtml("This is ~~deleted~~ text", DefaultOptions);
		Assert.Equal("<p>This is <s>deleted</s> text</p>", result);
	}

	[Fact]
	public void MarkdownParser_Link_RendersAnchor()
	{
		var result = MarkdownParser.ToHtml("[Click here](https://example.com)", DefaultOptions);
		Assert.Equal("<p><a href=\"https://example.com\">Click here</a></p>", result);
	}

	[Fact]
	public void MarkdownParser_LinkWithTitle_RendersTitleAttribute()
	{
		var result = MarkdownParser.ToHtml("[Link](https://example.com \"My Title\")", DefaultOptions);
		Assert.Contains("title=\"My Title\"", result);
	}

	[Fact]
	public void MarkdownParser_Image_RendersImgTag()
	{
		var result = MarkdownParser.ToHtml("![Alt text](https://example.com/img.png)", DefaultOptions);
		Assert.Contains("<img src=\"https://example.com/img.png\" alt=\"Alt text\"", result);
		Assert.Contains("class=\"img-fluid\"", result);
	}

	[Fact]
	public void MarkdownParser_Image_CustomCssClass()
	{
		var options = new MarkdownRenderOptions { SanitizeHtml = true, ImageCssClass = "custom-img" };
		var result = MarkdownParser.ToHtml("![Alt](url.png)", options);
		Assert.Contains("class=\"custom-img\"", result);
	}

	#endregion

	#region HTML Sanitization

	[Fact]
	public void MarkdownParser_SanitizeHtml_True_EscapesHtmlTags()
	{
		var options = new MarkdownRenderOptions { SanitizeHtml = true };
		var result = MarkdownParser.ToHtml("<script>alert('xss')</script>", options);
		Assert.Contains("&lt;script>", result);
		Assert.DoesNotContain("<script>", result);
	}

	[Fact]
	public void MarkdownParser_SanitizeHtml_False_PreservesHtmlTags()
	{
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("<em>italic</em>", options);
		Assert.Contains("<em>italic</em>", result);
	}

	#endregion

	#region Mixed Content

	[Fact]
	public void MarkdownParser_MixedContent_HeadingAndParagraph()
	{
		var markdown = "# Title\n\nSome text here.";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.StartsWith("<h1>Title</h1>", result);
		Assert.Contains("<p>Some text here.</p>", result);
	}

	[Fact]
	public void MarkdownParser_MixedContent_ListFollowedByParagraph()
	{
		var markdown = "- Item 1\n- Item 2\n\nParagraph text.";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("<ul>", result);
		Assert.Contains("<p>Paragraph text.</p>", result);
	}

	[Fact]
	public void MarkdownParser_BoldAndItalicCombined()
	{
		var result = MarkdownParser.ToHtml("***bold and italic***", DefaultOptions);
		// ** wraps *, so we get <strong><em>bold and italic</em></strong>
		Assert.Contains("<strong>", result);
		Assert.Contains("<em>", result);
	}

	[Fact]
	public void MarkdownParser_TableImmediatelyAfterParagraph_WithoutBlankLine_ParsedSeparately()
	{
		// Table immediately after paragraph text (no blank line separator) must be parsed as a separate table block
		var markdown = "Some text\n| A | B |\n|---|---|\n| 1 | 2 |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("<p>Some text</p>", result);
		Assert.Contains("<table", result);
		Assert.Contains("<th>A</th>", result);
		Assert.Contains("<td>1</td>", result);
	}

	#endregion

	#region Line Endings

	[Fact]
	public void MarkdownParser_CrLfLineEndings_ParsedCorrectly()
	{
		var markdown = "# Title\r\n\r\nParagraph text";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Equal("<h1>Title</h1><p>Paragraph text</p>", result);
	}

	#endregion

	#region Inline Code - Protection and Encoding

	[Fact]
	public void MarkdownParser_InlineCode_MarkdownInsideCode_NotProcessed()
	{
		// Bold markers inside a code span must not be processed — content is literal
		var result = MarkdownParser.ToHtml("`**bold**`", DefaultOptions);
		Assert.Equal("<p><code>**bold**</code></p>", result);
	}

	[Fact]
	public void MarkdownParser_InlineCode_LinkInsideCode_NotProcessed()
	{
		// Link syntax inside a code span must not be rendered as a hyperlink
		var result = MarkdownParser.ToHtml("`[link](https://example.com)`", DefaultOptions);
		Assert.DoesNotContain("<a ", result);
		Assert.Contains("<code>[link](https://example.com)</code>", result);
	}

	[Fact]
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

	[Fact]
	public void MarkdownParser_Link_BareRelativeUrl_IsPreserved()
	{
		// A bare relative URL (no leading /) must not be replaced with #
		var result = MarkdownParser.ToHtml("[Link](page.html)", DefaultOptions);
		Assert.Contains("href=\"page.html\"", result);
	}

	[Fact]
	public void MarkdownParser_Image_BareRelativeUrl_IsPreserved()
	{
		// A bare relative image path (no leading /) must not be replaced with #
		var result = MarkdownParser.ToHtml("![alt](photo.jpg)", DefaultOptions);
		Assert.Contains("src=\"photo.jpg\"", result);
	}

	#endregion

	#region Security - XSS and Attribute Injection

	[Fact]
	public void MarkdownParser_Image_AltWithQuote_AttributeInjectionPrevented()
	{
		// A " in the alt text must be encoded to prevent breaking out of the attribute and injecting new attributes.
		var result = MarkdownParser.ToHtml("![alt\" onerror=\"alert(1)](image.png)", DefaultOptions);
		// The output should have " encoded as &quot; so the attribute cannot be broken
		Assert.DoesNotContain("\" onerror=\"", result);
		Assert.Contains("alt=\"alt&quot; onerror=&quot;", result);
	}

	[Fact]
	public void MarkdownParser_Image_AltWithQuote_EncodedAsHtmlEntity()
	{
		var result = MarkdownParser.ToHtml("![say \"hello\"](image.png)", DefaultOptions);
		Assert.Contains("alt=\"say &quot;hello&quot;\"", result);
	}

	[Fact]
	public void MarkdownParser_Image_JavascriptUrlInSrc_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("![alt](javascript:alert(1))", DefaultOptions);
		Assert.DoesNotContain("javascript:", result);
	}

	[Fact]
	public void MarkdownParser_Image_DataUrlInSrc_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("![alt](data:text/html,payload)", DefaultOptions);
		Assert.DoesNotContain("data:", result);
	}

	[Fact]
	public void MarkdownParser_Image_SafeHttpsUrl_IsPreserved()
	{
		var result = MarkdownParser.ToHtml("![alt](https://example.com/img.png)", DefaultOptions);
		Assert.Contains("src=\"https://example.com/img.png\"", result);
	}

	[Fact]
	public void MarkdownParser_Image_SafeRelativeUrl_IsPreserved()
	{
		var result = MarkdownParser.ToHtml("![alt](/images/photo.png)", DefaultOptions);
		Assert.Contains("src=\"/images/photo.png\"", result);
	}

	[Fact]
	public void MarkdownParser_Image_JavascriptUrl_SanitizeHtmlFalse_IsPreserved()
	{
		// SanitizeHtml=false opts out of all sanitization; URL scheme is kept as-is.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("![alt](javascript:alert(1))", options);
		Assert.Contains("javascript:", result);
	}

	[Fact]
	public void MarkdownParser_Link_JavascriptUrlInHref_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("[Click here](javascript:alert(1))", DefaultOptions);
		Assert.DoesNotContain("javascript:", result);
	}

	[Fact]
	public void MarkdownParser_Link_DataUrlInHref_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("[Click here](data:text/html,payload)", DefaultOptions);
		Assert.DoesNotContain("data:", result);
	}

	[Fact]
	public void MarkdownParser_Link_SafeHttpsUrl_IsPreserved()
	{
		var result = MarkdownParser.ToHtml("[Link](https://example.com)", DefaultOptions);
		Assert.Contains("href=\"https://example.com\"", result);
	}

	[Fact]
	public void MarkdownParser_Link_JavascriptUrl_SanitizeHtmlFalse_IsPreserved()
	{
		// SanitizeHtml=false opts out of all sanitization; URL scheme is kept as-is.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("[Click here](javascript:alert(1))", options);
		Assert.Contains("javascript:", result);
	}

	[Fact]
	public void MarkdownParser_Image_ProtocolRelativeUrl_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("![alt](//evil.com/image.png)", DefaultOptions);
		Assert.DoesNotContain("//evil.com", result);
	}

	[Fact]
	public void MarkdownParser_Link_ProtocolRelativeUrl_IsBlocked()
	{
		var result = MarkdownParser.ToHtml("[Link](//evil.com)", DefaultOptions);
		Assert.DoesNotContain("//evil.com", result);
	}

	[Fact]
	public void MarkdownParser_Image_UrlWithQuote_AttributeInjectionPrevented()
	{
		// A " in the URL must be encoded to prevent breaking out of src="..." and injecting attributes.
		var result = MarkdownParser.ToHtml("![alt](url\"onclick=\"alert(1))", DefaultOptions);
		Assert.DoesNotContain("\" onclick=\"", result);
		Assert.DoesNotContain("\"onclick=\"", result);
		// The " chars in the URL are encoded as &quot; so the attribute cannot be broken.
		Assert.Contains("src=\"url&quot;onclick=&quot;alert(1\"", result);
	}

	[Fact]
	public void MarkdownParser_Image_UrlWithQuote_SanitizeHtmlFalse_AttributeInjectionPrevented()
	{
		// Even with SanitizeHtml=false, " in URL must be encoded to produce valid HTML attributes.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("![alt](url\"onclick=\"alert(1))", options);
		Assert.DoesNotContain("\" onclick=\"", result);
		Assert.DoesNotContain("\"onclick=\"", result);
		Assert.Contains("src=\"url&quot;onclick=&quot;alert(1\"", result);
	}

	[Fact]
	public void MarkdownParser_Image_AltWithQuote_SanitizeHtmlFalse_AttributeInjectionPrevented()
	{
		// Even with SanitizeHtml=false, " in alt must be encoded to produce valid HTML attributes.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("![alt\" onerror=\"alert(1)](image.png)", options);
		Assert.DoesNotContain("\" onerror=\"", result);
		Assert.Contains("alt=\"alt&quot; onerror=&quot;alert(1)\"", result);
	}

	[Fact]
	public void MarkdownParser_Link_UrlWithQuote_AttributeInjectionPrevented()
	{
		// A " in the href URL must be encoded to prevent breaking out of href="..." and injecting attributes.
		var result = MarkdownParser.ToHtml("[text](url\"onclick=\"alert(1))", DefaultOptions);
		Assert.DoesNotContain("\" onclick=\"", result);
		Assert.DoesNotContain("\"onclick=\"", result);
		// The " chars in the URL are encoded as &quot; so the attribute cannot be broken.
		Assert.Contains("href=\"url&quot;onclick=&quot;alert(1\"", result);
	}

	[Fact]
	public void MarkdownParser_Link_UrlWithQuote_SanitizeHtmlFalse_AttributeInjectionPrevented()
	{
		// Even with SanitizeHtml=false, " in URL must be encoded to produce valid HTML attributes.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("[text](url\"onclick=\"alert(1))", options);
		Assert.DoesNotContain("\" onclick=\"", result);
		Assert.DoesNotContain("\"onclick=\"", result);
		Assert.Contains("href=\"url&quot;onclick=&quot;alert(1\"", result);
	}

	[Fact]
	public void MarkdownParser_TableHeaderWithoutTrailingPipe_AfterParagraph_NotSplitIncorrectly()
	{
		// Header line without trailing pipe (| A | B) must NOT break the paragraph,
		// because TryParseTable requires both leading and trailing pipes.
		var markdown = "Some text\n| A | B\n|---|---|\n| 1 | 2 |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("Some text", result);
		Assert.Contains("| A | B", result);
		Assert.DoesNotContain("<table", result);
	}

	[Fact]
	public void MarkdownParser_TableHeaderWithTrailingWhitespace_AfterParagraph_ParsedSeparately()
	{
		// Trailing whitespace after the last | must not prevent table detection.
		// ParseParagraph must Trim() consistently with TryParseTable.
		var markdown = "Some text\n| A | B |   \n|---|---|\n| 1 | 2 |";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("<p>Some text</p>", result);
		Assert.Contains("<table", result);
	}

	[Fact]
	public void MarkdownParser_LinkUrlWithMarkdownMarkers_NotMangledByEmphasis()
	{
		// Bold markers ** inside a URL must not be converted to <strong>.
		var markdown = "[click](https://x.com/**path**/y)";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("href=\"https://x.com/**path**/y\"", result);
		Assert.DoesNotContain("<strong>", result);
	}

	[Fact]
	public void MarkdownParser_ImageAltWithUnderscores_NotMangledByEmphasis()
	{
		// Underscores in image alt text must not be converted to <em>.
		var markdown = "![my_image_alt](https://example.com/img.png)";
		var result = MarkdownParser.ToHtml(markdown, DefaultOptions);
		Assert.Contains("alt=\"my_image_alt\"", result);
		Assert.DoesNotContain("<em>", result);
	}

	[Fact]
	public void MarkdownParser_LinkUrlWithAmpersand_SanitizeHtmlFalse_AmpEncoded()
	{
		// & in URLs must be encoded as &amp; in attribute values even when SanitizeHtml=false.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("[link](https://x.com?a=1&b=2)", options);
		Assert.Contains("href=\"https://x.com?a=1&amp;b=2\"", result);
	}

	[Fact]
	public void MarkdownParser_ImageUrlWithAmpersand_SanitizeHtmlFalse_AmpEncoded()
	{
		// & in image URLs must be encoded as &amp; in attribute values even when SanitizeHtml=false.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("![alt](https://x.com/img?a=1&b=2)", options);
		Assert.Contains("src=\"https://x.com/img?a=1&amp;b=2\"", result);
	}

	#endregion

	#region Naked URLs (Autolinks)

	[Fact]
	public void MarkdownParser_NakedUrl_HttpsUrl_RenderedAsLink()
	{
		var result = MarkdownParser.ToHtml("Some text with link to https://www.havit.cz/test should work.", DefaultOptions);
		Assert.Contains("<a href=\"https://www.havit.cz/test\">https://www.havit.cz/test</a>", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_HttpUrl_RenderedAsLink()
	{
		var result = MarkdownParser.ToHtml("Visit http://example.com for details.", DefaultOptions);
		Assert.Contains("<a href=\"http://example.com\">http://example.com</a>", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_TrailingPeriod_PeriodNotIncludedInUrl()
	{
		var result = MarkdownParser.ToHtml("Visit https://example.com.", DefaultOptions);
		Assert.Contains("href=\"https://example.com\"", result);
		Assert.DoesNotContain("href=\"https://example.com.\"", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_TrailingComma_CommaNotIncludedInUrl()
	{
		var result = MarkdownParser.ToHtml("Visit https://example.com, then continue.", DefaultOptions);
		Assert.Contains("href=\"https://example.com\"", result);
		Assert.DoesNotContain("href=\"https://example.com,\"", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_TrailingClosingParenthesis_ParenNotIncludedInUrl()
	{
		var result = MarkdownParser.ToHtml("(see https://example.com)", DefaultOptions);
		Assert.Contains("href=\"https://example.com\"", result);
		Assert.DoesNotContain("href=\"https://example.com)\"", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_BalancedParentheses_ParensPreservedInUrl()
	{
		// Balanced parentheses in the URL (e.g. Wikipedia disambiguation) must not be stripped.
		var result = MarkdownParser.ToHtml("See https://en.wikipedia.org/wiki/C_(programming_language) for more.", DefaultOptions);
		Assert.Contains("href=\"https://en.wikipedia.org/wiki/C_(programming_language)\"", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_AtEndOfText_RenderedAsLink()
	{
		var result = MarkdownParser.ToHtml("Link: https://example.com", DefaultOptions);
		Assert.Contains("<a href=\"https://example.com\">https://example.com</a>", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_MultipleUrls_AllRenderedAsLinks()
	{
		var result = MarkdownParser.ToHtml("See https://one.com and https://two.com for details.", DefaultOptions);
		Assert.Contains("href=\"https://one.com\"", result);
		Assert.Contains("href=\"https://two.com\"", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_InsideCodeSpan_NotProcessed()
	{
		// URLs inside a code span must not be converted to hyperlinks.
		var result = MarkdownParser.ToHtml("Use `https://example.com` as the base URL.", DefaultOptions);
		Assert.DoesNotContain("<a ", result);
		Assert.Contains("<code>https://example.com</code>", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_ExplicitLinkNotDoubleProcessed()
	{
		// An explicit [text](url) link must not also create a naked-URL link for the same URL.
		var result = MarkdownParser.ToHtml("[click here](https://example.com)", DefaultOptions);
		var count = result.Split("<a ").Length - 1;
		Assert.Equal(1, count);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_WithQueryStringAndAmpersand_CorrectlyEncoded()
	{
		var result = MarkdownParser.ToHtml("See https://example.com?a=1&b=2 for info.", DefaultOptions);
		Assert.Contains("href=\"https://example.com?a=1&amp;b=2\"", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_WithQueryStringAndAmpersand_SanitizeHtmlFalse_DisplayTextCorrectlyEncoded()
	{
		var options = new MarkdownRenderOptions
		{
			SanitizeHtml = false,
			TableCssClass = DefaultOptions.TableCssClass,
			BlockquoteCssClass = DefaultOptions.BlockquoteCssClass,
			ImageCssClass = DefaultOptions.ImageCssClass
		};

		var result = MarkdownParser.ToHtml("https://example.com?a=1&b=2", options);
		Assert.Contains("href=\"https://example.com?a=1&amp;b=2\">https://example.com?a=1&amp;b=2</a>", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_WithBoldText_UrlNotMangledByEmphasis()
	{
		// **bold** surrounding a naked URL must not corrupt the URL.
		var result = MarkdownParser.ToHtml("**Visit https://example.com now**", DefaultOptions);
		Assert.Contains("href=\"https://example.com\"", result);
		Assert.Contains("<strong>", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_NakedUrlAdjacentToExplicitLink_BothRenderedCorrectly()
	{
		// A naked URL next to an explicit Markdown link — both must produce exactly one <a> each.
		var result = MarkdownParser.ToHtml("[example](https://example.com) and https://other.com", DefaultOptions);
		Assert.Contains("href=\"https://example.com\">example</a>", result);
		Assert.Contains("href=\"https://other.com\">https://other.com</a>", result);
		var count = result.Split("<a ").Length - 1;
		Assert.Equal(2, count);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_InsideSingleQuotedHtmlAttribute_SanitizeHtmlFalse_NotAutolinked()
	{
		// When SanitizeHtml=false, raw HTML passes through. A URL inside a single-quoted
		// HTML attribute (href='https://...') must not be matched by the naked-URL regex.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("<a href='https://example.com'>link</a>", options);
		// No auto-generated double-quoted href must appear — the single-quoted attribute must pass through intact.
		Assert.DoesNotContain("<a href=\"https://example.com", result);
		// The single-quoted attribute value must be preserved in the output.
		Assert.Contains("href='https://example.com'", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_TrailingSingleQuote_QuoteNotIncludedInUrl()
	{
		// A URL that is immediately followed by a single quote must not include that quote in the href.
		var options = new MarkdownRenderOptions { SanitizeHtml = false };
		var result = MarkdownParser.ToHtml("url: https://example.com' here", options);
		Assert.Contains("href=\"https://example.com\"", result);
		Assert.DoesNotContain("href=\"https://example.com'\"", result);
		// The single quote must appear after the closing </a>, not inside the href.
		Assert.Contains("</a>'", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_IPv6_NoPath_RenderedAsLink()
	{
		// IPv6 URL with no path — the closing ] is part of the host, not trailing punctuation.
		var result = MarkdownParser.ToHtml("Connect to https://[::1] now.", DefaultOptions);
		Assert.Contains("href=\"https://[::1]\"", result);
		Assert.DoesNotContain("href=\"https://[::1\"", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_IPv6_WithPath_RenderedAsLink()
	{
		// IPv6 URL with a path — the ] is followed by the path, should be preserved.
		var result = MarkdownParser.ToHtml("See https://[::1]/api/v1 for details.", DefaultOptions);
		Assert.Contains("href=\"https://[::1]/api/v1\"", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_IPv6_WithPort_RenderedAsLink()
	{
		// IPv6 URL with port number.
		var result = MarkdownParser.ToHtml("API at http://[2001:db8::1]:8080/path here.", DefaultOptions);
		Assert.Contains("href=\"http://[2001:db8::1]:8080/path\"", result);
	}

	[Fact]
	public void MarkdownParser_NakedUrl_IPv6_EndOfSentence_PeriodStrippedBracketKept()
	{
		// Trailing period is stripped but the closing ] of the IPv6 host must be preserved.
		var result = MarkdownParser.ToHtml("See https://[::1].", DefaultOptions);
		Assert.Contains("href=\"https://[::1]\"", result);
		Assert.DoesNotContain("href=\"https://[::1].\"", result);
	}

	#endregion

	#region Regular Markdown Links — Unaffected by Naked-URL Feature

	[Fact]
	public void MarkdownParser_RegularLink_WithNakedUrlInSameParagraph_BothRenderedCorrectly()
	{
		// A regular Markdown link and a naked URL in the same paragraph: each must produce exactly one <a>.
		var result = MarkdownParser.ToHtml("See [docs](https://docs.example.com) and https://example.com for info.", DefaultOptions);
		Assert.Contains("href=\"https://docs.example.com\">docs</a>", result);
		Assert.Contains("href=\"https://example.com\">https://example.com</a>", result);
		var count = result.Split("<a ").Length - 1;
		Assert.Equal(2, count);
	}

	[Fact]
	public void MarkdownParser_RegularLink_LinkTextPreservedExactly()
	{
		// The link text of a regular Markdown link must not be modified by the naked-URL pass.
		var result = MarkdownParser.ToHtml("[Click here](https://example.com)", DefaultOptions);
		Assert.Contains(">Click here</a>", result);
	}

	[Fact]
	public void MarkdownParser_RegularLink_UrlInLinkText_NotAutolinked()
	{
		// A URL appearing as the display text of a [url](url) link must not be double-linked.
		var result = MarkdownParser.ToHtml("[https://example.com](https://example.com)", DefaultOptions);
		var count = result.Split("<a ").Length - 1;
		Assert.Equal(1, count);
	}

	[Fact]
	public void MarkdownParser_RegularLink_HttpsUrlInHref_NotAutolinkedAgain()
	{
		// The href value of an explicit link must not also be picked up by the naked-URL regex.
		var result = MarkdownParser.ToHtml("[link](https://example.com)", DefaultOptions);
		Assert.Contains("href=\"https://example.com\"", result);
		var count = result.Split("<a ").Length - 1;
		Assert.Equal(1, count);
	}

	[Fact]
	public void MarkdownParser_RegularLink_WithTitle_TitlePreserved()
	{
		// The title attribute of a regular Markdown link must not be lost or mangled.
		var result = MarkdownParser.ToHtml("[link](https://example.com \"My title\")", DefaultOptions);
		Assert.Contains("title=\"My title\"", result);
		Assert.Contains("href=\"https://example.com\"", result);
		var count = result.Split("<a ").Length - 1;
		Assert.Equal(1, count);
	}

	[Fact]
	public void MarkdownParser_RegularLink_UrlWithQueryString_HrefEncoded()
	{
		// A regular link whose URL contains & must still have & encoded as &amp; in href.
		var result = MarkdownParser.ToHtml("[link](https://example.com?a=1&b=2)", DefaultOptions);
		Assert.Contains("href=\"https://example.com?a=1&amp;b=2\"", result);
		var count = result.Split("<a ").Length - 1;
		Assert.Equal(1, count);
	}

	[Fact]
	public void MarkdownParser_RegularLink_MultipleExplicitLinks_EachRenderedOnce()
	{
		// Two separate explicit Markdown links — each must produce exactly one <a>, total two.
		var result = MarkdownParser.ToHtml("[first](https://first.com) and [second](https://second.com)", DefaultOptions);
		Assert.Contains("href=\"https://first.com\">first</a>", result);
		Assert.Contains("href=\"https://second.com\">second</a>", result);
		var count = result.Split("<a ").Length - 1;
		Assert.Equal(2, count);
	}

	[Fact]
	public void MarkdownParser_RegularLink_InsideStrongEmphasis_RenderedCorrectly()
	{
		// An explicit link inside bold text: link and bold must both render, URL not duplicated.
		var result = MarkdownParser.ToHtml("**[bold link](https://example.com)**", DefaultOptions);
		Assert.Contains("<strong>", result);
		Assert.Contains("href=\"https://example.com\">bold link</a>", result);
		var count = result.Split("<a ").Length - 1;
		Assert.Equal(1, count);
	}

	[Fact]
	public void MarkdownParser_RegularLink_RelativeUrl_NotAutolinked()
	{
		// A regular Markdown link with a relative URL must render normally; the relative URL
		// does not match the naked-URL regex (no scheme), so there must be exactly one <a>.
		var result = MarkdownParser.ToHtml("[page](../docs/page.html)", DefaultOptions);
		Assert.Contains("href=\"../docs/page.html\"", result);
		var count = result.Split("<a ").Length - 1;
		Assert.Equal(1, count);
	}

	#endregion
}
