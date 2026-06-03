namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxSpinnerTests : BunitTestBase
{
	[Fact]
	public void HxSpinner_Render_DefaultBorderSpinner()
	{
		// Act
		var cut = RenderComponent<HxSpinner>();

		// Assert
		Assert.True(cut.Find("div").ClassList.Contains("spinner-border"));
	}

	[Fact]
	public void HxSpinner_TypeGrow_RendersGrowClass()
	{
		// Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Type, SpinnerType.Grow)
		);

		// Assert
		Assert.True(cut.Find("div").ClassList.Contains("spinner-grow"));
	}

	[Fact]
	public void HxSpinner_SizeSmall_RendersSizeClass()
	{
		// Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Size, SpinnerSize.Small)
		);

		// Assert
		Assert.True(cut.Find("div").ClassList.Contains("spinner-border-sm"));
	}
}
