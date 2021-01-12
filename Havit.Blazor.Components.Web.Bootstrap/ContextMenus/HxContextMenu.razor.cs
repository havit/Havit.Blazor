using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxContextMenu
	{
		[Parameter]	public RenderFragment ChildContent { get; set; }
	}
}
