using System.Collections.Generic;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Services.DataStores
{
	public interface IDictionaryStaticDataStore<TKey, TValue>
	{
		void Clear();
		Task<IEnumerable<TValue>> GetAllAsync();
		Task<TValue> GetByKeyAsync(TKey key);
		Task<TValue> TryGetByKeyAsync(TKey key);
	}
}