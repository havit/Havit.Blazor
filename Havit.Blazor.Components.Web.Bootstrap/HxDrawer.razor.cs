using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public partial class HxDrawer
	{
		[Parameter]
		public string Title { get; set; }

		[Parameter]
		public bool IsOpen { get; set; }

		[Parameter]
		// TODO: TitleTemplate nebo TitleSection? Bude mít vůbec význam?
		public RenderFragment TitleSection { get; set; }
		[Parameter]
		public RenderFragment BodySection { get; set; }

		[Parameter]
		public RenderFragment CommandsSection { get; set; }

	}
}