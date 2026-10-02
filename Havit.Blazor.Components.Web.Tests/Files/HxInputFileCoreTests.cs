using Bunit;
using Havit.Blazor.Components.Web.Bootstrap.Tests;

namespace Havit.Blazor.Components.Web.Tests.Files;

public class HxInputFileCoreTests : BunitTestBase
{
	[Fact]
	public async Task HxInputFileCore_UploadAsync_ShouldReturnResultWhenUploadCompleted()
	{
		// Arrange
		var cut = Render<HxInputFileCore>(parameters => parameters.Add(p => p.UploadUrl, "/upload"));

		// Act
		var uploadTask = cut.Instance.UploadAsync();
		await cut.Instance.HandleUploadCompleted(fileCount: 0, totalSize: 0);

		// Assert
		var result = await uploadTask.WaitAsync(TimeSpan.FromSeconds(5), Xunit.TestContext.Current.CancellationToken);
		Assert.Equal(0, result.FileCount);
	}

	[Fact]
	public async Task HxInputFileCore_UploadAsync_ShouldBeCanceledWhenComponentDisposedDuringUpload()
	{
		// Arrange
		var cut = Render<HxInputFileCore>(parameters => parameters.Add(p => p.UploadUrl, "/upload"));
		var uploadTask = cut.Instance.UploadAsync();
		Assert.False(uploadTask.IsCompleted);

		// Act
		await cut.Instance.DisposeAsync();

		// Assert
		await Assert.ThrowsAnyAsync<OperationCanceledException>(() => uploadTask.WaitAsync(TimeSpan.FromSeconds(5), Xunit.TestContext.Current.CancellationToken));
	}

	[Fact]
	public async Task HxInputFileCore_UploadAsync_ShouldThrowWhenAnotherUploadIsInProgress()
	{
		// Arrange
		var cut = Render<HxInputFileCore>(parameters => parameters.Add(p => p.UploadUrl, "/upload"));
		var firstUploadTask = cut.Instance.UploadAsync();

		// Act + Assert
		await Assert.ThrowsAsync<InvalidOperationException>(() => cut.Instance.UploadAsync());

		// the first upload is not affected
		await cut.Instance.HandleUploadCompleted(fileCount: 0, totalSize: 0);
		await firstUploadTask.WaitAsync(TimeSpan.FromSeconds(5), Xunit.TestContext.Current.CancellationToken);
	}

	[Fact]
	public async Task HxInputFileCore_UploadAsync_ShouldAllowNextUploadAfterPreviousCompleted()
	{
		// Arrange
		var cut = Render<HxInputFileCore>(parameters => parameters.Add(p => p.UploadUrl, "/upload"));
		var firstUploadTask = cut.Instance.UploadAsync();
		await cut.Instance.HandleUploadCompleted(fileCount: 0, totalSize: 0);
		await firstUploadTask.WaitAsync(TimeSpan.FromSeconds(5), Xunit.TestContext.Current.CancellationToken);

		// Act
		var secondUploadTask = cut.Instance.UploadAsync();
		await cut.Instance.HandleUploadCompleted(fileCount: 0, totalSize: 0);

		// Assert
		await secondUploadTask.WaitAsync(TimeSpan.FromSeconds(5), Xunit.TestContext.Current.CancellationToken);
	}

	[Fact]
	public async Task HxInputFileCore_UploadAsync_ShouldBeCanceledWhenComponentAlreadyDisposed()
	{
		// Arrange
		var cut = Render<HxInputFileCore>(parameters => parameters.Add(p => p.UploadUrl, "/upload"));
		await cut.Instance.DisposeAsync();

		// Act + Assert
		await Assert.ThrowsAnyAsync<OperationCanceledException>(() => cut.Instance.UploadAsync().WaitAsync(TimeSpan.FromSeconds(5), Xunit.TestContext.Current.CancellationToken));

		// no JS module is imported after dispose
		Assert.DoesNotContain(JSInterop.Invocations, invocation => invocation.Identifier == "import");
	}
}
