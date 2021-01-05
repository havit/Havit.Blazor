using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	// TODO: Odstranit?
	public abstract class HxFilterBase : ComponentBase
	{
		[Parameter] public string Label { get; set; }

		[Parameter] public RenderFragment LabelTemplate { get; set; }
	}
}
