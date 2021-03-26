using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public record ContextMenuItemSkin
	{
		/// <summary>
		/// Item text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Item icon (use <see cref="BootstrapIcon" />).
		/// </summary>
		public IconBase Icon { get; set; }

		/// <summary>
		/// Displays <see cref="HxMessageBox" /> to get a confirmation.
		/// </summary>
		public string ConfirmationQuestion { get; set; }

		/// <summary>
		/// Button resource type.
		/// </summary>
		public Type ResourceType { get; set; }
	}
}
