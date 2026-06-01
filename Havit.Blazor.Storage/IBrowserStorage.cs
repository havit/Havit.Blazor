using System.Text.Json;
using Havit.Blazor.Storage.Exceptions;

namespace Havit.Blazor.Storage;

/// <summary>
/// Provides access to a browser Web Storage area (<c>localStorage</c> or <c>sessionStorage</c>).
/// </summary>
public interface IBrowserStorage
{
	/// <summary>
	/// Tries to get the string value stored under the given <paramref name="key"/>.
	/// </summary>
	/// <param name="key">The key to look up.</param>
	/// <param name="value">The stored value when the key is present; otherwise <c>null</c>.</param>
	/// <returns><c>true</c> when the key is present; otherwise <c>false</c>.</returns>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	bool TryGetStringValue(string key, out string value);

	/// <summary>
	/// Tries to get the value stored under the given <paramref name="key"/> deserialized from JSON.
	/// </summary>
	/// <param name="key">The key to look up.</param>
	/// <param name="value">The deserialized value when the key is present; otherwise the default value of <typeparamref name="TValue"/>.</param>
	/// <param name="jsonSerializerOptions">
	/// The serializer options to use. When <c>null</c>, the options configured during the service registration are used.
	/// </param>
	/// <returns><c>true</c> when the key is present; otherwise <c>false</c>.</returns>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	bool TryGetValue<TValue>(string key, out TValue value, JsonSerializerOptions jsonSerializerOptions = null);

	/// <summary>
	/// Removes the value stored under the given <paramref name="key"/>.
	/// </summary>
	/// <param name="key">The key to remove.</param>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	void Remove(string key);

	/// <summary>
	/// Removes all keys from the storage area.
	/// </summary>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	void Clear();

	/// <summary>
	/// Returns the number of keys stored in the storage area.
	/// </summary>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	int GetLength();

	/// <summary>
	/// Returns the name of the key at the given zero-based <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the key.</param>
	/// <exception cref="StorageIndexOutOfRangeException">Thrown when the <paramref name="index"/> is out of range.</exception>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	string GetKeyByIndex(int index);

	/// <summary>
	/// Tries to get the name of the key at the given zero-based <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the key.</param>
	/// <param name="key">The name of the key when the <paramref name="index"/> is in range; otherwise <c>null</c>.</param>
	/// <returns><c>true</c> when the <paramref name="index"/> is in range; otherwise <c>false</c>.</returns>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	bool TryGetKeyByIndex(int index, out string key);

	/// <summary>
	/// Stores the given string <paramref name="value"/> under the given <paramref name="key"/>.
	/// </summary>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is <c>null</c>.</exception>
	void SetStringValue(string key, string value);

	/// <summary>
	/// Serializes <paramref name="value"/> to JSON and stores it under the given <paramref name="key"/>.
	/// </summary>
	/// <param name="key">The key to store the value under.</param>
	/// <param name="value">The value to store.</param>
	/// <param name="jsonSerializerOptions">
	/// The serializer options to use. When <c>null</c>, the options configured during the service registration are used.
	/// </param>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is <c>null</c>.</exception>
	void SetValue<TValue>(string key, TValue value, JsonSerializerOptions jsonSerializerOptions = null);

	/// <summary>
	/// Returns the string value stored under the given <paramref name="key"/>.
	/// </summary>
	/// <exception cref="StorageKeyNotFoundException">Thrown when the key is not present.</exception>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	string GetStringValue(string key);

	/// <summary>
	/// Returns the value stored under the given <paramref name="key"/> deserialized from JSON.
	/// </summary>
	/// <param name="key">The key to look up.</param>
	/// <param name="jsonSerializerOptions">
	/// The serializer options to use. When <c>null</c>, the options configured during the service registration are used.
	/// </param>
	/// <exception cref="StorageKeyNotFoundException">Thrown when the key is not present.</exception>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="InvalidOperationException">Thrown when the IJSInProcessRuntime is not available (not in Interactive WebAssembly mode).</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	TValue GetValue<TValue>(string key, JsonSerializerOptions jsonSerializerOptions = null);

