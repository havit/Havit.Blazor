using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Toasts
{
	public interface IMessenger
	{
		public void AddMessage(Message message);
	}
}
