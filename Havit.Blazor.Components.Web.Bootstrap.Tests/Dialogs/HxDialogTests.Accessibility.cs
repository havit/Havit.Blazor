namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

public partial class HxDialogTests
{
	[Fact]
	public void HxDialog_Title_LabelsTheDialog()
	{
		// Act
		var cut = Render<HxDialog>(parameters => parameters
			.Add(p => p.RenderMode, DialogRenderMode.Always)
			.Add(p => p.Title, "Test Dialog"));

		// Assert
		var dialog = cut.Find("dialog.dialog");
		var title = cut.Find("h1.dialog-title");
		Assert.False(String.IsNullOrEmpty(title.Id));
		Assert.Equal(title.Id, dialog.GetAttribute("aria-labelledby"));
	}

	[Fact]
	public void HxDialog_NoTitle_NoAriaLabelledBy()
	{
		// Act
		var cut = Render<HxDialog>(parameters => parameters
			.Add(p => p.RenderMode, DialogRenderMode.Always));

		// Assert
		Assert.Null(cut.Find("dialog.dialog").GetAttribute("aria-labelledby"));
	}

	[Fact]
	public void HxDialog_AdditionalAttributes_SplattedOnDialog()
	{
		// Act
		var cut = Render<HxDialog>(parameters => parameters
			.AddUnmatched("data-bs-theme", "dark"));

		// Assert
		Assert.Equal("dark", cut.Find("dialog.dialog").GetAttribute("data-bs-theme"));
	}

	[Fact]
	public void HxDialog_BackdropFalse_RendersDataAttribute()
	{
		// Act
		var cut = Render<HxDialog>(parameters => parameters
			.Add(p => p.Backdrop, DialogBackdrop.False));

		// Assert - the attribute is used both by Bootstrap and by the HxDialog CSS hiding the ::backdrop
		Assert.Equal("false", cut.Find("dialog.dialog").GetAttribute("data-bs-backdrop"));
	}

	[Fact]
	public async Task HxDialog_CloseButton_HasNoLegacyColorReset()
	{
		// Arrange
		var cut = Render<HxDialog>(parameters => parameters
			.Add(p => p.Title, "Test Dialog"));

		// Act
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert - .btn-close inherits the color in Bootstrap 6
		var closeButton = cut.Find("button.btn-close");
		Assert.DoesNotContain("fg-reset", closeButton.ClassList);
		Assert.DoesNotContain("text-reset", closeButton.ClassList);
	}

	[Fact]
	public async Task HxMessageBox_AdditionalAttributes_PassedToDialog()
	{
		// Arrange - HxMessageBox splats its AdditionalAttributes onto HxDialog
		var cut = Render<HxMessageBox>(parameters => parameters
			.Add(p => p.Title, "Confirmation")
			.Add(p => p.AdditionalAttributes, new Dictionary<string, object> { ["data-bs-theme"] = "dark" }));

		// Act
		await cut.InvokeAsync(() => cut.Instance.ShowAsync());

		// Assert
		Assert.Equal("dark", cut.Find("dialog.dialog").GetAttribute("data-bs-theme"));
	}
}