	/// <summary>
	/// Tries to get the string value stored under the given <paramref name="key"/>.
	/// </summary>
	/// <param name="key">The key to look up.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// A tuple where <c>Success</c> indicates whether the key is present and <c>Value</c> contains the stored value
	/// (or <c>null</c> when the key is not present).
	/// </returns>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	ValueTask<(bool Success, string Value)> TryGetStringValueAsync(string key, CancellationToken cancellationToken = default);

	/// <summary>
	/// Tries to get the value stored under the given <paramref name="key"/> deserialized from JSON.
	/// </summary>
	/// <param name="key">The key to look up.</param>
	/// <param name="jsonSerializerOptions">
	/// The serializer options to use. When <c>null</c>, the options configured during the service registration are used.
	/// </param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// A tuple where <c>Success</c> indicates whether the key is present and <c>Value</c> contains the deserialized value
	/// (or the default value of <typeparamref name="TValue"/> when the key is not present).
	/// </returns>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	ValueTask<(bool Success, TValue Value)> TryGetValueAsync<TValue>(string key, JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Removes the value stored under the given <paramref name="key"/>.
	/// </summary>
	/// <param name="key">The key to remove.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	ValueTask RemoveAsync(string key, CancellationToken cancellationToken = default);

	/// <summary>
	/// Removes all keys from the storage area.
	/// </summary>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	ValueTask ClearAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the number of keys stored in the storage area.
	/// </summary>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	ValueTask<int> GetLengthAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the name of the key at the given zero-based <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the key.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <exception cref="StorageIndexOutOfRangeException">Thrown when the <paramref name="index"/> is out of range.</exception>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	ValueTask<string> GetKeyByIndexAsync(int index, CancellationToken cancellationToken = default);

	/// <summary>
	/// Tries to get the name of the key at the given zero-based <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The zero-based index of the key.</param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <returns>
	/// A tuple where <c>Success</c> indicates whether the <paramref name="index"/> is in range and <c>Key</c> contains the
	/// name of the key (or <c>null</c> when the <paramref name="index"/> is out of range).
	/// </returns>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	ValueTask<(bool Success, string Key)> TryGetKeyByIndexAsync(int index, CancellationToken cancellationToken = default);

	/// <summary>
	/// Stores the given string <paramref name="value"/> under the given <paramref name="key"/>.
	/// </summary>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is <c>null</c>.</exception>
	ValueTask SetStringValueAsync(string key, string value, CancellationToken cancellationToken = default);

	/// <summary>
	/// Serializes <paramref name="value"/> to JSON and stores it under the given <paramref name="key"/>.
	/// </summary>
	/// <param name="key">The key to store the value under.</param>
	/// <param name="value">The value to store.</param>
	/// <param name="jsonSerializerOptions">
	/// The serializer options to use. When <c>null</c>, the options configured during the service registration are used.
	/// </param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	/// <exception cref="ArgumentNullException">Thrown when the <paramref name="value"/> is <c>null</c>.</exception>
	ValueTask SetValueAsync<TValue>(string key, TValue value, JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the string value stored under the given <paramref name="key"/>.
	/// </summary>
	/// <exception cref="StorageKeyNotFoundException">Thrown when the key is not present.</exception>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	ValueTask<string> GetStringValueAsync(string key, CancellationToken cancellationToken = default);

	/// <summary>
	/// Returns the value stored under the given <paramref name="key"/> deserialized from JSON.
	/// </summary>
	/// <param name="key">The key to look up.</param>
	/// <param name="jsonSerializerOptions">
	/// The serializer options to use. When <c>null</c>, the options configured during the service registration are used.
	/// </param>
	/// <param name="cancellationToken">The cancellation token.</param>
	/// <exception cref="StorageKeyNotFoundException">Thrown when the key is not present.</exception>
	/// <exception cref="StorageNotAvailableException">Thrown when the underlying storage area is not accessible.</exception>
	/// <exception cref="ArgumentException">Thrown when the <paramref name="key"/> is <c>null</c> or whitespace.</exception>
	ValueTask<TValue> GetValueAsync<TValue>(string key, JsonSerializerOptions jsonSerializerOptions = null, CancellationToken cancellationToken = default);
}