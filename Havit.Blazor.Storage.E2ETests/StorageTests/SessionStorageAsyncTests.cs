namespace Havit.Blazor.Storage.E2ETests.StorageTests;

/// <summary>
/// E2E tests verifying that <see cref="ISessionStorageService"/> reads/writes the browser <c>sessionStorage</c>
/// through the JS Interop.
/// </summary>
[TestClass]
public class SessionStorageAsyncTests : StorageAsyncTests
{
	protected override string StorageArea => "session";

	protected override string JsStorageName => "sessionStorage";
}