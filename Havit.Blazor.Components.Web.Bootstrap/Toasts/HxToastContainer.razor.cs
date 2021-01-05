using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxToastContainer : ComponentBase
	{
		[Parameter] public HxToastContainerPosition Position { get; set; }
		[Parameter] public RenderFragment ChildContent { get; set; }

		private string GetPositionCss()
		{
			// https://getbootstrap.com/docs/5.0/utilities/position/#center-elements
			return this.Position switch
			{
				HxToastContainerPosition.TopStart => "position-absolute top-0 start-0",
				HxToastContainerPosition.TopCenter => "position-absolute start-50 translate-middle-x",
				HxToastContainerPosition.TopEnd => "position-absolute top-0 end-0",
				HxToastContainerPosition.MiddleStart => "position-absolute top-50 start-0 translate-middle-y",
				HxToastContainerPosition.MiddleCenter => "position-absolute top-50 start-50 translate-middle",
				HxToastContainerPosition.MiddleEnd => "position-absolute top-50 end-0 translate-middle-y",
				HxToastContainerPosition.BottomStart => "position-absolute bottom-0 start-0",
				HxToastContainerPosition.BottomCenter => "position-absolute bottom-0 start-50 translate-middle-x",
				HxToastContainerPosition.BottomEnd => "position-absolute bottom-0 end-0",
				HxToastContainerPosition.None => null,
				_ => throw new InvalidOperationException($"Unknown {nameof(Position)} value.")
			};
		}
	}
}
