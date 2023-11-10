namespace Havit.Blazor.Components.Web.Services.DataStores;

/// <summary>
/// Abstract base-class for implementation of your own single-value static data store.
/// </summary>
/// <remarks>
/// Uses in-memory static field to store the data.
/// Does not preload data, the data get loaded within first data-retrieval call.
/// Does not implement any memory-release logic, the data get refreshed within data-retrievals where <see cref="ShouldRefresh"/> returns <c>true</c>.
/// </remarks>
public abstract class StaticDataStore<TValue> : IStaticDataStore<TValue>
{
	protected bool HasData;
	protected TValue Data;

	/// <summary>
	/// Template method to implement the data retrieval logic.
	/// You should never call this method directly, use <see cref="EnsureDataAsync"/> to load data.
	/// This method is sequential (does not allow parallel runs), just take care of the data retrieval.
	/// Must return non-<c>default</c> value.
	/// </summary>
	protected internal abstract Task<TValue> LoadDataAsync();

	/// <summary>
	/// Indicates whether the store has valid data.
	/// </summary>
	public bool IsLoaded => HasData && !ShouldRefresh();

	/// <summary>
	/// Template method to implement your own logic of data expiration/refresh.
	/// Gets called within all data-retrieval calls to check for refreshment need.<br />
	/// Can be implemented as "<c>=> false</c>" if you do not want the data to ever expire.
	/// </summary>
	protected internal abstract bool ShouldRefresh();

	/// <summary>
	/// Returns value from the store (includes data load if needed).
	/// </summary>
	public async Task<TValue> GetValueAsync()
	{
		await EnsureDataAsync();
		return GetValue();
	}

	/// <summary>
	/// Returns value from the store (requires <see cref="EnsureDataAsync"/> to be called first).
	/// </summary>
	public TValue GetValue(bool throwIfNotLoaded = false)
	{
		if (VerifyIsLoaded(throwIfNotLoaded))
		{
			return Data;
		}
		return default;
	}

	/// <summary>
	/// Throws away all the data.
	/// </summary>
	public void Clear()
	{
		this.HasData = false;
		this.Data = default;
	}

	/// <summary>
	/// To be called before any data-retrieval to load/refresh the data.<br/>
	/// Is automatically called before all asynchronous data-retrieval calls.
	/// You have to call this method on your own (e.g. in <c>OnInitializedAsync</c>) before calling any synchronous API.<br/>
	/// Uses <see cref="ShouldRefresh"/> to check for refreshment request.
	/// Uses lock to prevent multiple parallel loads.
	/// </summary>
	public async Task EnsureDataAsync()
	{
		if (!HasData || ShouldRefresh())
		{
			await loadLock.WaitAsync(); // basic lock (Monitor) is thread-based and cannot be used for async code
			try
			{
				if (!HasData || ShouldRefresh()) // do not use previous ShouldRefresh result, the data might got refreshed in meantime
				{
					this.Data = await LoadDataAsync();
					this.HasData = true;
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
