namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxSpinnerTests : BunitTestBase
{
	[Fact]
	public void HxSpinner_Render_DefaultBorderSpinner()
	{
		// Act
		var cut = Render<HxSpinner>();

		// Assert
		Assert.True(cut.Find("div").ClassList.Contains("spinner-border"));
	}

	[Fact]
	public void HxSpinner_TypeGrow_RendersGrowClass()
	{
		// Act
		var cut = Render<HxSpinner>(parameters => parameters
			.Add(p => p.Type, SpinnerType.Grow)
		);

		// Assert
		Assert.True(cut.Find("div").ClassList.Contains("spinner-grow"));
	}

	[Theory]
	[InlineData(SpinnerType.Border, "spinner-border")]
	[InlineData(SpinnerType.Grow, "spinner-grow")]
	public void HxSpinner_SizeSmall_RendersSizeClass(SpinnerType type, string typeCssClass)
	{
		// Act
		var cut = Render<HxSpinner>(parameters => parameters
			.Add(p => p.Type, type)
			.Add(p => p.Size, SpinnerSize.Small)
		);

		// Assert - Bootstrap 6 uses a single .spinner-sm class (.spinner-border-sm/.spinner-grow-sm were removed)
		var spinner = cut.Find("div");
		Assert.True(spinner.ClassList.Contains("spinner-sm"));
		Assert.False(spinner.ClassList.Contains(typeCssClass + "-sm"));
	}
}
