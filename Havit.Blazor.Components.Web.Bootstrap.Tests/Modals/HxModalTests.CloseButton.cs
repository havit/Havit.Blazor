namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public partial class HxModalTests : BunitTestBase
{
	[Fact]
	public async Task HxModal_CloseButton_DefaultShouldNotHaveWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxModal>(parameters => parameters
			.Add(p => p.Title, "Test Modal")
		);

		// Simulate opening the modal
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.False(closeButton.ClassList.Contains("btn-close-white"));
	}

	[Fact]
	public async Task HxModal_CloseButtonSettings_WhiteTrue_ShouldAddWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxModal>(parameters => parameters
			.Add(p => p.Title, "Test Modal")
			.Add(p => p.CloseButtonSettings, new CloseButtonSettings { White = true })
		);

		// Simulate opening the modal
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.True(closeButton.ClassList.Contains("btn-close-white"));
	}

	[Fact]
	public async Task HxModal_CloseButtonSettings_WhiteFalse_ShouldNotHaveWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxModal>(parameters => parameters
			.Add(p => p.Title, "Test Modal")
			.Add(p => p.CloseButtonSettings, new CloseButtonSettings { White = false })
		);

		// Simulate opening the modal
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.False(closeButton.ClassList.Contains("btn-close-white"));
	}

	[Fact]
	public async Task HxModal_CloseButtonSettings_WhiteTrue_ViaSettings_ShouldAddWhiteClass()
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
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.True(closeButton.ClassList.Contains("btn-close-white"));
	}
}
