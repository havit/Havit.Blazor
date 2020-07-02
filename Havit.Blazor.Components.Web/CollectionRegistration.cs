using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Allows child components to register to an owner component via this class shared as cascading value &amp; parameter.
	/// </summary>
	public class CollectionRegistration<TItem>
	{
		private readonly ICollection<TItem> collection;
		private readonly Action stateHasChangedAction;
		private readonly Func<bool> isOwnerDisposedAction;

		/// <summary>
		/// Constructor.
		/// </summary>
		public CollectionRegistration(ICollection<TItem> collection, Action stateHasChangedAction, Func<bool> isOwnerDisposedAction)
		{
			this.collection = collection;
			this.stateHasChangedAction = stateHasChangedAction;
			this.isOwnerDisposedAction = isOwnerDisposedAction;
		}

		/// <summary>
		/// Registers to the collection and call state has changed.
		/// </summary>
		public void Register(TItem item)
		{
			collection.Add(item);
			stateHasChangedAction();
		}

		/// <summary>
		/// Unregisters from the collection and call state has changed (when owner is not disposed).
		/// </summary>
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
				stateHasChangedAction();
#endif
			}
		}
	}
}
