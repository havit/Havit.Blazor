using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxCarouselCaption
	{
		[Parameter] public RenderFragment ChildContent { get; set; }
		[Parameter] public string CssClass { get; set; }
	}
}
