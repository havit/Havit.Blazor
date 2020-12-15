using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Component wrapper displaying <see cref="HxToast"/> to show messages.
	/// </summary>
	public partial class HxMessenger : ComponentBase, IDisposable
	{
		[Inject] public IMessenger Messenger { get; set; }
		[Inject] public NavigationManager NavigationManager { get; set; }

		[Parameter] public bool RemoveMessagesOnNavigation { get; set; } = true;

		private List<MessengerMessage> messages = new List<MessengerMessage>();

		protected override void OnInitialized()
		{
			base.OnInitialized();

			Messenger.OnMessage += Messenger_OnMessage;
			if (RemoveMessagesOnNavigation)
			{
				NavigationManager.LocationChanged += NavigationManager_LocationChanged_RemoveMessagesOnNavigation;
			}
		}

		private void NavigationManager_LocationChanged_RemoveMessagesOnNavigation(object sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
		{
			InvokeAsync(() =>
			{
				messages.Clear();
				StateHasChanged();
			});
		}

		private void Messenger_OnMessage(MessengerMessage message)
		{
			InvokeAsync(() =>
			{
				messages.Add(message);

				StateHasChanged();
			});
		}

		/// <summary>
		/// Receive notification from javascript when message is hidden.
		/// </summary>
		protected void HandleToastHidden(MessengerMessage message)
		{
			messages.Remove(message);
		}

		public void Dispose()
		{
			Messenger.OnMessage -= Messenger_OnMessage;
			if (RemoveMessagesOnNavigation)
			{
				NavigationManager.LocationChanged -= NavigationManager_LocationChanged_RemoveMessagesOnNavigation;
			}
		}
	}
}
