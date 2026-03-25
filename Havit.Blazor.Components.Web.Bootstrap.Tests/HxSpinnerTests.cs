namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxSpinnerTests : BunitTestBase
{
	[TestMethod]
	public void HxSpinner_Render_DefaultBorderSpinner()
	{
		// Act
		var cut = RenderComponent<HxSpinner>();

		// Assert
		Assert.IsTrue(cut.Find("div").ClassList.Contains("spinner-border"));
	}

	[TestMethod]
	public void HxSpinner_TypeGrow_RendersGrowClass()
	{
		// Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Type, SpinnerType.Grow)
		);

		// Assert
		Assert.IsTrue(cut.Find("div").ClassList.Contains("spinner-grow"));
	}

	[TestMethod]
	public void HxSpinner_SizeSmall_RendersSizeClass()
	{
		// Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Size, SpinnerSize.Small)
		);

		// Assert
		Assert.IsTrue(cut.Find("div").ClassList.Contains("spinner-border-sm"));
	}
}
