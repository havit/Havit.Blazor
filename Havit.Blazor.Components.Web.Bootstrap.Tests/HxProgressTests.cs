namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxProgressTests : BunitTestBase
{
	[TestMethod]
	public void HxProgress_Render_OutputsProgressContainer()
	{
		// Act
		var cut = RenderComponent<HxProgress>();

		// Assert
		var progressDiv = cut.Find("div.progress");
		Assert.IsNotNull(progressDiv);
	}

	[TestMethod]
	public void HxProgressBar_Value_SetsCorrectWidth()
	{
		// Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(b => b.Value, 50f)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		Assert.Contains("width: 50%", progressBar.GetAttribute("style"));
	}

	[TestMethod]
	public void HxProgressBar_Color_AppliesColorClass()
	{
		// Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(b => b.Color, ThemeColor.Success)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		Assert.IsTrue(progressBar.ClassList.Contains("bg-success"));
	}

	[TestMethod]
	public void HxProgressBar_Striped_AppliesStripedClass()
	{
		// Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(b => b.Striped, true)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		Assert.IsTrue(progressBar.ClassList.Contains("progress-bar-striped"));
	}

	[TestMethod]
	public void HxProgressBar_Label_DisplaysTextInBar()
	{
		// Arrange
		const string labelText = "Loading...";

		// Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(b => b.Label, labelText)
			)
		);

		// Assert
		var progressBar = cut.Find("div.progress-bar");
		Assert.Contains(labelText, progressBar.TextContent);
	}
}
