namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public partial class HxDialogTests : BunitTestBase
{
	[Fact]
	public async Task HxDialog_CloseButton_RendersBtnClose()
	{
		// Arrange & Act
		var cut = RenderComponent<HxDialog>(parameters => parameters
			.Add(p => p.Title, "Test Dialog")
		);

		// Simulate opening the dialog
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		var closeButton = cut.Find("button.btn-close");
		// The close button is wired via @onclick (single close owner); it intentionally carries no data-bs-dismiss.
		Assert.Null(closeButton.GetAttribute("data-bs-dismiss"));
		// Bootstrap 6: the icon is a CSS mask tinted with currentcolor; no btn-close-white class exists
		Assert.DoesNotContain("btn-close-white", closeButton.ClassList);
	}

	[Fact]
	public async Task HxDialog_CloseButton_ShowCloseButtonFalse_NotRendered()
	{
		// Arrange & Act
		var cut = RenderComponent<HxDialog>(parameters => parameters
			.Add(p => p.Title, "Test Dialog")
			.Add(p => p.ShowCloseButton, false)
		);

		// Simulate opening the dialog
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		Assert.Empty(cut.FindAll("button.btn-close"));
	}
}
