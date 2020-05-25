using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web
{
	public class CollectionRegistration<TItem>
	{
		private readonly ICollection<TItem> collection;
		private readonly Action stageHasChangedAction;

		public CollectionRegistration(ICollection<TItem> collection, Action stageHasChangedAction)
		{
			this.collection = collection;
			this.stageHasChangedAction = stageHasChangedAction;
		}
		public void Register(TItem item)
		{
			collection.Add(item);
			stageHasChangedAction();
		}

		public void Unregister(TItem item)
		{
			collection.Remove(item);
#if DEBUG
			try
			{
				stageHasChangedAction();
			}
			catch (System.ObjectDisposedException)
			{
				// NOOP
			}
#else
			stageHasChangedAction();
#endif
		}
	}
}
