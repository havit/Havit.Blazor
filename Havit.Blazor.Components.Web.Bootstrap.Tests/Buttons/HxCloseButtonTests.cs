namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxCloseButtonTests : BunitTestBase
{
	[TestMethod]
	public void HxCloseButton_Render_OutputsButtonElement()
	{
		// Arrange & Act
		var cut = RenderComponent<HxCloseButton>();

		// Assert
		var button = cut.Find("button");
		Assert.IsNotNull(button);
		Assert.IsTrue(button.ClassList.Contains("btn-close"));
	}

	[TestMethod]
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
		Assert.IsTrue(clicked);
	}
}
