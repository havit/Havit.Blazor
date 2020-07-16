using System.Collections.Generic;

namespace Havit.Blazor.Components.Web.Bootstrap.Toasts
{
	// TODO: Naming?
	public class Messenger : IMessenger
	{
		private HxMessenger messengerComponent;

		public Messenger(HxMessenger messengerComponent)
		{
			this.messengerComponent = messengerComponent;
		}

		public void AddMessage(Message message)
		{
			messengerComponent.AddMessage(message);
		}
		
	}
}