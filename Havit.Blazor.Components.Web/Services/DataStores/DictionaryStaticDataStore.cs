namespace Havit.Blazor.Components.Web.Services.DataStores;

/// <summary>
/// Abstract base-class for implementation of your own dictionary-style static data store.
/// </summary>
/// <remarks>
/// Uses in-memory Dictionary to store the data.
/// Does not preload data, the data get loaded within first data-retrieval call.
/// Does not implement any memory-release logic, the data get refreshed within data-retrievals where <see cref="ShouldRefresh"/> returns <c>true</c>.
/// </remarks>
public abstract class DictionaryStaticDataStore<TKey, TValue> : IDictionaryStaticDataStore<TKey, TValue>
{
	protected Dictionary<TKey, TValue> Data;

	/// <summary>
	/// Template method to implement the data retrieval logic.
	/// You should never call this method directly, use <see cref="EnsureDataAsync"/> to load data.
	/// This method is sequential (does not allow parallel runs), just take care of the data retrieval.
	/// Must return non-<c>null</c> value, use Enumerable.Empty if needed.
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
	/// Retrieves value from dictionary (includes data load if needed). Throws exception when key not found.
	/// </summary>
	public async Task<TValue> GetByKeyAsync(TKey key)
	{
		await EnsureDataAsync();
		return GetByKey(key);
	}

	/// <summary>
	/// Retrieves value from dictionary (requires <see cref="EnsureDataAsync"/> to be called first). Throws exception when key not found.
	/// </summary>
	public TValue GetByKey(TKey key, bool throwIfNotLoaded = false)
	{
		Contract.Requires<ArgumentNullException>(key is not null);

		if (VerifyIsLoaded(throwIfNotLoaded))
		{
			return this.Data[key];
		}
		return default;
	}

	/// <summary>
	/// Retrieves value from dictionary (includes data load if needed). Returns <c>default</c> when not found.
	/// </summary>
	public async Task<TValue> GetByKeyOrDefaultAsync(TKey key, TValue defaultValue = default)
	{
		await EnsureDataAsync();
		return GetByKeyOrDefault(key, defaultValue);
	}

	/// <summary>
	/// Retrieves value from dictionary (requires <see cref="EnsureDataAsync"/> to be called first). Returns <c>defaultValue</c> when not found.
	/// </summary>
	public TValue GetByKeyOrDefault(TKey key, TValue defaultValue = default, bool throwIfNotLoaded = false)
	{
		Contract.Requires<ArgumentNullException>(key is not null);

		if (VerifyIsLoaded(throwIfNotLoaded))
		{
			if (this.Data.TryGetValue(key, out var value))
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
		this.Data = null;
	}

	/// <summary>
	/// To be called before any data-retrieval to load/refresh the data.<br/>
	/// Is automatically called before all asynchronous data-retrieval calls.
	/// You have to call this method on your own (e.g. in <c>OnInitializedAsync</c>) before calling any sychronous API.<br/>
	/// Uses <see cref="ShouldRefresh"/> to check for refreshment request.
	/// Uses lock to prevent multiple parallel loads.
	/// </summary>
	public async Task EnsureDataAsync()
	{
		if ((Data is null) || ShouldRefresh())
		{
			await loadLock.WaitAsync(); // basic lock (Monitor) is thread-based and cannot be used for async code
			try
			{
				if ((Data is null) || ShouldRefresh()) // do not use previous ShouldRefresh result, the data might got refreshed in meantime
				{
					var rawData = await LoadDataAsync();
					Contract.Requires<InvalidOperationException>(rawData is not null, $"{nameof(LoadDataAsync)} is required to return non-null value. Use Enumerable.Empty if needed.");
					this.Data = rawData.ToDictionary(this.KeySelector);
				}
			}
			finally
			{
				loadLock.Release();
			}
		}
	}
	private readonly SemaphoreSlim loadLock = new SemaphoreSlim(1, 1);

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
