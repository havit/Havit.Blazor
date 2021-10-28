using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Triggers for <see cref="HxTooltip"/>.
	/// </summary>
	[Flags]
	public enum TooltipTrigger
	{
		Click = 1,
		Hover = 2,
		Focus = 4,
		Manual = 8
	}
}
