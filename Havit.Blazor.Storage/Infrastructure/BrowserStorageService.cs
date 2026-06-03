using System.Text.Json;

namespace Havit.Blazor.Storage.Infrastructure;

/// <summary>
/// Base class for the browser storage services. It delegates all calls to the shared <see cref="BrowserStorageAccessor"/>
/// passing the <see cref="BrowserStorageType"/> of the concrete service.
/// </summary>
internal abstract class BrowserStorageService(BrowserStorageAccessor browserStorageAccessor) : IBrowserStorage
{
	protected abstract BrowserStorageType StorageType { get; }

	/// <inheritdoc/>
	public bool TryGetStringValue(string key, out string value) => browserStorageAccessor.TryGetStringValue(StorageType, key, out value);

	/// <inheritdoc/>
	public bool TryGetValue<TValue>(string key, out TValue value, JsonSerializerOptions jsonSerializerOptions = null) => browserStorageAccessor.TryGetValue(StorageType, key, out value, jsonSerializerOptions);

	/// <inheritdoc/>
	public void Remove(string key) => browserStorageAccessor.Remove(StorageType, key);

	/// <inheritdoc/>
	public void Clear() => browserStorageAccessor.Clear(StorageType);

	/// <inheritdoc/>
	public int GetLength() => browserStorageAccessor.GetLength(StorageType);

	/// <inheritdoc/>
	public string GetKeyByIndex(int index) => browserStorageAccessor.GetKeyByIndex(StorageType, index);

	/// <inheritdoc/>
	public bool TryGetKeyByIndex(int index, out string key) => browserStorageAccessor.TryGetKeyByIndex(StorageType, index, out key);

	/// <inheritdoc/>
	public void SetStringValue(string key, string value) => browserStorageAccessor.SetStringValue(StorageType, key, value);

	/// <inheritdoc/>
	public void SetValue<TValue>(string key, TValue value, JsonSerializerOptions jsonSerializerOptions = null) => browserStorageAccessor.SetValue(StorageType, key, value, jsonSerializerOptions);

	/// <inheritdoc/>
	public string GetStringValue(string key) => browserStorageAccessor.GetStringValue(StorageType, key);

	/// <inheritdoc/>
	public TValue GetValue<TValue>(string key, JsonSerializerOptions jsonSerializerOptions = null) => browserStorageAccessor.GetValue<TValue>(StorageType, key, jsonSerializerOptions);

	/// <inheritdoc/>
	public ValueTask<(bool Success, string Value)> TryGetStringValueAsync(string key, CancellationToken cancellationToken = default) => browserStorageAccessor.TryGetStringValueAsync(StorageType, key, cancellationToken);

	/// <inheritdoc/>
	public ValueTask<(bool Success, TValue Value)> TryGetValueAsync<TValue>(string key, JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default) => browserStorageAccessor.TryGetValueAsync<TValue>(StorageType, key, jsonSerializerOptions, cancellationToken);

	/// <inheritdoc/>
	public ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default) => browserStorageAccessor.RemoveAsync(StorageType, key, cancellationToken);

	/// <inheritdoc/>
	public ValueTask ClearAsync(CancellationToken cancellationToken = default) => browserStorageAccessor.ClearAsync(StorageType, cancellationToken);

	/// <inheritdoc/>
	public ValueTask<int> GetLengthAsync(CancellationToken cancellationToken = default) => browserStorageAccessor.GetLengthAsync(StorageType, cancellationToken);

	/// <inheritdoc/>
	public ValueTask<string> GetKeyByIndexAsync(int index, CancellationToken cancellationToken = default) => browserStorageAccessor.GetKeyByIndexAsync(StorageType, index, cancellationToken);

	/// <inheritdoc/>
	public ValueTask<(bool Success, string Key)> TryGetKeyByIndexAsync(int index, CancellationToken cancellationToken = default) => browserStorageAccessor.TryGetKeyByIndexAsync(StorageType, index, cancellationToken);

	/// <inheritdoc/>
	public ValueTask SetStringValueAsync(string key, string value, CancellationToken cancellationToken = default) => browserStorageAccessor.SetStringValueAsync(StorageType, key, value, cancellationToken);

	/// <inheritdoc/>
	public ValueTask SetValueAsync<TValue>(string key, TValue value, JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default) => browserStorageAccessor.SetValueAsync(StorageType, key, value, jsonSerializerOptions, cancellationToken);

	/// <inheritdoc/>
	public ValueTask<string> GetStringValueAsync(string key, CancellationToken cancellationToken = default) => browserStorageAccessor.GetStringValueAsync(StorageType, key, cancellationToken);

	/// <inheritdoc/>
	public ValueTask<TValue> GetValueAsync<TValue>(string key, JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default) => browserStorageAccessor.GetValueAsync<TValue>(StorageType, key, jsonSerializerOptions, cancellationToken);
}