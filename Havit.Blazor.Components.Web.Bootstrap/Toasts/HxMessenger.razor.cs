namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see cref="HxToastContainer"/> wrapper for displaying <see cref="HxToast"/> messages dispatched through <see cref="IHxMessengerService"/>.<br />
	/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxMessenger">https://havit.blazor.eu/components/HxMessenger</see>
	/// </summary>
	public partial class HxMessenger : ComponentBase, IDisposable
	{
		/// <summary>
		/// Position of the messages. Default is <see cref="ToastContainerPosition.None"/>.
		/// </summary>
		[Parameter] public ToastContainerPosition Position { get; set; } = ToastContainerPosition.None;

		/// <summary>
		/// Fires when a message (a toast) is hidden by close-button click or through auto-hide.
		/// </summary>
		[Parameter] public EventCallback<MessengerMessage> OnMessageClosed { get; set; }

		/// <summary>
		/// Additional CSS class.
		/// </summary>
		[Parameter] public string CssClass { get; set; }

		[Inject] protected IHxMessengerService Messenger { get; set; }
		[Inject] protected NavigationManager NavigationManager { get; set; }

		private List<MessengerMessage> messages = new List<MessengerMessage>();

		protected override void OnInitialized()
		{
			Messenger.OnMessage += HandleMessage;
			Messenger.OnClear += HandleClear;
		}

		private void HandleMessage(MessengerMessage message)
		{
			InvokeAsync(() =>
			{
				messages.Add(message);

				StateHasChanged();
			});
		}

		private void HandleClear()
		{
			InvokeAsync(() =>
			{
				messages.Clear();

				StateHasChanged();
			});
		}

		/// <summary>
		/// Receive notification from <see cref="HxToast"/> when message is hidden.
		/// </summary>
		private async void HandleToastHidden(MessengerMessage message)
		{
			messages.Remove(message);
			await OnMessageClosed.InvokeAsync(message);
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
}
