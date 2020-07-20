using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap.Toasts
{
	/// <summary>
	/// Cascading value propagating access to <see cref="HxMessenger"/> as <see cref="IMessenger" />.
	/// </summary>
	internal class MessengerCascadingValue : IMessenger
	{
		private HxMessenger messengerComponent;

		/// <summary>
		/// Constructor.
		/// </summary>
		public MessengerCascadingValue(HxMessenger messengerComponent)
		{
			this.messengerComponent = messengerComponent;
		}

		/// <inheritdoc />
		public void AddMessage(Message message)
		{
			messengerComponent.AddMessage(message);
		}
	}
}