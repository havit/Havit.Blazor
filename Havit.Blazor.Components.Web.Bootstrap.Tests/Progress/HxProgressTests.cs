namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Progress;

[TestClass]
public class HxProgressTests : BunitTestBase
{
	[TestMethod]
	public void HxProgressBar_Value_ShouldRenderWidthStyle()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(p => p.Value, 50f)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		var style = progressBar.GetAttribute("style");
		Assert.IsTrue(style.Contains("width: 50%"));
	}

	[TestMethod]
	public void HxProgressBar_Value_Zero_ShouldRenderZeroWidthStyle()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(p => p.Value, 0f)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		var style = progressBar.GetAttribute("style");
		Assert.IsTrue(style.Contains("width: 0%"));
	}

	[TestMethod]
	public void HxProgressBar_Value_100_ShouldRenderFullWidthStyle()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(p => p.Value, 100f)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		var style = progressBar.GetAttribute("style");
		Assert.IsTrue(style.Contains("width: 100%"));
	}

	[TestMethod]
	public void HxProgressBar_Color_Primary_ShouldRenderBackgroundColorClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(p => p.Value, 50f)
				.Add(p => p.Color, ThemeColor.Primary)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		Assert.IsTrue(progressBar.ClassList.Contains("bg-primary"));
	}

	[TestMethod]
	public void HxProgressBar_Striped_ShouldRenderStripedClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(p => p.Value, 75f)
				.Add(p => p.Striped, true)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		Assert.IsTrue(progressBar.ClassList.Contains("progress-bar-striped"));
	}

	[TestMethod]
	public void HxProgressBar_Animated_ShouldRenderBothStripedAndAnimatedClasses()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(p => p.Value, 60f)
				.Add(p => p.Animated, true)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		Assert.IsTrue(progressBar.ClassList.Contains("progress-bar-striped"));
		Assert.IsTrue(progressBar.ClassList.Contains("progress-bar-animated"));
	}

	[TestMethod]
	public void HxProgressBar_WithoutStripedOrAnimated_ShouldNotRenderStripedClasses()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(p => p.Value, 40f)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		Assert.IsFalse(progressBar.ClassList.Contains("progress-bar-striped"));
		Assert.IsFalse(progressBar.ClassList.Contains("progress-bar-animated"));
	}

	[TestMethod]
	public void HxProgressBar_Label_ShouldBeRendered()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(p => p.Value, 30f)
				.Add(p => p.Label, "30%")
			)
		);

		// Assert
		Assert.IsTrue(cut.Markup.Contains("30%"));
	}

	[TestMethod]
	public void HxProgress_Height_ShouldRenderHeightStyle()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.Add(p => p.Height, 20)
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(p => p.Value, 50f)
			)
		);

		// Assert
		var progress = cut.Find("div.progress");
		var style = progress.GetAttribute("style");
		Assert.IsTrue(style.Contains("height: 20px"));
	}

	[TestMethod]
	public void HxProgressBar_AriaAttributes_ShouldBeRendered()
	{
		// Arrange & Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(p => p.Value, 75f)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		Assert.AreEqual("75", progressBar.GetAttribute("aria-valuenow"));
		Assert.AreEqual("0", progressBar.GetAttribute("aria-valuemin"));
		Assert.AreEqual("100", progressBar.GetAttribute("aria-valuemax"));
	}
}
