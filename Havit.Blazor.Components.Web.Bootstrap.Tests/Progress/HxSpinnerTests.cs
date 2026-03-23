namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Progress;

[TestClass]
public class HxSpinnerTests : BunitTestBase
{
	[TestMethod]
	public void HxSpinner_DefaultType_ShouldRenderBorderClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxSpinner>();

		// Assert
		var spinner = cut.Find("div[role='status']");
		Assert.IsTrue(spinner.ClassList.Contains("spinner-border"));
		Assert.IsFalse(spinner.ClassList.Contains("spinner-grow"));
	}

	[TestMethod]
	public void HxSpinner_Type_Border_ShouldRenderBorderClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Type, SpinnerType.Border)
		);

		// Assert
		var spinner = cut.Find("div[role='status']");
		Assert.IsTrue(spinner.ClassList.Contains("spinner-border"));
	}

	[TestMethod]
	public void HxSpinner_Type_Grow_ShouldRenderGrowClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Type, SpinnerType.Grow)
		);

		// Assert
		var spinner = cut.Find("div[role='status']");
		Assert.IsTrue(spinner.ClassList.Contains("spinner-grow"));
		Assert.IsFalse(spinner.ClassList.Contains("spinner-border"));
	}

	[TestMethod]
	public void HxSpinner_Size_Regular_ShouldNotRenderSmClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Size, SpinnerSize.Regular)
		);

		// Assert
		var spinner = cut.Find("div[role='status']");
		Assert.IsFalse(spinner.ClassList.Contains("spinner-border-sm"));
		Assert.IsFalse(spinner.ClassList.Contains("spinner-grow-sm"));
	}

	[TestMethod]
	public void HxSpinner_Size_Small_Border_ShouldRenderSmClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Type, SpinnerType.Border)
			.Add(p => p.Size, SpinnerSize.Small)
		);

		// Assert
		var spinner = cut.Find("div[role='status']");
		Assert.IsTrue(spinner.ClassList.Contains("spinner-border-sm"));
	}

	[TestMethod]
	public void HxSpinner_Size_Small_Grow_ShouldRenderSmClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Type, SpinnerType.Grow)
			.Add(p => p.Size, SpinnerSize.Small)
		);

		// Assert
		var spinner = cut.Find("div[role='status']");
		Assert.IsTrue(spinner.ClassList.Contains("spinner-grow-sm"));
	}

	[TestMethod]
	public void HxSpinner_Color_Primary_ShouldRenderTextColorClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
		);

		// Assert
		var spinner = cut.Find("div[role='status']");
		Assert.IsTrue(spinner.ClassList.Contains("text-primary"));
	}

	[TestMethod]
	public void HxSpinner_Color_None_ShouldNotRenderTextColorClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.Color, ThemeColor.None)
		);

		// Assert
		var spinner = cut.Find("div[role='status']");
		Assert.IsFalse(spinner.ClassList.Any(c => c.StartsWith("text-")));
	}

	[TestMethod]
	public void HxSpinner_ShouldRenderHiddenTextForAccessibility()
	{
		// Arrange & Act
		var cut = RenderComponent<HxSpinner>();

		// Assert
		var hiddenText = cut.Find("span.visually-hidden");
		Assert.IsNotNull(hiddenText);
	}

	[TestMethod]
	public void HxSpinner_CssClass_ShouldBeApplied()
	{
		// Arrange & Act
		var cut = RenderComponent<HxSpinner>(parameters => parameters
			.Add(p => p.CssClass, "my-custom-spinner")
		);

		// Assert
		var spinner = cut.Find("div[role='status']");
		Assert.IsTrue(spinner.ClassList.Contains("my-custom-spinner"));
	}
}
