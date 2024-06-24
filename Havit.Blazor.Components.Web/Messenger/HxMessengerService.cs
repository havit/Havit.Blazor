namespace Havit.Blazor.Components.Web;

/// <summary>
/// Propagates access to HxMessenger as <see cref="IHxMessengerService" />.
/// </summary>
internal class HxMessengerService : IHxMessengerService
{
	/// <inheritdoc cref="IHxMessengerService.OnMessage" />
	public event Action<MessengerMessage> OnMessage;

	/// <inheritdoc cref="IHxMessengerService.OnClear" />
	public event Action OnClear;

	/// <inheritdoc cref="IHxMessengerService.AddMessage(MessengerMessage)" />
	public void AddMessage(MessengerMessage message)
	{
		OnMessage?.Invoke(message);
	}

	/// <inheritdoc cref="IHxMessengerService.Clear" />
	public void Clear()
	{
		OnClear?.Invoke();
	}
}