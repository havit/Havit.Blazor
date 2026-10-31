namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxCloseButtonTests : BunitTestBase
{
	[Fact]
	public void HxCloseButton_Render_OutputsButtonElement()
	{
		// Arrange & Act
		var cut = Render<HxCloseButton>();

		// Assert
		var button = cut.Find("button");
		Assert.NotNull(button);
		Assert.True(button.ClassList.Contains("btn-close"));
	}

	[Fact]
	public void HxCloseButton_AdditionalAttributes_KeepsDefaultAriaLabel()
	{
		// Arrange & Act
		var cut = Render<HxCloseButton>(parameters => parameters
			.AddUnmatched("title", "Dismiss")
		);

		// Assert
		var button = cut.Find("button");
		Assert.Equal("Close", button.GetAttribute("aria-label"));
		Assert.Equal("Dismiss", button.GetAttribute("title"));
	}

	[Fact]
	public void HxCloseButton_AriaLabelAttribute_OverridesDefault()
	{
		// Arrange & Act
		var cut = Render<HxCloseButton>(parameters => parameters
			.AddUnmatched("aria-label", "Zavřít")
		);

		// Assert
		Assert.Equal("Zavřít", cut.Find("button").GetAttribute("aria-label"));
	}

	[Fact]
	public async Task HxCloseButton_Click_TriggersOnClick()
	{
		// Arrange
		var clicked = false;
		var cut = Render<HxCloseButton>(parameters => parameters
			.Add(p => p.OnClick, () => clicked = true)
		);

		// Act
		await cut.Find("button").ClickAsync(new());

		// Assert
		Assert.True(clicked);
	}
}
