using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web
{
	public class CollectionRegistration<TItem>
	{
		private readonly ICollection<TItem> collection;
		private readonly Action stateHasChangedAction;
		private readonly Func<bool> isOwnerDisposedAction;

		public CollectionRegistration(ICollection<TItem> collection, Action stateHasChangedAction, Func<bool> isOwnerDisposedAction)
		{
			this.collection = collection;
			this.stateHasChangedAction = stateHasChangedAction;
			this.isOwnerDisposedAction = isOwnerDisposedAction;
		}

		public void Register(TItem item)
		{
			collection.Add(item);
			stateHasChangedAction();
		}

		public void Unregister(TItem item)
		{
			collection.Remove(item);
			if (!isOwnerDisposedAction())
			{
#if DEBUG
				try
				{
					stateHasChangedAction();
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
}
