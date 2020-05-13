using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public class CollectionRegistration<TItem>
	{
		private readonly ICollection<TItem> collection;

		public CollectionRegistration(ICollection<TItem> collection)
		{
			this.collection = collection;
		}
		public void Register(TItem item)
		{
			collection.Add(item);
		}

		public void Unregister(TItem item)
		{
			collection.Remove(item);
		}
	}
}
