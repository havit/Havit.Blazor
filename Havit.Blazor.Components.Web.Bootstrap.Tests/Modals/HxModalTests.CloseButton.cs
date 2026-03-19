namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public partial class HxModalTests : BunitTestBase
{
	[TestMethod]
	public void HxModal_CloseButton_DefaultShouldNotHaveWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxModal>(parameters => parameters
			.Add(p => p.Title, "Test Modal")
		);

		// Simulate opening the modal
		cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.IsFalse(closeButton.ClassList.Contains("btn-close-white"));
	}

	[TestMethod]
	public void HxModal_CloseButtonSettings_WhiteTrue_ShouldAddWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxModal>(parameters => parameters
			.Add(p => p.Title, "Test Modal")
			.Add(p => p.CloseButtonSettings, new CloseButtonSettings { White = true })
		);

		// Simulate opening the modal
		cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.IsTrue(closeButton.ClassList.Contains("btn-close-white"));
	}

	[TestMethod]
	public void HxModal_CloseButtonSettings_WhiteFalse_ShouldNotHaveWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxModal>(parameters => parameters
			.Add(p => p.Title, "Test Modal")
			.Add(p => p.CloseButtonSettings, new CloseButtonSettings { White = false })
		);

		// Simulate opening the modal
		cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.IsFalse(closeButton.ClassList.Contains("btn-close-white"));
	}

	[TestMethod]
	public void HxModal_CloseButtonSettings_WhiteTrue_ViaSettings_ShouldAddWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxModal>(parameters => parameters
			.Add(p => p.Title, "Test Modal")
			.Add(p => p.Settings, new ModalSettings
			{
				CloseButtonSettings = new CloseButtonSettings { White = true }
			})
		);

		// Simulate opening the modal
		cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.IsTrue(closeButton.ClassList.Contains("btn-close-white"));
	}
}
