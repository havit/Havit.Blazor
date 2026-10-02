namespace Havit.Blazor.E2ETests.HxInputFileTests;

public class HxInputFileTests : TestAppTestBase
{
	[Fact]
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

	[Fact]
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

	[Fact]
	public async Task HxInputFile_Issue1578_DisposedDuringUpload_AbortsRequestAndCancelsUploadAsync()
	{
		// Arrange - collect page-level JS errors and console errors (calls to a disposed DotNetObjectReference surface as unhandled rejections)
		var jsErrors = new List<string>();
		Page.PageError += (_, error) => jsErrors.Add(error);
		Page.Console += (_, message) =>
		{
			if (message.Type == "error")
			{
				jsErrors.Add(message.Text);
			}
		};

		// Hold the upload request (never respond) so the component is removed while the upload is in flight.
		var uploadRequestStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
		var uploadRequestFailed = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
		await Page.RouteAsync("**/hx-input-file-dispose-during-upload", _ =>
		{
			uploadRequestStarted.TrySetResult();
			return Task.CompletedTask;
		});
		Page.RequestFailed += (_, request) =>
		{
			if (request.Url.EndsWith("/hx-input-file-dispose-during-upload"))
			{
				uploadRequestFailed.TrySetResult(request.Failure);
			}
		};

		await NavigateToTestAppAsync("/HxInputFile_DisposeDuringUpload");

		string tmpDir = Path.Combine(Path.GetTempPath(), "hx-input-file-dispose-test", Guid.NewGuid().ToString("N"));
		Directory.CreateDirectory(tmpDir);
		string tmpFile = Path.Combine(tmpDir, "hx-test-upload.txt");
		File.WriteAllText(tmpFile, "test content");

		try
		{
			await Page.Locator("[data-testid='input-file-container'] input[type='file']").SetInputFilesAsync(tmpFile);
			await Page.Locator("[data-testid='upload-button']").ClickAsync();
			await uploadRequestStarted.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken);

			// Act - remove (dispose) the component while the upload is in flight
			await Page.Locator("[data-testid='remove-button']").ClickAsync();

			// Assert
			await Expect(Page.Locator("[data-testid='input-file-container']")).ToHaveCountAsync(0);
			await Expect(Page.Locator("[data-testid='upload-result']")).ToHaveTextAsync("Canceled"); // the awaiting UploadAsync() is released
			await uploadRequestFailed.Task.WaitAsync(TimeSpan.FromSeconds(10), TestContext.Current.CancellationToken); // the request is aborted

			// give potential late callbacks a chance to surface
			await Page.WaitForTimeoutAsync(500);
			Assert.Empty(jsErrors);
		}
		finally
		{
			try { Directory.Delete(tmpDir, recursive: true); } catch (IOException) { /* best-effort cleanup */ }
		}
	}

	[Fact]
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
		Assert.NotEqual(borderColorBefore, borderColorAfter);
	}
}
