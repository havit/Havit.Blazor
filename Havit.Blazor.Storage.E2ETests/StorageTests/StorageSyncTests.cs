namespace Havit.Blazor.Storage.E2ETests.StorageTests;

/// <summary>
/// Base class for the browser storage E2E tests that exercise the <b>synchronous</b> storage methods.
/// The synchronous methods require <see cref="Microsoft.JSInterop.IJSInProcessRuntime"/> which is available only in the
/// WebAssembly render mode, hence the dedicated WebAssembly test page (<c>/Storage_InteractiveWebAssembly_Sync_Test</c>)
/// is used, whose buttons are wired to the synchronous storage API.
/// </summary>
public abstract class StorageSyncTests : StorageTests
{
	protected override string PageRouteTemplate => "/Storage_InteractiveWebAssembly_Sync_Test";

	protected override async Task WaitForPageReadyAsync()
	{
		// Wait until the WebAssembly component became interactive (the marker is rendered from OnAfterRender),
		// which guarantees IJSInProcessRuntime required by the synchronous storage methods is available.
		await Page.WaitForSelectorAsync("[data-testid='ready']", new() { State = WaitForSelectorState.Attached });
	}

	[TestMethod]
	public async Task SetStringValue_StoresValueInBrowserStorage()
	{
		// Arrange
		await NavigateAsync();
		await KeyInput.FillAsync("fruit");
		await ValueInput.FillAsync("apple");

		// Act — the storage service stores the value via JS Interop
		await Page.Locator("[data-testid='set-string-button']").ClickAsync();
		await Expect(Result).ToHaveTextAsync("set");

		// Assert — the value is present in the real browser storage
		var stored = await ReadItemDirectlyAsync("fruit");
		Assert.AreEqual("apple", stored);
	}

	[TestMethod]
	public async Task GetStringValue_ReadsValueFromBrowserStorage()
	{
		// Arrange — a value written directly into the browser storage
		await NavigateAsync();
		await SetItemDirectlyAsync("fruit", "banana");
		await KeyInput.FillAsync("fruit");

		// Act — the storage service reads the value via JS Interop
		await Page.Locator("[data-testid='get-string-button']").ClickAsync();

		// Assert
		await Expect(Result).ToHaveTextAsync("banana");
	}

	[TestMethod]
	public async Task GetStringValue_ThrowsWhenKeyMissing()
	{
		// Arrange
		await NavigateAsync();
		await KeyInput.FillAsync("missing");

		// Act
		await Page.Locator("[data-testid='get-string-button']").ClickAsync();

		// Assert — the service throws StorageKeyNotFoundException, surfaced by the test page as "<missing>"
		await Expect(Result).ToHaveTextAsync("<missing>");
	}

	[TestMethod]
	public async Task TryGetStringValue_ReturnsTrueAndValueWhenPresent()
	{
		// Arrange
		await NavigateAsync();
		await SetItemDirectlyAsync("key1", "value1");
		await KeyInput.FillAsync("key1");

		// Act
		await Page.Locator("[data-testid='try-get-string-button']").ClickAsync();

		// Assert
		await Expect(Result).ToHaveTextAsync("True:value1");
	}

	[TestMethod]
	public async Task TryGetStringValue_ReturnsFalseWhenKeyMissing()
	{
		// Arrange
		await NavigateAsync();
		await KeyInput.FillAsync("nope");

		// Act
		await Page.Locator("[data-testid='try-get-string-button']").ClickAsync();

		// Assert
		await Expect(Result).ToHaveTextAsync("False:<null>");
	}

	[TestMethod]
	public async Task TryGetValue_ReturnsTrueAndDeserializedValueWhenPresent()
	{
		// Arrange — a serialized object written directly into the browser storage
		await NavigateAsync();
		await SetItemDirectlyAsync("person", "{\"Name\":\"John\",\"Age\":42}");
		await KeyInput.FillAsync("person");

		// Act
		await Page.Locator("[data-testid='try-get-value-button']").ClickAsync();

		// Assert
		await Expect(Result).ToHaveTextAsync("True:John:42");
	}

	[TestMethod]
	public async Task TryGetValue_ReturnsFalseWhenKeyMissing()
	{
		// Arrange
		await NavigateAsync();
		await KeyInput.FillAsync("nope");

		// Act
		await Page.Locator("[data-testid='try-get-value-button']").ClickAsync();

		// Assert
		await Expect(Result).ToHaveTextAsync("False:<null>");
	}

