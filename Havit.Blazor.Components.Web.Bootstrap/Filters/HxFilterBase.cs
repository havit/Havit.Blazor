using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Filters
{
	public abstract class HxFilterBase<TValue> : ComponentBase
	{
		[Parameter]
		public string Label { get; set; }

		[Parameter]
		public RenderFragment LabelTemplate { get; set; }

		[Parameter]
		public TValue Value { get; set; }

		[Parameter]
		public EventCallback<TValue> ValueChanged { get; set; }
	}
}
