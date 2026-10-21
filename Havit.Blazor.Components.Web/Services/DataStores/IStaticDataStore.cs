namespace Havit.Blazor.Components.Web.Services.DataStores;

public interface IStaticDataStore<TValue>
{

	/// <summary>
	/// To be called before any data-retrieval to load/refresh the data.<br/>
	/// Is automatically called before all asynchronous data-retrieval calls.
	/// You have to call this method on your own (e.g. in <c>OnInitializedAsync</c>) before calling any synchronous API.<br/>
	/// </summary>
	Task EnsureDataAsync();

	/// <summary>
	/// Indicates whether the store has data, i.e. whether the synchronous data-retrieval methods can be used.
	/// The data might be stale (when <c>ShouldRefresh</c> returns <c>true</c>); it gets refreshed within the next asynchronous data-retrieval call (or <c>EnsureDataAsync</c>).
	/// </summary>
	bool IsLoaded { get; }

	/// <summary>
	/// Discards all the data.
	/// </summary>
	void Clear();

	/// <summary>
	/// Retrieves a value from the store (requires <see cref="EnsureDataAsync"/> to be called first).
	/// </summary>
	TValue GetValue(bool throwIfNotLoaded = false);

	/// <summary>
	/// Retrieves a value from the store (includes data load if needed).
	/// </summary>
	Task<TValue> GetValueAsync();

	/// <summary>
	/// Refreshes the data. Discards all the data (if any) and loads it again.
	/// </summary>
	Task RefreshDataAsync();
}
