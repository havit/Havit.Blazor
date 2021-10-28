using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Triggers for <see cref="HxPopover"/>.
	/// </summary>
	[Flags]
	public enum PopoverTrigger
	{
		Click = TooltipTrigger.Click,
		Hover = TooltipTrigger.Hover,
		Focus = TooltipTrigger.Focus,
		Manual = TooltipTrigger.Manual
	}
}
