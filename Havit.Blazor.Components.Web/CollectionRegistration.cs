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
		private readonly Action<TItem> itemAddedCallback;
		private readonly Action<TItem> itemRemovedCallback;

		/// <summary>
		/// Constructor.
		/// </summary>
		public CollectionRegistration(ICollection<TItem> collection, Action stateHasChangedAction, Func<bool> isOwnerDisposedAction, Action<TItem> itemAddedCallback = null, Action<TItem> itemRemovedCallback = null)
		{
			this.collection = collection;
			this.stateHasChangedAction = stateHasChangedAction;
			this.isOwnerDisposedAction = isOwnerDisposedAction;
			this.itemAddedCallback = itemAddedCallback;
			this.itemRemovedCallback = itemRemovedCallback;
		}

		/// <summary>
		/// Registers to the collection and call state has changed.
		/// </summary>
		public void Register(TItem item)
		{
			collection.Add(item);
			itemAddedCallback?.Invoke(item);
			stateHasChangedAction();
		}

		/// <summary>
		/// Unregisters from the collection and call state has changed (when owner is not disposed).
		/// </summary>
		public void Unregister(TItem item)
		{
			collection.Remove(item);
			itemRemovedCallback?.Invoke(item);
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
