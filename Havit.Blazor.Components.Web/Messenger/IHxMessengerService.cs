using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web
{
	/// <summary>
	/// Provides methods for adding and showing message. Additional extension methods available in concrete implementation.
	/// </summary>
	public interface IHxMessengerService
	{
		/// <summary>
		/// Subscription seam for HxMessenger component to be able to receive the messages.
		/// </summary>
		public event Action<MessengerMessage> OnMessage;

		/// <summary>
		/// Subscription seam for HxMessenger component to be able to receive Clear() command.
		/// </summary>
		public event Action OnClear;

		/// <summary>
		/// Adds and shows message.
		/// </summary>
		public void AddMessage(MessengerMessage message);

		/// <summary>
		/// Removes all messages.
		/// </summary>
		public void Clear();
	}
}
