using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Messenger message.
	/// </summary>
	public class MessengerMessage
	{
		/// <summary>
		/// Key. Used for component paring during rendering (@key).
		/// </summary>
		public string Key { get; } = Guid.NewGuid().ToString("N");

		/// <summary>
		/// Bootstrap icon.
		/// </summary>
		public IconBase Icon { get; set; }

		/// <summary>
		/// Css class.
		/// </summary>
		public string CssClass { get; set; }

		/// <summary>
		/// Delay in milliseconds to autohide message.
		/// </summary>
		public int? AutohideDelay { get; set; }

		/// <summary>
		/// Message text.
		/// </summary>
		public string Text { get; set; }
	}
}
