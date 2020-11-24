using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Provides methods for adding and showing message. For better API use <see cref="MessengerExtensions">extension methods</see>.
	/// Consume interface as cascading parameter in <see cref="HxMessenger"/> which must be present in the layout component (or any parent component).
	/// </summary>
	public interface IMessenger
	{
		/// <summary>
		/// Adds and shows message.
		/// </summary>
		public void AddMessage(Message message);
	}
}
