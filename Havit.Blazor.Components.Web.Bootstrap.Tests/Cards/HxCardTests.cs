namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Cards;

public class HxCardTests : BunitTestBase
{
	[Fact]
	public void HxCard_Render_DisplaysBodyContent()
	{
		// Act
		var cut = RenderComponent<HxCard>(parameters => parameters
			.Add(p => p.BodyTemplate, "Body content here")
		);

		// Assert
		var body = cut.Find(".card-body");
		Assert.NotNull(body);
		Assert.Equal("Body content here", body.TextContent);
	}

	[Fact]
	public void HxCard_HeaderAndFooter_RenderCorrectly()
	{
		// Act
		var cut = RenderComponent<HxCard>(parameters => parameters
			.Add(p => p.HeaderTemplate, "Header text")
			.Add(p => p.BodyTemplate, "Body text")
			.Add(p => p.FooterTemplate, "Footer text")
		);

		// Assert
		var header = cut.Find(".card-header");
		Assert.Equal("Header text", header.TextContent);

		var footer = cut.Find(".card-footer");
		Assert.Equal("Footer text", footer.TextContent);
	}

	[Fact]
	public void HxCard_ImageTop_RendersImageAboveBody()
	{
		// Act
		var cut = RenderComponent<HxCard>(parameters => parameters
			.Add(p => p.ImageSrc, "test-image.png")
			.Add(p => p.ImagePlacement, CardImagePlacement.Top)
			.Add(p => p.BodyTemplate, "Body text")
		);

		// Assert
		var card = cut.Find(".card");
		var img = card.QuerySelector("img.card-img-top");
		Assert.NotNull(img);
		Assert.Equal("test-image.png", img.GetAttribute("src"));

		// Image should appear before the body in the DOM
		var children = card.Children;
		var childrenArray = children.ToArray();
		var body = card.QuerySelector(".card-body");
		var imgIndex = Array.IndexOf(childrenArray, img);
		var bodyIndex = Array.IndexOf(childrenArray, body);
		Assert.True(imgIndex < bodyIndex, "Image should be rendered before the body");
	}

	[Fact]
	public void HxCardTitle_Render_OutputsH5WithText()
	{
		// Act
		var cut = RenderComponent<HxCardTitle>(parameters => parameters
			.AddChildContent("Card Title Text")
		);

		// Assert
		var title = cut.Find("h5.card-title");
		Assert.NotNull(title);
		Assert.Equal("Card Title Text", title.TextContent);
	}

	[Fact]
	public void HxCardSubtitle_Render_OutputsH6WithText()
	{
		// Act
		var cut = RenderComponent<HxCardSubtitle>(parameters => parameters
			.AddChildContent("Card Subtitle Text")
		);

		// Assert
		var subtitle = cut.Find("h6.card-subtitle");
		Assert.NotNull(subtitle);
		Assert.Equal("Card Subtitle Text", subtitle.TextContent);
	}

	[Fact]
	public void HxCardText_Render_OutputsParagraphWithText()
	{
		// Act
		var cut = RenderComponent<HxCardText>(parameters => parameters
			.AddChildContent("Card text content")
		);

		// Assert
		var text = cut.Find("p.card-text");
		Assert.NotNull(text);
		Assert.Equal("Card text content", text.TextContent);
	}
}
