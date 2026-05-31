using System.Text.Json;
using Havit.Blazor.Storage.Exceptions;
using Havit.Diagnostics.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Havit.Blazor.Storage.Infrastructure;

/// <summary>
/// Shared implementation of the browser storage access. It contains the actual JS Interop calls and accepts a
/// <see cref="BrowserStorageType"/> parameter that determines which storage area (<c>localStorage</c> or
/// <c>sessionStorage</c>) is accessed. The public <see cref="ILocalStorageService"/> and <see cref="ISessionStorageService"/>
/// implementations only delegate to this class passing the correct <see cref="BrowserStorageType"/>.
/// </summary>
internal sealed class BrowserStorageAccessor(IJSRuntime jsRuntime, IOptions<StorageServiceOptions> options)
{
	private readonly StorageServiceOptions _options = options.Value;

	public bool TryGetStringValue(BrowserStorageType storageType, string key, out string value)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));

		value = GetByKeyCore(storageType, key);
		return value is not null;
	}

	public bool TryGetValue<TValue>(BrowserStorageType storageType, string key, out TValue value, JsonSerializerOptions jsonSerializerOptions)
	{
		if (TryGetStringValue(storageType, key, out var json))
		{
			value = JsonSerializer.Deserialize<TValue>(json, ResolveJsonSerializerOptions(jsonSerializerOptions));
			return true;
		}

		value = default;
		return false;
	}

	public void Remove(BrowserStorageType storageType, string key)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));

		var jsInProcessRuntime = GetInProcessRuntime();
		try
		{
			jsInProcessRuntime.InvokeVoid(GetStorageName(storageType) + ".removeItem", key);
		}
		catch (JSException exception) when (IsStorageNotAvailable(storageType, exception))
		{
			throw CreateNotAvailableException(storageType, exception);
		}
	}

	public void Clear(BrowserStorageType storageType)
	{
		var jsInProcessRuntime = GetInProcessRuntime();
		try
		{
			jsInProcessRuntime.InvokeVoid(GetStorageName(storageType) + ".clear");
		}
		catch (JSException exception) when (IsStorageNotAvailable(storageType, exception))
		{
			throw CreateNotAvailableException(storageType, exception);
		}
	}

	public int GetLength(BrowserStorageType storageType)
	{
		var jsInProcessRuntime = GetInProcessRuntime();
		try
		{
			return jsInProcessRuntime.Invoke<int>("HavitBlazorStorage.getLength", GetStorageName(storageType));
		}
		catch (JSException exception) when (IsStorageNotAvailable(storageType, exception))
		{
			throw CreateNotAvailableException(storageType, exception);
		}
	}

	public string GetKeyByIndex(BrowserStorageType storageType, int index)
	{
		var key = GetKeyByIndexCore(storageType, index);
		return key ?? throw CreateIndexOutOfRangeException(storageType, index);
	}

	public bool TryGetKeyByIndex(BrowserStorageType storageType, int index, out string key)
	{
		key = GetKeyByIndexCore(storageType, index);
		return key is not null;
	}

	public void SetStringValue(BrowserStorageType storageType, string key, string value)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));
		Contract.Requires<ArgumentNullException>(value is not null);

		var jsInProcessRuntime = GetInProcessRuntime();
		try
		{
			jsInProcessRuntime.InvokeVoid(GetStorageName(storageType) + ".setItem", key, value);
		}
		catch (JSException exception) when (IsStorageNotAvailable(storageType, exception))
		{
			throw CreateNotAvailableException(storageType, exception);
		}
	}

	public void SetValue<TValue>(BrowserStorageType storageType, string key, TValue value, JsonSerializerOptions jsonSerializerOptions)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));
		Contract.Requires<ArgumentNullException>(value is not null);

		SetStringValue(storageType, key, JsonSerializer.Serialize(value, ResolveJsonSerializerOptions(jsonSerializerOptions)));
	}

	public string GetStringValue(BrowserStorageType storageType, string key)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));

		var value = GetByKeyCore(storageType, key);
		return value ?? throw CreateKeyNotFoundException(storageType, key);
	}

	public TValue GetValue<TValue>(BrowserStorageType storageType, string key, JsonSerializerOptions jsonSerializerOptions)
	{
		var json = GetStringValue(storageType, key);
		return JsonSerializer.Deserialize<TValue>(json, ResolveJsonSerializerOptions(jsonSerializerOptions));
	}

	public async ValueTask<(bool Success, string Value)> TryGetStringValueAsync(BrowserStorageType storageType, string key, CancellationToken cancellationToken)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));

		var value = await GetByKeyCoreAsync(storageType, key, cancellationToken);
		return (value is not null, value);
	}

	public async ValueTask<(bool Success, TValue Value)> TryGetValueAsync<TValue>(BrowserStorageType storageType, string key, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken)
	{
		var (success, json) = await TryGetStringValueAsync(storageType, key, cancellationToken);
		return success
			? (true, JsonSerializer.Deserialize<TValue>(json, ResolveJsonSerializerOptions(jsonSerializerOptions)))
			: (false, default);
	}

	public async ValueTask RemoveAsync(BrowserStorageType storageType, string key, CancellationToken cancellationToken)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));

		try
		{
			await jsRuntime.InvokeVoidAsync(GetStorageName(storageType) + ".removeItem", cancellationToken, key);
		}
		catch (JSException exception) when (IsStorageNotAvailable(storageType, exception))
		{
			throw CreateNotAvailableException(storageType, exception);
		}
	}

	public async ValueTask ClearAsync(BrowserStorageType storageType, CancellationToken cancellationToken)
	{
		try
		{
			await jsRuntime.InvokeVoidAsync(GetStorageName(storageType) + ".clear", cancellationToken);
		}
		catch (JSException exception) when (IsStorageNotAvailable(storageType, exception))
		{
			throw CreateNotAvailableException(storageType, exception);
		}
	}

	public async ValueTask<int> GetLengthAsync(BrowserStorageType storageType, CancellationToken cancellationToken)
	{
		try
		{
			return await jsRuntime.InvokeAsync<int>("HavitBlazorStorage.getLength", cancellationToken, GetStorageName(storageType));
		}
		catch (JSException exception) when (IsStorageNotAvailable(storageType, exception))
		{
			throw CreateNotAvailableException(storageType, exception);
		}
	}

	public async ValueTask<string> GetKeyByIndexAsync(BrowserStorageType storageType, int index, CancellationToken cancellationToken)
	{
		var key = await GetKeyByIndexCoreAsync(storageType, index, cancellationToken);
		return key ?? throw CreateIndexOutOfRangeException(storageType, index);
	}

	public async ValueTask<(bool Success, string Key)> TryGetKeyByIndexAsync(BrowserStorageType storageType, int index, CancellationToken cancellationToken)
	{
		var key = await GetKeyByIndexCoreAsync(storageType, index, cancellationToken);
		return (key is not null, key);
	}

	public async ValueTask SetStringValueAsync(BrowserStorageType storageType, string key, string value, CancellationToken cancellationToken)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));
		Contract.Requires<ArgumentNullException>(value is not null);

		try
		{
			await jsRuntime.InvokeVoidAsync(GetStorageName(storageType) + ".setItem", cancellationToken, key, value);
		}
		catch (JSException exception) when (IsStorageNotAvailable(storageType, exception))
		{
			throw CreateNotAvailableException(storageType, exception);
		}
	}

	public async ValueTask SetValueAsync<TValue>(BrowserStorageType storageType, string key, TValue value, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));
		Contract.Requires<ArgumentNullException>(value is not null);

		await SetStringValueAsync(storageType, key, JsonSerializer.Serialize(value, ResolveJsonSerializerOptions(jsonSerializerOptions)), cancellationToken);
	}

	public async ValueTask<string> GetStringValueAsync(BrowserStorageType storageType, string key, CancellationToken cancellationToken)
	{
		Contract.Requires<ArgumentException>(!string.IsNullOrWhiteSpace(key));

		var value = await GetByKeyCoreAsync(storageType, key, cancellationToken);
		return value ?? throw CreateKeyNotFoundException(storageType, key);
	}

	public async ValueTask<TValue> GetValueAsync<TValue>(BrowserStorageType storageType, string key, JsonSerializerOptions jsonSerializerOptions, CancellationToken cancellationToken)
	{
		var json = await GetStringValueAsync(storageType, key, cancellationToken);
		return JsonSerializer.Deserialize<TValue>(json, ResolveJsonSerializerOptions(jsonSerializerOptions));
	}

	private string GetByKeyCore(BrowserStorageType storageType, string key)
	{
		var jsInProcessRuntime = GetInProcessRuntime();
		try
		{
			return jsInProcessRuntime.Invoke<string>(GetStorageName(storageType) + ".getItem", key);
		}
		catch (JSException ex) when (IsStorageNotAvailable(storageType, ex))
		{
			throw CreateNotAvailableException(storageType, ex);
		}
	}

	private async ValueTask<string> GetByKeyCoreAsync(BrowserStorageType storageType, string key, CancellationToken cancellationToken)
	{
		try
		{
			return await jsRuntime.InvokeAsync<string>(GetStorageName(storageType) + ".getItem", cancellationToken, key);
		}
		catch (JSException ex) when (IsStorageNotAvailable(storageType, ex))
		{
			throw CreateNotAvailableException(storageType, ex);
		}
	}

	private string GetKeyByIndexCore(BrowserStorageType storageType, int index)
	{
		var jsInProcessRuntime = GetInProcessRuntime();
		try
		{
			return jsInProcessRuntime.Invoke<string>(GetStorageName(storageType) + ".key", index);
		}
		catch (JSException ex) when (IsStorageNotAvailable(storageType, ex))
		{
			throw CreateNotAvailableException(storageType, ex);
		}
	}

	private async ValueTask<string> GetKeyByIndexCoreAsync(BrowserStorageType storageType, int index, CancellationToken cancellationToken)
	{
		try
		{
			return await jsRuntime.InvokeAsync<string>(GetStorageName(storageType) + ".key", cancellationToken, index);
		}
		catch (JSException ex) when (IsStorageNotAvailable(storageType, ex))
		{
			throw CreateNotAvailableException(storageType, ex);
		}
	}

	private IJSInProcessRuntime GetInProcessRuntime()
	{
		if (jsRuntime is IJSInProcessRuntime jsInProcessRuntime)
		{
			return jsInProcessRuntime;
		}

		throw new InvalidOperationException($"Synchronous browser storage access requires '{nameof(IJSInProcessRuntime)}' which is available only in the WebAssembly render mode. Use the asynchronous methods instead.");
	}

	private JsonSerializerOptions ResolveJsonSerializerOptions(JsonSerializerOptions jsonSerializerOptions)
	{
		return jsonSerializerOptions ?? _options.JsonSerializerOptions;
	}

	private static string GetStorageName(BrowserStorageType storageType)
	{
		return storageType switch
		{
			BrowserStorageType.Local => "localStorage",
			BrowserStorageType.Session => "sessionStorage",
			_ => throw new ArgumentOutOfRangeException(nameof(storageType), storageType, null)
		};
	}

	private static bool IsStorageNotAvailable(BrowserStorageType storageType, JSException exception)
	{
		return exception.Message.Contains($"Failed to read the '{GetStorageName(storageType)}' property from 'Window'", StringComparison.OrdinalIgnoreCase) // Chrome DOMException
			|| exception.Message.Contains("can't access property", StringComparison.OrdinalIgnoreCase); // Firefox and Safari - localStorage and sessionStorage are null
	}

	private static StorageKeyNotFoundException CreateKeyNotFoundException(BrowserStorageType storageType, string key)
	{
		return new StorageKeyNotFoundException($"The key '{key}' was not found in the browser {GetStorageName(storageType)}.");
	}

	private static StorageNotAvailableException CreateNotAvailableException(BrowserStorageType storageType, JSException innerException)
	{
		return new StorageNotAvailableException($"The browser {GetStorageName(storageType)} is not accessible. It might be disabled or blocked by the browser settings.", innerException);
	}

	private static StorageIndexOutOfRangeException CreateIndexOutOfRangeException(BrowserStorageType storageType, int index)
	{
		return new StorageIndexOutOfRangeException($"The index '{index}' is out of range of the keys present in the browser {GetStorageName(storageType)}.");
	}
}