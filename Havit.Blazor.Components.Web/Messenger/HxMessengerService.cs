using System;
using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Messenger
{
	/// <summary>
	/// Propagating access to <see cref="HxMessenger"/> as <see cref="IHxMessengerService" />.
	/// </summary>
	internal class HxMessengerService : IHxMessengerService
	{
		/// <inheritdoc />
		public event Action<MessengerMessage> OnMessage;

		/// <inheritdoc />
		public void AddMessage(MessengerMessage message)
		{
			OnMessage?.Invoke(message);
		}
	}
}