using System;
using System.Collections.Generic;

namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Propagating access to HxMessenger as <see cref="IHxMessengerService" />.
	/// </summary>
	internal class HxMessengerService : IHxMessengerService
	{
		/// <inheritdoc />
		public event Action<MessengerMessage> OnMessage;

		/// <inheritdoc />
		public event Action OnClear;

		/// <inheritdoc />
		public void AddMessage(MessengerMessage message)
		{
			OnMessage?.Invoke(message);
		}

		/// <inheritdoc />
		public void Clear()
		{
			OnClear?.Invoke();
		}
	}
}