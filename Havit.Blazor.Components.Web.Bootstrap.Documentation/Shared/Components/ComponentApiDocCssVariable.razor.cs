using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
	public partial class ComponentApiDocCssVariable
	{
		[Parameter] public string Name { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }
	}
}
