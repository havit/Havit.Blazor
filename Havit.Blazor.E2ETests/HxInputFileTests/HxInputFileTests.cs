namespace Havit.Blazor.E2ETests.HxInputFileTests;

[TestClass]
public class HxInputFileTests : TestAppTestBase
{
	[TestMethod]
	public async Task HxInputFile_UploadFile_DisplaysFileName()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputFile");

		string tmpDir = Path.Combine(Path.GetTempPath(), "hx-input-file-single-test", Guid.NewGuid().ToString("N"));
		Directory.CreateDirectory(tmpDir);
		string tmpFile = Path.Combine(tmpDir, "hx-test-upload.txt");
		File.WriteAllText(tmpFile, "test content");
		string expectedFileName = Path.GetFileName(tmpFile);

		try
		{
			// Act
			var fileInput = Page.Locator("[data-testid='input-file-single-container'] input[type='file']");
			await fileInput.SetInputFilesAsync(tmpFile);

			// Assert
			await Expect(Page.Locator("[data-testid='selected-file-name']")).ToContainTextAsync(expectedFileName);
		}
		finally
		{
			try { Directory.Delete(tmpDir, recursive: true); } catch (IOException) { /* best-effort cleanup */ }
		}
	}

	[TestMethod]
	public async Task HxInputFile_UploadMultiple_AllFilesListed()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputFile");

		string tmpDir = Path.Combine(Path.GetTempPath(), "hx-input-file-multi-test", Guid.NewGuid().ToString("N"));
		Directory.CreateDirectory(tmpDir);
		string file1 = Path.Combine(tmpDir, "file1.txt");
		string file2 = Path.Combine(tmpDir, "file2.txt");
		string file3 = Path.Combine(tmpDir, "file3.txt");
		File.WriteAllText(file1, "content 1");
		File.WriteAllText(file2, "content 2");
		File.WriteAllText(file3, "content 3");

		try
		{
			// Act
			var fileInput = Page.Locator("[data-testid='input-file-multiple-container'] input[type='file']");
			await fileInput.SetInputFilesAsync(new[] { file1, file2, file3 });

			// Assert — all three file names should be listed
			await Expect(Page.Locator("[data-testid='selected-file-item']")).ToHaveCountAsync(3);
			await Expect(Page.Locator("[data-testid='selected-files-list']")).ToContainTextAsync("file1.txt");
			await Expect(Page.Locator("[data-testid='selected-files-list']")).ToContainTextAsync("file2.txt");
			await Expect(Page.Locator("[data-testid='selected-files-list']")).ToContainTextAsync("file3.txt");
		}
		finally
		{
			try { Directory.Delete(tmpDir, recursive: true); } catch (IOException) { /* best-effort cleanup */ }
		}
	}

	[TestMethod]
	public async Task HxInputFileDropZone_Hover_ShowsVisualFeedback()
	{
		// Arrange
		await NavigateToTestAppAsync("/HxInputFile");

		var dropZone = Page.Locator("[data-testid='drop-zone-container'] .hx-input-file-drop-zone");

		// Get the initial border color (before hover)
		string borderColorBefore = await dropZone.EvaluateAsync<string>("el => window.getComputedStyle(el).borderTopColor");

		// Act — hover over the drop zone to trigger the CSS :hover visual feedback
		await dropZone.HoverAsync();

		// Get the border color after hover
		string borderColorAfter = await dropZone.EvaluateAsync<string>("el => window.getComputedStyle(el).borderTopColor");

		// Assert — border color should change on hover (as per the component's CSS hover rules)
		Assert.AreNotEqual(borderColorBefore, borderColorAfter, "Border color should change on hover to indicate drop zone visual feedback.");
	}
}
