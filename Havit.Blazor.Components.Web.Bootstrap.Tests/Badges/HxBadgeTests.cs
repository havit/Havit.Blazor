namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Badges;

[TestClass]
public class HxBadgeTests : BunitTestBase
{
	[TestMethod]
	public void HxBadge_Color_Primary_WithoutTextColor_ShouldRenderTextBgClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Badge"))
		);

		// Assert
		var badge = cut.Find("span.badge");
		Assert.IsTrue(badge.ClassList.Contains("text-bg-primary"));
	}

	[TestMethod]
	public void HxBadge_Color_Success_WithoutTextColor_ShouldRenderTextBgClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Success)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Badge"))
		);

		// Assert
		var badge = cut.Find("span.badge");
		Assert.IsTrue(badge.ClassList.Contains("text-bg-success"));
	}

	[TestMethod]
	public void HxBadge_Color_WithExplicitTextColor_ShouldRenderBgAndTextColorClasses()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.TextColor, ThemeColor.Dark)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Badge"))
		);

		// Assert
		var badge = cut.Find("span.badge");
		Assert.IsTrue(badge.ClassList.Contains("bg-primary"));
		Assert.IsTrue(badge.ClassList.Contains("text-dark"));
		Assert.IsFalse(badge.ClassList.Contains("text-bg-primary"));
	}

	[TestMethod]
	public void HxBadge_Type_Regular_ShouldNotRenderRoundedPillClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Secondary)
			.Add(p => p.Type, BadgeType.Regular)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Badge"))
		);

		// Assert
		var badge = cut.Find("span.badge");
		Assert.IsFalse(badge.ClassList.Contains("rounded-pill"));
	}

	[TestMethod]
	public void HxBadge_Type_RoundedPill_ShouldRenderRoundedPillClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Danger)
			.Add(p => p.Type, BadgeType.RoundedPill)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Badge"))
		);

		// Assert
		var badge = cut.Find("span.badge");
		Assert.IsTrue(badge.ClassList.Contains("rounded-pill"));
	}

	[TestMethod]
	public void HxBadge_ShouldRenderChildContent()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Warning)
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Test content"))
		);

		// Assert
		Assert.IsTrue(cut.Markup.Contains("Test content"));
	}

	[TestMethod]
	public void HxBadge_CssClass_ShouldBeApplied()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Info)
			.Add(p => p.CssClass, "my-custom-badge")
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Badge"))
		);

		// Assert
		var badge = cut.Find("span.badge");
		Assert.IsTrue(badge.ClassList.Contains("my-custom-badge"));
	}

	[TestMethod]
	public void HxBadge_Settings_Type_ShouldOverrideDefaults()
	{
		// Arrange & Act
		var cut = RenderComponent<HxBadge>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.Settings, new BadgeSettings { Type = BadgeType.RoundedPill })
			.Add(p => p.ChildContent, builder => builder.AddMarkupContent(0, "Badge"))
		);

		// Assert
		var badge = cut.Find("span.badge");
		Assert.IsTrue(badge.ClassList.Contains("rounded-pill"));
	}
}
