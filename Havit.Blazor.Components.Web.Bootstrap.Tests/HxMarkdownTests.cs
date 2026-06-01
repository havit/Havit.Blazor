namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxMarkdownTests : BunitTestBase
{
	#region Basic Rendering

	[Fact]
	public void HxMarkdown_NullContent_RendersNothing()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, null)
		);

		// Assert
		Assert.Equal(string.Empty, cut.Markup.Trim());
	}

	[Fact]
	public void HxMarkdown_EmptyContent_RendersNothing()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "")
		);

		// Assert
		Assert.Equal(string.Empty, cut.Markup.Trim());
	}

	[Fact]
	public void HxMarkdown_SimpleText_RendersParagraph()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "Hello world")
		);

		// Assert
		cut.Find("p"); // throws if not found
		Assert.Equal("Hello world", cut.Find("p").TextContent);
	}

	[Fact]
	public void HxMarkdown_Heading_RendersH1()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "# My Heading")
		);

		// Assert
		Assert.Equal("My Heading", cut.Find("h1").TextContent);
	}

	#endregion

	#region Wrapper div Behavior

	[Fact]
	public void HxMarkdown_NoWrapperByDefault()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "Text")
		);

		// Assert – no wrapper div, content rendered directly
		Assert.Empty(cut.FindAll("div"));
	}

	[Fact]
	public void HxMarkdown_CssClass_RendersWrapperDiv()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "Text")
			.Add(p => p.CssClass, "my-class")
		);

		// Assert
		var div = cut.Find("div.my-class");
		Assert.NotNull(div);
		Assert.Equal("Text", div.QuerySelector("p").TextContent);
	}

	[Fact]
	public void HxMarkdown_AdditionalAttributes_RendersWrapperDiv()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "Text")
			.AddUnmatched("id", "my-md")
		);

		// Assert
		var div = cut.Find("div#my-md");
		Assert.NotNull(div);
	}

	#endregion

	#region SanitizeHtml Parameter

	[Fact]
	public void HxMarkdown_SanitizeHtml_DefaultTrue_EscapesHtml()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "<b>bold</b>")
		);

		// Assert – HTML should be escaped (< encoded to &lt;, > remains)
		Assert.Empty(cut.FindAll("b"));
		Assert.True(cut.Markup.Contains("&lt;b&gt;") || cut.Markup.Contains("&lt;b>"));
	}

	[Fact]
	public void HxMarkdown_SanitizeHtml_False_PreservesHtml()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "<b>bold</b>")
			.Add(p => p.SanitizeHtml, false)
		);

		// Assert – HTML should pass through
		Assert.Equal("bold", cut.Find("b").TextContent);
	}

	#endregion

	#region Settings / Defaults

	[Fact]
	public void HxMarkdown_Settings_OverridesDefaults()
	{
		var settings = new MarkdownSettings
		{
			TableCssClass = "table table-bordered"
		};

		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "| A |\n|---|\n| 1 |")
			.Add(p => p.Settings, settings)
		);

		// Assert
		var table = cut.Find("table");
		Assert.Contains("table-bordered", table.GetAttribute("class"));
	}

	[Fact]
	public void HxMarkdown_ParameterOverridesSettings()
	{
		var settings = new MarkdownSettings
		{
			TableCssClass = "table table-bordered"
		};

		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "| A |\n|---|\n| 1 |")
			.Add(p => p.Settings, settings)
			.Add(p => p.TableCssClass, "custom-table")
		);

		// Assert
		var table = cut.Find("table");
		Assert.Equal("custom-table", table.GetAttribute("class"));
	}

	[Fact]
	public void HxMarkdown_Defaults_ApplyTableClass()
	{
		// Act – use default settings (table class = "table")
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "| A |\n|---|\n| 1 |")
		);

		// Assert
		var table = cut.Find("table");
		Assert.Equal("table", table.GetAttribute("class"));
	}

	[Fact]
	public void HxMarkdown_Defaults_ApplyBlockquoteClass()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "> Quote")
		);

		// Assert
		var bq = cut.Find("blockquote");
		Assert.Equal("blockquote", bq.GetAttribute("class"));
	}

	[Fact]
	public void HxMarkdown_Defaults_ApplyImageClass()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "![Alt](https://example.com/img.png)")
		);

		// Assert
		var img = cut.Find("img");
		Assert.Equal("img-fluid", img.GetAttribute("class"));
	}

	#endregion

	#region Complex Rendering

	[Fact]
	public void HxMarkdown_CodeBlock_RendersPreCode()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "```\nvar x = 1;\n```")
		);

		// Assert
		Assert.NotNull(cut.Find("pre code"));
	}

	[Fact]
	public void HxMarkdown_UnorderedList_RendersUl()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "- Item 1\n- Item 2")
		);

		// Assert
		Assert.Equal(2, cut.FindAll("ul li").Count());
	}

	[Fact]
	public void HxMarkdown_OrderedList_RendersOl()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "1. First\n2. Second")
		);

		// Assert
		Assert.Equal(2, cut.FindAll("ol li").Count());
	}

	[Fact]
	public void HxMarkdown_HorizontalRule_RendersHr()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "---")
		);

		// Assert
		Assert.NotNull(cut.Find("hr"));
	}

	[Fact]
	public void HxMarkdown_InlineBold_RendersStrong()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "This is **bold**")
		);

		// Assert
		Assert.Equal("bold", cut.Find("strong").TextContent);
	}

	[Fact]
	public void HxMarkdown_InlineLink_RendersAnchor()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "[Click](https://example.com)")
		);

		// Assert
		var link = cut.Find("a");
		Assert.Equal("https://example.com", link.GetAttribute("href"));
		Assert.Equal("Click", link.TextContent);
	}

	#endregion

	#region Re-render on Content Change

	[Fact]
	public void HxMarkdown_ContentChange_UpdatesRenderedOutput()
	{
		// Arrange
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "# First")
		);
		Assert.Equal("First", cut.Find("h1").TextContent);

		// Act
		cut.SetParametersAndRender(parameters => parameters
			.Add(p => p.Content, "## Second")
		);

		// Assert
		Assert.Empty(cut.FindAll("h1"));
		Assert.Equal("Second", cut.Find("h2").TextContent);
	}

	#endregion
}
