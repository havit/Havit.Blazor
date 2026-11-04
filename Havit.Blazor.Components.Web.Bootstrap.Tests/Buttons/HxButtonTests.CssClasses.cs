namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public partial class HxButtonTests
{
	[Fact]
	public void HxButton_NoColor_RendersNeutralMarkerClass()
	{
		// Arrange & Act
		var cut = Render<HxButton>(parameters => parameters
			.Add(p => p.Text, "Neutral"));

		// Assert
		// Bootstrap 6 button groups target members with [class*=btn-]
		var button = cut.Find("button");
		Assert.Equal(["hx-button", "btn", "hx-btn-neutral"], button.ClassList);
	}

	[Fact]
	public void HxButton_Color_RendersVariantAndThemeWithoutNeutralMarker()
	{
		// Arrange & Act
		var cut = Render<HxButton>(parameters => parameters
			.Add(p => p.Text, "Primary")
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.Variant, ButtonVariant.Outline));

		// Assert
		var button = cut.Find("button");
		Assert.Equal(["hx-button", "btn", "btn-outline", "theme-primary"], button.ClassList);
	}

	[Theory]
	[InlineData(ThemeColor.Danger, new[] { "btn-link", "theme-danger" })]
	[InlineData(ThemeColor.Link, new[] { "btn-link" })]
	[InlineData(ThemeColor.None, new[] { "btn-link" })]
	public void HxButton_VariantLink_RendersThemeColor(ThemeColor color, string[] expectedClasses)
	{
		// Arrange & Act
		var cut = Render<HxButton>(parameters => parameters
			.Add(p => p.Text, "Link")
			.Add(p => p.Color, color)
			.Add(p => p.Variant, ButtonVariant.Link));

		// Assert
		var button = cut.Find("button");
		Assert.Equal(["hx-button", "btn", .. expectedClasses], button.ClassList);
	}
}
