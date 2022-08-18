namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Allows child components to register to an owner component via this class shared as cascading value &amp; parameter.
	/// </summary>
	public class CollectionRegistration<TItem>
	{
		private readonly ICollection<TItem> collection;
		private readonly Func<Task> stateHasChangedAction;
		private readonly Func<bool> isOwnerDisposedFunc;
		private readonly Action<TItem> itemAddedCallback;
		private readonly Action<TItem> itemRemovedCallback;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="collection">Collection to add/remove components.</param>
		/// <param name="stateHasChangedAction">Action to call StateHasChanged method. Due to server prerendering issue in .NET 5, the StateHasChanged must be called inside InvokeAsync, so the action is awaitable.</param>
		/// <param name="isOwnerDisposedFunc">Function to return if the holding component is disposed.</param>
		/// <param name="itemAddedCallback">Callback to call when the component is added to the collection.</param>
		/// <param name="itemRemovedCallback">Callback to call when the component is removed from thecollection.</param>
		public CollectionRegistration(ICollection<TItem> collection, Func<Task> stateHasChangedAction, Func<bool> isOwnerDisposedFunc, Action<TItem> itemAddedCallback = null, Action<TItem> itemRemovedCallback = null)
		{
			this.collection = collection;
			this.stateHasChangedAction = stateHasChangedAction;
			this.isOwnerDisposedFunc = isOwnerDisposedFunc;
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
			stateHasChangedAction?.Invoke();
		}

		/// <summary>
		/// Unregisters from the collection and call state has changed (when owner is not disposed).
		/// </summary>
		public async Task UnregisterAsync(TItem item)
		{
			collection.Remove(item);
			itemRemovedCallback?.Invoke(item);
			if (!isOwnerDisposedFunc())
			{
				if (stateHasChangedAction != null)
				{
					try
					{
						await stateHasChangedAction.Invoke();
					}
					catch (ObjectDisposedException)
					{
						// NOOP
					}
				}
			}
		}
	}
}
