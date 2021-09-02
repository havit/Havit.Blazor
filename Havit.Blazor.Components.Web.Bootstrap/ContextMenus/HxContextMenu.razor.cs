using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Ready-made context menu (based on <see href="https://getbootstrap.com/docs/5.1/components/dropdowns/">Bootstrap Dropdown</see>).
	/// </summary>
	public partial class HxContextMenu
	{
		[Parameter] public RenderFragment ChildContent { get; set; }
	}
}
