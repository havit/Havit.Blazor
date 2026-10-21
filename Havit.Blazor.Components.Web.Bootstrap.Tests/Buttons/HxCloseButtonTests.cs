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

	[Fact]
	public void HxCloseButton_Render_DoesNotRenderBtnClass()
	{
		// Arrange & Act
		var cut = Render<HxCloseButton>(parameters => parameters
			.Add(p => p.Color, ThemeColor.Primary)
			.Add(p => p.Size, ButtonSize.Large)
			.Add(p => p.CssClass, "custom"));

		// Assert
		// .btn hover/disabled backgrounds would paint over the masked icon
		var button = cut.Find("button");
		Assert.Equal(["btn-close", "custom"], button.ClassList);
	}

	[Fact]
	public void HxCloseButton_AriaLabel_DefaultsToLocalizedClose()
	{
		// Arrange & Act
		var cut = Render<HxCloseButton>();

		// Assert
		Assert.Equal("Close", cut.Find("button").GetAttribute("aria-label"));
	}

	[Fact]
	public void HxCloseButton_AriaLabel_UsesParameterValue()
	{
		// Arrange & Act
		var cut = Render<HxCloseButton>(parameters => parameters
			.Add(p => p.AriaLabel, "Zavřít"));

		// Assert
		Assert.Equal("Zavřít", cut.Find("button").GetAttribute("aria-label"));
	}

	[Fact]
	public void HxCloseButton_AdditionalAttributes_KeepDefaultAriaLabel()
	{
		// Arrange & Act
		var cut = Render<HxCloseButton>(parameters => parameters
			.AddUnmatched("data-test", "x"));

		// Assert
		var button = cut.Find("button");
		Assert.Equal("x", button.GetAttribute("data-test"));
		Assert.Equal("Close", button.GetAttribute("aria-label"));
	}
}
