using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public static class ContextMenuItemSkins
	{
		public static ContextMenuItemSkin Delete = new ContextMenuItemSkin() { Icon = BootstrapIcon.Trash, ResourceType = typeof(ContextMenuItemSkins), Text = "Delete", ConfirmationQuestion = "DeleteConfirmation" };
	}
}
