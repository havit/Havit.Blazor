using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;

namespace Havit.Blazor.Components.Web.Services.DataStores
{
	/// <summary>
	/// Abstract base-class for implementation of your own dictionary-style static data store.
	/// </summary>
	/// <remarks>
	/// Uses in-memory Dictionary to store the data.
	/// Does not preload data, the data get loaded within first data-retriaval call.
	/// Does not implement any memory-release logic, the data get refreshed within data-retrivals where <see cref="ShouldRefresh"/> returns <c>true</c>.
	/// </remarks>
	public abstract class DictionaryStaticDataStore<TKey, TValue> : IDictionaryStaticDataStore<TKey, TValue>
	{
		protected Dictionary<TKey, TValue> Data;

		/// <summary>
		/// Template method to implement the data retrival logic.
		/// You should never call this method directly, use <see cref="EnsureDataAsync"/> to load data.
		/// This method is sequential (does not allow parallel runs), just take care of the data retrieval.
		/// Must return non-null value, use Enumerable.Empty if needed.
		/// </summary>
		protected internal abstract Task<IEnumerable<TValue>> LoadDataAsync();

		/// <summary>
		/// Template selector to be used when building the dictionary from retrieved data.
		/// </summary>
		protected internal abstract Func<TValue, TKey> KeySelector { get; }

		/// <summary>
		/// Template method to implement your own logic of data expiration/refresh.
		/// Gets called within all data-retrieval calls to check for refreshment need.
		/// </summary>
		protected internal abstract bool ShouldRefresh();

		public async Task<IEnumerable<TValue>> GetAllAsync()
		{
			await EnsureDataAsync();

			return Data.Values;
		}

		/// <summary>
		/// Retrieves value from dictionary. Throws exception when not found.
		/// </summary>
		public async Task<TValue> GetByKeyAsync(TKey key)
		{
			Contract.Requires<ArgumentNullException>(key is not null);

			await EnsureDataAsync();

			return this.Data[key];
		}

		/// <summary>
		/// Retrieves value from dictionary. Returns <c>default</c> when not found.
		/// </summary>
		public async Task<TValue> TryGetByKeyAsync(TKey key)
		{
			Contract.Requires<ArgumentNullException>(key is not null);

			await EnsureDataAsync();

			if (this.Data.TryGetValue(key, out var value))
			{
				return value;
			}
			return default;
		}

		/// <summary>
		/// Throws away all the data.
		/// </summary>
		public void Clear()
		{
			this.Data = null;
		}

		/// <summary>
		/// To be called before any data-retrival to load/refresh the data.
		/// Uses <see cref="ShouldRefresh"/> to check for refreshment request.
		/// Uses lock to prevent multiple parallel loads.
		/// </summary>
		protected internal async Task EnsureDataAsync()
		{
			if ((Data is null) || ShouldRefresh())
			{
				try
				{
					await loadLock.WaitAsync(); // basic lock (Monitor) is thread-based and cannot be used for async code

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
		private readonly SemaphoreSlim loadLock = new SemaphoreSlim(1);
	}
}
