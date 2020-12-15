using System;
using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Propagating access to <see cref="HxMessenger"/> as <see cref="IMessenger" />.
	/// </summary>
	internal class Messenger : IMessenger
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