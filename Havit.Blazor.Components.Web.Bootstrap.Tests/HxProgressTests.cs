namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxProgressTests : BunitTestBase
{
	[Fact]
	public void HxProgress_Render_OutputsProgressStackedContainer()
	{
		// Act
		var cut = RenderComponent<HxProgress>();

		// Assert
		var progressDiv = cut.Find("div.progress-stacked");
		Assert.NotNull(progressDiv);
	}

	[Fact]
	public void HxProgressBar_Value_SetsCorrectWidthAndAriaAttributes()
	{
		// Act
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(b => b.Value, 50f)
			)
		);

		// Assert — Bootstrap 6 structure: .progress wrapper carries role/aria and the width, .progress-bar is purely visual
		var progress = cut.Find("div.progress");
		Assert.Contains("width: 50%", progress.GetAttribute("style"));
		Assert.Equal("progressbar", progress.GetAttribute("role"));
		Assert.Equal("50", progress.GetAttribute("aria-valuenow"));
		Assert.Equal("0", progress.GetAttribute("aria-valuemin"));
		Assert.Equal("100", progress.GetAttribute("aria-valuemax"));
		var progressBar = cut.Find("div.progress > div.progress-bar");
		Assert.Null(progressBar.GetAttribute("role"));
	}

	[Fact]
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
		Assert.True(progressBar.ClassList.Contains("bg-success"));
	}

	[Fact]
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
		Assert.True(progressBar.ClassList.Contains("progress-bar-striped"));
	}

	[Fact]
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

	[Fact]
	public void HxProgressBar_CustomMinMaxRange_CalculatesCorrectWidth()
	{
		// Act — regression for #813: HxProgressBar must work with custom MinValue/MaxValue range
		var cut = RenderComponent<HxProgress>(parameters => parameters
			.AddChildContent<HxProgressBar>(bar => bar
				.Add(b => b.Value, 25f)
				.Add(b => b.MinValue, 0f)
				.Add(b => b.MaxValue, 50f)
			)
		);

		// Assert — Value 25 in range [0, 50] should be 50% width (set on the .progress wrapper since Bootstrap 6)
		var progress = cut.Find("div.progress");
		Assert.Contains("width: 50%", progress.GetAttribute("style"));
	}
}
