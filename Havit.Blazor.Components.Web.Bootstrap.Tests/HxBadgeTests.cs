namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxBadgeTests : BunitTestBase
{
	[Fact]
	public void HxBadge_Render_DisplaysContent()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.AddChildContent("New")
		);

		// Assert
		Assert.Equal("New", cut.Find("span.badge").TextContent);
	}

	[Fact]
	public void HxBadge_Color_AppliesCorrectCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
		);

		// Assert
		Assert.NotNull(cut.Find("span.badge.theme-primary"));
	}

	[Fact]
	public void HxBadge_RoundedPill_AppliesPillClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.Type, BadgeType.RoundedPill)
		);

		// Assert
		Assert.NotNull(cut.Find("span.badge.rounded-pill"));
	}
}
