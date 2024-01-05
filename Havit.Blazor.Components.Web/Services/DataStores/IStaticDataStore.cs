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
	/// Indicates whether the store has valid data.
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
}
