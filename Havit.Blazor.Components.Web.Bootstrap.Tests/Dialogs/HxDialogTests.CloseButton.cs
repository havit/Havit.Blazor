namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public partial class HxDialogTests : BunitTestBase
{
	[Fact]
	public async Task HxDialog_CloseButton_DefaultShouldNotHaveWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxDialog>(parameters => parameters
			.Add(p => p.Title, "Test Dialog")
		);

		// Simulate opening the dialog
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.False(closeButton.ClassList.Contains("btn-close-white"));
	}

	[Fact]
	public async Task HxDialog_CloseButtonSettings_WhiteTrue_ShouldAddWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxDialog>(parameters => parameters
			.Add(p => p.Title, "Test Dialog")
			.Add(p => p.CloseButtonSettings, new CloseButtonSettings { White = true })
		);

		// Simulate opening the dialog
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.True(closeButton.ClassList.Contains("btn-close-white"));
	}

	[Fact]
	public async Task HxDialog_CloseButtonSettings_WhiteFalse_ShouldNotHaveWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxDialog>(parameters => parameters
			.Add(p => p.Title, "Test Dialog")
			.Add(p => p.CloseButtonSettings, new CloseButtonSettings { White = false })
		);

		// Simulate opening the dialog
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.False(closeButton.ClassList.Contains("btn-close-white"));
	}

	[Fact]
	public async Task HxDialog_CloseButtonSettings_WhiteTrue_ViaSettings_ShouldAddWhiteClass()
	{
		// Arrange & Act
		var cut = RenderComponent<HxDialog>(parameters => parameters
			.Add(p => p.Title, "Test Dialog")
			.Add(p => p.Settings, new DialogSettings
			{
				CloseButtonSettings = new CloseButtonSettings { White = true }
			})
		);

		// Simulate opening the dialog
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		Assert.True(closeButton.ClassList.Contains("btn-close-white"));
	}
}
