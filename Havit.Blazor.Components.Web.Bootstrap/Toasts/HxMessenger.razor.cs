namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// <see cref="HxToastContainer"/> wrapper for displaying <see cref="HxToast"/> messages dispatched through <see cref="IHxMessengerService"/>.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxMessenger">https://havit.blazor.eu/components/HxMessenger</see>
/// </summary>
public partial class HxMessenger : ComponentBase, IDisposable
{
	/// <summary>
	/// Position of the messages. The default value is <see cref="ToastContainerPosition.None"/>.
	/// </summary>
	[Parameter] public ToastContainerPosition Position { get; set; } = ToastContainerPosition.None;

	/// <summary>
	/// Additional CSS class.
	/// </summary>
	[Parameter] public string CssClass { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto the underlying <c>div.toast-container</c> element
	/// (e.g. <c>data-bs-theme="dark"</c> to render all the messages in dark mode).
	/// To render a single message in dark mode, use <see cref="MessengerMessage.AdditionalAttributes"/>.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	[Inject] protected IHxMessengerService Messenger { get; set; }
	[Inject] protected NavigationManager NavigationManager { get; set; }

	private List<MessengerMessage> _messages = new List<MessengerMessage>();

	protected override void OnInitialized()
	{
		Messenger.OnMessage += HandleMessage;
		Messenger.OnClear += HandleClear;
	}

	private void HandleMessage(MessengerMessage message)
	{
		_ = InvokeAsync(() =>
		{
			_messages.Add(message);

			StateHasChanged();
		});
	}

	private void HandleClear()
	{
		_ = InvokeAsync(() =>
		{
			_messages.Clear();

			StateHasChanged();
		});
	}

	/// <summary>
	/// Receive notification from <see cref="HxToast"/> when the message is hidden.
	/// </summary>
	private void HandleToastHidden(MessengerMessage message)
	{
		_messages.Remove(message);
	}

	public void Dispose()
	{
		Dispose(true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			Messenger.OnMessage -= HandleMessage;
			Messenger.OnClear -= HandleClear;
		}
	}
}
