using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Provides methods for adding and showing message. Use <see cref="MessengerExtensions">extension methods</see>.
	/// </summary>
	public interface IMessenger
	{
		/// <summary>
		/// Subscription seam for HxMessenger component to be able to receive the messages.
		/// </summary>
		public event Action<MessengerMessage> OnMessage;

		/// <summary>
		/// Adds and shows message.
		/// </summary>
		public void AddMessage(MessengerMessage message);
	}
}
