namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public class HxCloseButtonTests : BunitTestBase
{
	[Fact]
	public void HxCloseButton_Render_OutputsButtonElement()
	{
		// Arrange & Act
		var cut = RenderComponent<HxCloseButton>();

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
		var cut = RenderComponent<HxCloseButton>(parameters => parameters
			.Add(p => p.OnClick, () => clicked = true)
		);

		// Act
		await cut.Find("button").ClickAsync(new());

		// Assert
		Assert.True(clicked);
	}
}
