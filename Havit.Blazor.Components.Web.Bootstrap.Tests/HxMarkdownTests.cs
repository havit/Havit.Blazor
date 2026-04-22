namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxMarkdownTests : BunitTestBase
{
	#region Basic Rendering

	[TestMethod]
	public void HxMarkdown_NullContent_RendersNothing()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, null)
		);

		// Assert
		Assert.AreEqual(string.Empty, cut.Markup.Trim());
	}

	[TestMethod]
	public void HxMarkdown_EmptyContent_RendersNothing()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "")
		);

		// Assert
		Assert.AreEqual(string.Empty, cut.Markup.Trim());
	}

	[TestMethod]
	public void HxMarkdown_SimpleText_RendersParagraph()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "Hello world")
		);

		// Assert
		cut.Find("p"); // throws if not found
		Assert.AreEqual("Hello world", cut.Find("p").TextContent);
	}

	[TestMethod]
	public void HxMarkdown_Heading_RendersH1()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "# My Heading")
		);

		// Assert
		Assert.AreEqual("My Heading", cut.Find("h1").TextContent);
	}

	#endregion

	#region Wrapper div Behavior

	[TestMethod]
	public void HxMarkdown_NoWrapperByDefault()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "Text")
		);

		// Assert – no wrapper div, content rendered directly
		Assert.IsEmpty(cut.FindAll("div"));
	}

	[TestMethod]
	public void HxMarkdown_CssClass_RendersWrapperDiv()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "Text")
			.Add(p => p.CssClass, "my-class")
		);

		// Assert
		var div = cut.Find("div.my-class");
		Assert.IsNotNull(div);
		Assert.AreEqual("Text", div.QuerySelector("p").TextContent);
	}

	[TestMethod]
	public void HxMarkdown_AdditionalAttributes_RendersWrapperDiv()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "Text")
			.AddUnmatched("id", "my-md")
		);

		// Assert
		var div = cut.Find("div#my-md");
		Assert.IsNotNull(div);
	}

	#endregion

	#region SanitizeHtml Parameter

	[TestMethod]
	public void HxMarkdown_SanitizeHtml_DefaultTrue_EscapesHtml()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "<b>bold</b>")
		);

		// Assert – HTML should be escaped (< encoded to &lt;, > remains)
		Assert.IsEmpty(cut.FindAll("b"));
		Assert.IsTrue(cut.Markup.Contains("&lt;b&gt;") || cut.Markup.Contains("&lt;b>"));
	}

	[TestMethod]
	public void HxMarkdown_SanitizeHtml_False_PreservesHtml()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "<b>bold</b>")
			.Add(p => p.SanitizeHtml, false)
		);

		// Assert – HTML should pass through
		Assert.AreEqual("bold", cut.Find("b").TextContent);
	}

	#endregion

	#region Settings / Defaults

	[TestMethod]
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

	[TestMethod]
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
		Assert.AreEqual("custom-table", table.GetAttribute("class"));
	}

	[TestMethod]
	public void HxMarkdown_Defaults_ApplyTableClass()
	{
		// Act – use default settings (table class = "table")
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "| A |\n|---|\n| 1 |")
		);

		// Assert
		var table = cut.Find("table");
		Assert.AreEqual("table", table.GetAttribute("class"));
	}

	[TestMethod]
	public void HxMarkdown_Defaults_ApplyBlockquoteClass()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "> Quote")
		);

		// Assert
		var bq = cut.Find("blockquote");
		Assert.AreEqual("blockquote", bq.GetAttribute("class"));
	}

	[TestMethod]
	public void HxMarkdown_Defaults_ApplyImageClass()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "![Alt](https://example.com/img.png)")
		);

		// Assert
		var img = cut.Find("img");
		Assert.AreEqual("img-fluid", img.GetAttribute("class"));
	}

	#endregion

	#region Complex Rendering

	[TestMethod]
	public void HxMarkdown_CodeBlock_RendersPreCode()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "```\nvar x = 1;\n```")
		);

		// Assert
		Assert.IsNotNull(cut.Find("pre code"));
	}

	[TestMethod]
	public void HxMarkdown_UnorderedList_RendersUl()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "- Item 1\n- Item 2")
		);

		// Assert
		Assert.HasCount(2, cut.FindAll("ul li"));
	}

	[TestMethod]
	public void HxMarkdown_OrderedList_RendersOl()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "1. First\n2. Second")
		);

		// Assert
		Assert.HasCount(2, cut.FindAll("ol li"));
	}

	[TestMethod]
	public void HxMarkdown_HorizontalRule_RendersHr()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "---")
		);

		// Assert
		Assert.IsNotNull(cut.Find("hr"));
	}

	[TestMethod]
	public void HxMarkdown_InlineBold_RendersStrong()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "This is **bold**")
		);

		// Assert
		Assert.AreEqual("bold", cut.Find("strong").TextContent);
	}

	[TestMethod]
	public void HxMarkdown_InlineLink_RendersAnchor()
	{
		// Act
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "[Click](https://example.com)")
		);

		// Assert
		var link = cut.Find("a");
		Assert.AreEqual("https://example.com", link.GetAttribute("href"));
		Assert.AreEqual("Click", link.TextContent);
	}

	#endregion

	#region Re-render on Content Change

	[TestMethod]
	public void HxMarkdown_ContentChange_UpdatesRenderedOutput()
	{
		// Arrange
		var cut = RenderComponent<HxMarkdown>(parameters => parameters
			.Add(p => p.Content, "# First")
		);
		Assert.AreEqual("First", cut.Find("h1").TextContent);

		// Act
		cut.SetParametersAndRender(parameters => parameters
			.Add(p => p.Content, "## Second")
		);

		// Assert
		Assert.IsEmpty(cut.FindAll("h1"));
		Assert.AreEqual("Second", cut.Find("h2").TextContent);
	}

	#endregion
}
