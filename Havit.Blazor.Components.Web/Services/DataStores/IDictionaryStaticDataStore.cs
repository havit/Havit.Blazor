namespace Havit.Blazor.Components.Web.Services.DataStores;

/// <summary>
/// Base interface for Blazor dictionary-based static data stores.
/// </summary>
public interface IDictionaryStaticDataStore<TKey, TValue>
{
	/// <summary>
	/// To be called before any data retrieval to load/refresh the data.<br/>
	/// Is automatically called before all asynchronous data retrieval calls.
	/// You have to call this method on your own (e.g. in <c>OnInitializedAsync</c>) before calling any synchronous API.<br/>
	/// </summary>
	Task EnsureDataAsync();

	/// <summary>
	/// Indicates whether the store has data, i.e. whether the synchronous data-retrieval methods can be used.
	/// The data might be stale (when <c>ShouldRefresh</c> returns <c>true</c>); it gets refreshed within the next asynchronous data-retrieval call (or <c>EnsureDataAsync</c>).
	/// </summary>
	bool IsLoaded { get; }

	/// <summary>
	/// Returns all data from the store (includes data load if needed).
	/// </summary>
	Task<IEnumerable<TValue>> GetAllAsync();

	/// <summary>
	/// Returns all data from the store (requires <see cref="EnsureDataAsync"/> to be called first).
	/// </summary>
	IEnumerable<TValue> GetAll(bool throwIfNotLoaded = false);

	/// <summary>
	/// Retrieves value from dictionary (includes data load if needed). Throws an exception when the key is not found.
	/// </summary>
	Task<TValue> GetByKeyAsync(TKey key);

	/// <summary>
	/// Retrieves value from dictionary (requires <see cref="EnsureDataAsync"/> to be called first). Throws an exception when the key is not found.
	/// </summary>
	TValue GetByKey(TKey key, bool throwIfNotLoaded = false);

	/// <summary>
	/// Retrieves value from dictionary (includes data load if needed). Returns <c>default</c> when not found.
	/// </summary>
	Task<TValue> GetByKeyOrDefaultAsync(TKey key, TValue defaultValue = default);

	/// <summary>
	/// Retrieves value from dictionary (requires <see cref="EnsureDataAsync"/> to be called first). Returns <c>defaultValue</c> when not found.
	/// </summary>
	TValue GetByKeyOrDefault(TKey key, TValue defaultValue = default, bool throwIfNotLoaded = false);

	/// <summary>
	/// Discards all the data.
	/// </summary>
	void Clear();

	/// <summary>
	/// Refreshes the data. Discards all the data (if any) and loads it again.
	/// </summary>
	Task RefreshDataAsync();
}
