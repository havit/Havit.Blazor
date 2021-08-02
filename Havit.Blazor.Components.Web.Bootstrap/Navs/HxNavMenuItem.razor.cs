using System;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxNavMenuItem : ComponentBase
	{
		[Parameter] public IconBase Icon { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string Text { get; set; }
		[Parameter] public string Href { get; set; }
		[Parameter] public bool IsSubItem { get; set; } = false;
		[Parameter] public string CssClass { get; set; }

		[CascadingParameter(Name = "IsDesktopCollapsed")] public bool IsDesktopCollapsed { get; set; }

		private string id = "hx" + Guid.NewGuid().ToString("N");
	}
}
