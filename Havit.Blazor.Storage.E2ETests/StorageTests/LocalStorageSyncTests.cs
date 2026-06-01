namespace Havit.Blazor.Storage.E2ETests.StorageTests;

/// <summary>
/// E2E tests verifying that the <b>synchronous</b> <see cref="ILocalStorageService"/> methods read/write the browser
/// <c>localStorage</c> through the in-process JS Interop (WebAssembly render mode).
/// </summary>
public class LocalStorageSyncTests : StorageSyncTests
{
	protected override string StorageArea => "local";

	protected override string JsStorageName => "localStorage";
}
