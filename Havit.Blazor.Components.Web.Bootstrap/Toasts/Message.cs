using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.Toasts
{
	public class Message
	{
		public string Key { get; } = Guid.NewGuid().ToString("N");
		public BootstrapIcon? Icon { get; set; }
		public string CssClass { get; set; }
		public int? AutohideDelay { get; set; }
		public string Text { get; set; }
	}
}
