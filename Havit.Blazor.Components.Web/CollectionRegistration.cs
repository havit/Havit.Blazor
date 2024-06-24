namespace Havit.Blazor.Components.Web;

/// <summary>
/// Allows child components to register with an owner component via this class shared as a cascading value &amp; parameter.
/// </summary>
/// <remarks>
/// Constructor.
/// </remarks>
/// <param name="collection">Collection to add/remove components.</param>
/// <param name="stateHasChangedAction">Action to call the StateHasChanged method. Due to a server prerendering issue in .NET 5, the StateHasChanged must be called inside InvokeAsync, so the action is awaitable.</param>
/// <param name="isOwnerDisposedFunc">Function to return if the holding component is disposed.</param>
/// <param name="itemAddedCallback">Callback to call when the component is added to the collection.</param>
/// <param name="itemRemovedCallback">Callback to call when the component is removed from the collection.</param>
public class CollectionRegistration<TItem>(
	ICollection<TItem> collection,
	Func<Task> stateHasChangedAction,
	Func<bool> isOwnerDisposedFunc,
	Action<TItem> itemAddedCallback = null,
	Action<TItem> itemRemovedCallback = null)
{
	private readonly ICollection<TItem> _collection = collection;
	private readonly Func<Task> _stateHasChangedAction = stateHasChangedAction;
	private readonly Func<bool> _isOwnerDisposedFunc = isOwnerDisposedFunc;
	private readonly Action<TItem> _itemAddedCallback = itemAddedCallback;
	private readonly Action<TItem> _itemRemovedCallback = itemRemovedCallback;

	/// <summary>
	/// Registers with the collection and calls the state has changed.
	/// </summary>
	public void Register(TItem item)
	{
		_collection.Add(item);
		_itemAddedCallback?.Invoke(item);
#pragma warning disable VSTHRD110 // Observe result of async calls
		// TODO: Consider redesign
		_stateHasChangedAction?.Invoke();
#pragma warning restore VSTHRD110 // Observe result of async calls
	}

	/// <summary>
	/// Unregisters from the collection and calls the state has changed (when the owner is not disposed).
	/// </summary>
	public async Task UnregisterAsync(TItem item)
	{
		_collection.Remove(item);
		_itemRemovedCallback?.Invoke(item);
		if (!_isOwnerDisposedFunc())
		{
			if (_stateHasChangedAction != null)
			{
				try
				{
					await _stateHasChangedAction.Invoke();
				}
				catch (ObjectDisposedException)
				{
					// NOOP
				}
			}
		}
	}
}
