using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Services
{
	/* TODO
	 *  - ICodebooksStore / IStaticDataStore? ...myslím to může obsahovat i jiné položky než číselníky, ale bude pak stejný jejich životní cyklus?
	 *			- ICodebooksStore by mohl lépe pracovat s IEnumerable<T> na rozhraní, IStaticDataStore nejspíš musí být generičtější
	 *	- Jeden store pro všechny, nebo store per dataset?
	 *	- Identifikace data-setu klíčem, nebo jen typem?
	 *	- Ukládání do localStorage?
	 *			- Pouze tam nebo i in-memory? (asi ano)
	 *			- localStorage 
	 */
	public interface IStaticDataStore
	{
		Task<T> GetAsync<T>();
		T Get<T>();  // synchronní blokující verze? má smysl?

		void Register<T>(Func<Task<T>> provider, RegistrationOptions options);  // stačí Func, nebo potřebujeme Request+Result?
		void Register<T>(T data, RegistrationOptions options);

		/// <summary>
		/// Unregisters the data from the store.
		/// </summary>
		void Remove<T>();

		/// <summary>
		/// Removes current data from the store. The data get loaded within the next Get().
		/// </summary>
		void Reset<T>();

		/// <summary>
		/// Removes current data and loads a fresh set from the provider.
		/// </summary>
		Task Reload<T>();

		void ClearAll();

		/// <summary>
		/// Starts the store and loads the data marked for auto-load.
		/// </summary>
		Task Start();  // nechceme nějaký mass-load jedním server requestem?
	}

	public struct RegistrationOptions
	{
		public TimeSpan? Expiration { get; set; }

		public bool LoadOnStart { get; set; }
	}
}
