namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxBadgeTests : BunitTestBase
{
	[TestMethod]
	public void HxBadge_Render_DisplaysContent()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.AddChildContent("New")
		);

		// Assert
		Assert.AreEqual("New", cut.Find("span.badge").TextContent);
	}

	[TestMethod]
	public void HxBadge_Color_AppliesCorrectCssClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
		);

		// Assert
		Assert.IsNotNull(cut.Find("span.badge.text-bg-primary"));
	}

	[TestMethod]
	public void HxBadge_RoundedPill_AppliesPillClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.Type, BadgeType.RoundedPill)
		);

		// Assert
		Assert.IsNotNull(cut.Find("span.badge.rounded-pill"));
	}
}
