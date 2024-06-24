namespace Havit.Blazor.Components.Web.Services.DataStores;

/// <summary>
/// Abstract base-class for implementation of your own dictionary-style static data store.
/// </summary>
/// <remarks>
/// Uses an in-memory Dictionary to store the data.
/// Does not preload data; the data is loaded within the first data-retrieval call.
/// Does not implement any memory-release logic; the data is refreshed within data-retrievals where <see cref="ShouldRefresh"/> returns <c>true</c>.
/// </remarks>
public abstract class DictionaryStaticDataStore<TKey, TValue> : IDictionaryStaticDataStore<TKey, TValue>
{
	protected Dictionary<TKey, TValue> Data;

	/// <summary>
	/// Template method to implement the data retrieval logic.
	/// You should never call this method directly; use <see cref="EnsureDataAsync"/> to load data.
	/// This method is sequential (does not allow parallel runs); just take care of the data retrieval.
	/// Must return a non-<c>null</c> value; use Enumerable.Empty if needed.
	/// </summary>
	protected internal abstract Task<IEnumerable<TValue>> LoadDataAsync();

	/// <summary>
	/// Template selector to be used when building the dictionary from retrieved data.
	/// </summary>
	protected internal abstract Func<TValue, TKey> KeySelector { get; }

	/// <summary>
	/// Indicates whether the store has valid data.
	/// </summary>
	public bool IsLoaded => (Data is not null) && !ShouldRefresh();

	/// <summary>
	/// Template method to implement your own logic of data expiration/refresh.
	/// Gets called within all data-retrieval calls to check for refreshment need.<br />
	/// Can be implemented as "<c>=> false</c>" if you do not want the data to ever expire.
	/// </summary>
	protected internal abstract bool ShouldRefresh();

	/// <summary>
	/// Returns all data from the store (includes data load if needed).
	/// </summary>
	public async Task<IEnumerable<TValue>> GetAllAsync()
	{
		await EnsureDataAsync();
		return GetAll(throwIfNotLoaded: true);
	}

	/// <summary>
	/// Returns all data from the store (requires <see cref="EnsureDataAsync"/> to be called first).
	/// </summary>
	public IEnumerable<TValue> GetAll(bool throwIfNotLoaded = false)
	{
		if (VerifyIsLoaded(throwIfNotLoaded))
		{
			return Data.Values;
		}
		return null;
	}

	/// <summary>
	/// Retrieves a value from the dictionary (includes data load if needed). Throws an exception when the key is not found.
	/// </summary>
	public async Task<TValue> GetByKeyAsync(TKey key)
	{
		await EnsureDataAsync();
		return GetByKey(key);
	}

	/// <summary>
	/// Retrieves a value from the dictionary (requires <see cref="EnsureDataAsync"/> to be called first). Throws an exception when the key is not found.
	/// </summary>
	public TValue GetByKey(TKey key, bool throwIfNotLoaded = false)
	{
		Contract.Requires<ArgumentNullException>(key is not null);

		if (VerifyIsLoaded(throwIfNotLoaded))
		{
			return Data[key];
		}
		return default;
	}

	/// <summary>
	/// Retrieves a value from the dictionary (includes data load if needed). Returns <c>default</c> when not found.
	/// </summary>
	public async Task<TValue> GetByKeyOrDefaultAsync(TKey key, TValue defaultValue = default)
	{
		await EnsureDataAsync();
		return GetByKeyOrDefault(key, defaultValue);
	}

	/// <summary>
	/// Retrieves a value from the dictionary (requires <see cref="EnsureDataAsync"/> to be called first). Returns <c>defaultValue</c> when not found.
	/// </summary>
	public TValue GetByKeyOrDefault(TKey key, TValue defaultValue = default, bool throwIfNotLoaded = false)
	{
		Contract.Requires<ArgumentNullException>(key is not null);

		if (VerifyIsLoaded(throwIfNotLoaded))
		{
			if (Data.TryGetValue(key, out var value))
			{
				return value;
			}
		}
		return defaultValue;
	}

	/// <summary>
	/// Throws away all the data.
	/// </summary>
	public void Clear()
	{
		Data = null;
	}

	/// <summary>
	/// To be called before any data-retrieval to load/refresh the data.<br/>
	/// Is automatically called before all asynchronous data-retrieval calls.
	/// You have to call this method on your own (e.g. in <c>OnInitializedAsync</c>) before calling any synchronous API.<br/>
	/// Uses <see cref="ShouldRefresh"/> to check for refreshment request.
	/// Uses a lock to prevent multiple parallel loads.
	/// </summary>
	public async Task EnsureDataAsync()
	{
		if ((Data is null) || ShouldRefresh())
		{
			await _loadLock.WaitAsync(); // basic lock (Monitor) is thread-based and cannot be used for async code
			try
			{
				if ((Data is null) || ShouldRefresh()) // do not use the previous ShouldRefresh result; the data might have been refreshed in the meantime
				{
					var rawData = await LoadDataAsync();
					Contract.Requires<InvalidOperationException>(rawData is not null, $"{nameof(LoadDataAsync)} is required to return a non-null value. Use Enumerable.Empty if needed.");
					Data = rawData.ToDictionary(KeySelector);
				}
			}
			finally
			{
				_loadLock.Release();
			}
		}
	}
	private readonly SemaphoreSlim _loadLock = new SemaphoreSlim(1, 1);

	private bool VerifyIsLoaded(bool throwIfNotLoaded)
	{
		if (Data is not null)
		{
			return true;
		}
		if (throwIfNotLoaded)
		{
			throw new InvalidOperationException($"Data not loaded. You have to call {nameof(EnsureDataAsync)} first.");
		}
		return false;
	}
}
