namespace Havit.Blazor.Storage.E2ETests.StorageTests;

/// <summary>
/// E2E tests verifying that <see cref="ILocalStorageService"/> reads/writes the browser <c>localStorage</c>
/// through the JS Interop.
/// </summary>
public class LocalStorageAsyncTests : StorageAsyncTests
{
	protected override string StorageArea => "local";

	protected override string JsStorageName => "localStorage";
}