	[TestMethod]
	public async Task Remove_RemovesValueFromBrowserStorage()
	{
		// Arrange
		await NavigateAsync();
		await SetItemDirectlyAsync("key1", "value1");
		await KeyInput.FillAsync("key1");

		// Act
		await Page.Locator("[data-testid='remove-button']").ClickAsync();
		await Expect(Result).ToHaveTextAsync("removed");

		// Assert — the key is gone from the real browser storage
		var stored = await ReadItemDirectlyAsync("key1");
		Assert.IsNull(stored);
	}

	[TestMethod]
	public async Task Clear_RemovesAllValuesFromBrowserStorage()
	{
		// Arrange
		await NavigateAsync();
		await SetItemDirectlyAsync("key1", "value1");
		await SetItemDirectlyAsync("key2", "value2");

		// Act
		await Page.Locator("[data-testid='clear-button']").ClickAsync();
		await Expect(Result).ToHaveTextAsync("cleared");

		// Assert — the storage area is empty
		var length = await GetLengthDirectlyAsync();
		Assert.AreEqual(0, length);
	}

	[TestMethod]
	public async Task GetLength_ReturnsActualKeyCount()
	{
		// Arrange
		await NavigateAsync();
		await SetItemDirectlyAsync("key1", "value1");
		await SetItemDirectlyAsync("key2", "value2");

		// Act
		await Page.Locator("[data-testid='length-button']").ClickAsync();

		// Assert
		await Expect(LengthResult).ToHaveTextAsync("2");
	}

	[TestMethod]
	public async Task GetKeyByIndex_ReturnsKeyAtIndex()
	{
		// Arrange — a single key so its index is deterministic
		await NavigateAsync();
		await SetItemDirectlyAsync("theOnlyKey", "value1");
		await IndexInput.FillAsync("0");

		// Act
		await Page.Locator("[data-testid='key-by-index-button']").ClickAsync();

		// Assert
		await Expect(Result).ToHaveTextAsync("theOnlyKey");
	}

	[TestMethod]
	public async Task GetKeyByIndex_ThrowsWhenIndexOutOfRange()
	{
		// Arrange — empty storage so any index is out of range
		await NavigateAsync();
		await IndexInput.FillAsync("0");

		// Act
		await Page.Locator("[data-testid='key-by-index-button']").ClickAsync();

		// Assert — the service throws StorageIndexOutOfRangeException, surfaced by the test page as "<out-of-range>"
		await Expect(Result).ToHaveTextAsync("<out-of-range>");
	}

	[TestMethod]
	public async Task TryGetKeyByIndex_ReturnsTrueAndKeyWhenInRange()
	{
		// Arrange — a single key so its index is deterministic
		await NavigateAsync();
		await SetItemDirectlyAsync("theOnlyKey", "value1");
		await IndexInput.FillAsync("0");

		// Act
		await Page.Locator("[data-testid='try-key-by-index-button']").ClickAsync();

		// Assert
		await Expect(Result).ToHaveTextAsync("True:theOnlyKey");
	}

	[TestMethod]
	public async Task TryGetKeyByIndex_ReturnsFalseWhenIndexOutOfRange()
	{
		// Arrange — empty storage so any index is out of range
		await NavigateAsync();
		await IndexInput.FillAsync("0");

		// Act
		await Page.Locator("[data-testid='try-key-by-index-button']").ClickAsync();

		// Assert
		await Expect(Result).ToHaveTextAsync("False:<null>");
	}

	[TestMethod]
	public async Task SetValueGetValue_SerializesAndDeserializesJsonViaBrowserStorage()
	{
		// Arrange
		await NavigateAsync();
		await KeyInput.FillAsync("person");
		await ValueInput.FillAsync("John");

		// Act — store a serialized object
		await Page.Locator("[data-testid='set-value-button']").ClickAsync();
		await Expect(Result).ToHaveTextAsync("set");

		// Assert — the raw JSON is present in the real browser storage
		var raw = await ReadItemDirectlyAsync("person");
		Assert.Contains("\"Name\":\"John\"", raw);
		Assert.Contains("\"Age\":42", raw);

		// Act — read the object back via the service
		await Page.Locator("[data-testid='get-value-button']").ClickAsync();

		// Assert
		await Expect(Result).ToHaveTextAsync("John:42");
	}
}
