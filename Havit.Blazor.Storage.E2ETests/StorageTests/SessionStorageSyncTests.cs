namespace Havit.Blazor.Storage.E2ETests.StorageTests;

/// <summary>
/// E2E tests verifying that the <b>synchronous</b> <see cref="ISessionStorageService"/> methods read/write the browser
/// <c>sessionStorage</c> through the in-process JS Interop (WebAssembly render mode).
/// </summary>
public class SessionStorageSyncTests : StorageSyncTests
{
	protected override string StorageArea => "session";

	protected override string JsStorageName => "sessionStorage";
}